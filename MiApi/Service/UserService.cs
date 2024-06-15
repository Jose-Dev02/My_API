using AutoMapper;
using MiApi.DTOs;
using MiApi.Models;
using MiApi.Repository;
using Microsoft.IdentityModel.Tokens;

namespace MiApi.Service
{
    public class UserService : ICommonService<UserDto, UserInsertDto, UserUpdateDto>
    {

        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }

        public UserService([FromKeyedServices("userRepository")]IRepository<User> repository,
                            IMapper mapper

            )
        {
            _repository = repository;
            _mapper = mapper;
            Errors = [];
        }

  

        public async Task<UserDto> AddAsync(UserInsertDto userInsertDto)
        {
            var user = _mapper.Map<User>(userInsertDto);

            await _repository.AddAsync(user);
            await _repository.Save();
            
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Delete(int id)
        {
            var user = await _repository.FindByIdAsync(id);

            if (user == null) return null;

            _repository.Delete(user);
            await _repository.Save();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> FindByIdAsync(int id)
        {
            var user = await _repository.FindByIdAsync(id);

            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<UserDto> Find(string name)
        {
            var user = await _repository.Find(name);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();

            return !users.IsNullOrEmpty() ? users.Select(_mapper.Map<UserDto>) : null;
        }

        public async Task<UserDto> Update(int id, UserUpdateDto userUpdateDto)
        {
            var user = await _repository.FindByIdAsync(id);

            if (user == null) return null;

            user = _mapper.Map<User>(userUpdateDto);
            _repository.Update(user);
            await _repository.Save();

            return _mapper.Map<UserDto>(userUpdateDto);
        }

        public bool Validate(UserInsertDto userInsertDto)
        {
            if (_repository.Search(b => b.Name == userInsertDto.Name).Any())
            {
                Errors.Add($"Username have been taken {userInsertDto.Name}");
            }
            else if (_repository.Search(b => b.Correo == userInsertDto.Correo).Any())
            {
                Errors.Add($"Email have been taken {userInsertDto.Name}");
            }
            else return true;
            
            return false;
        }

        public bool Validate(UserUpdateDto userUpdatedto)
        {
            if(_repository.Search(b => b.Name == userUpdatedto.Name && b.Id != userUpdatedto.Id).Any())
            {
                Errors.Add($"Username have been taken {userUpdatedto.Name}");
            }else if(_repository.Search(b => b.Correo == userUpdatedto.Correo && b.Id != userUpdatedto.Id).Any())
            {
                Errors.Add($"Email have been taken {userUpdatedto.Correo}");
            }else return true;

            return false;
        }
    }
}
