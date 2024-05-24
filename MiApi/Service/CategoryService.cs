using AutoMapper;
using MiApi.DTOs;
using MiApi.Models;
using MiApi.Repository;
using Microsoft.IdentityModel.Tokens;

namespace MiApi.Service
{
    public class CategoryService : ICommonService<CategoryDto, CategoryInsertDto, CategoryUpdateDto>
    {
        private IRepository<Category> _repositoryCategory;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public CategoryService(
                               [FromKeyedServices("categoryRepository")]IRepository<Category> categoryRepository,
                               IMapper mapper
                               )
        {
            _mapper = mapper;
            _repositoryCategory = categoryRepository;
            Errors = [];
        }



        public async Task<CategoryDto> AddAsync(CategoryInsertDto categoryInsertDto)
        {
            var category = _mapper.Map<Category>(categoryInsertDto);
            await _repositoryCategory.AddAsync(category);
            await _repositoryCategory.Save();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> Delete(int id)
        {
            var category = await _repositoryCategory.FindByIdAsync(id);

            if(category != null)
            {
                _repositoryCategory.Delete(category);
                await _repositoryCategory.Save();

                return _mapper.Map<CategoryDto>(category);
            }
            return null;
        }

        public async Task<CategoryDto> FindByIdAsync(int id)
        {
            var category = await _repositoryCategory.FindByIdAsync(id);

            return category != null ? _mapper.Map<CategoryDto>(category) :  null;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var category = await _repositoryCategory.GetAllAsync();

            return category.IsNullOrEmpty() ? null : category.Select(_mapper.Map<CategoryDto>);

        }

        public async Task<CategoryDto> Update(int id, CategoryUpdateDto categoryUpdateDto)
        {
            var category = await _repositoryCategory.FindByIdAsync(id);
            if(category != null)
            {
                 category = _mapper.Map(categoryUpdateDto, category);
                _repositoryCategory.Update(category);
                await _repositoryCategory.Save();
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return categoryDto;
            }
            return null;
        }

        public bool Validate(CategoryInsertDto categoryInsertdto)
        {
            if(_repositoryCategory.Search(b => b.Name == categoryInsertdto.Name).Any())
            {
                Errors.Add($"Cannot be elements REPEATED {categoryInsertdto.Name}");
                return false;
            }
            return true;
            
        }

        public bool Validate(CategoryUpdateDto dto)
        {
            if(_repositoryCategory.Search(b => b.Name == dto.Name && dto.Id != b.Id).Any()) 
            {
                Errors.Add($"Cannot be elements REPETEAD {dto.Name}");
                return false;
            }
            return true;
        }
    }
}
