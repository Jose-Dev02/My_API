using AutoMapper;
using MiApi.DTOs;
using MiApi.Models;
using MiApi.Repository;
using Microsoft.IdentityModel.Tokens;

namespace MiApi.Service
{
    public class WarehouseService : ICommonService<WarehouseDto, WarehouseInsertDto, WarehouseUpdateDto>
    {
        private readonly IRepository<Warehouse> _warehouseRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }

        public WarehouseService([FromKeyedServices("warehouseRepository")] IRepository<Warehouse> warehouseRepository,
                                IMapper mapper
                                )
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            Errors = [];
        }

        public async Task<WarehouseDto> AddAsync(WarehouseInsertDto warehouseInsertDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseInsertDto);
            await _warehouseRepository.AddAsync(warehouse);
            await _warehouseRepository.Save();

            return _mapper.Map<WarehouseDto>(warehouse);
            
        }

        public async Task<WarehouseDto> Delete(int id)
        {
             var warehouse = await _warehouseRepository.FindByIdAsync(id);

            if(warehouse != null)
            {
                _warehouseRepository.Delete(warehouse);
                await _warehouseRepository.Save();

                return _mapper.Map<WarehouseDto>(warehouse);    
            }
            return null;
        }

        public async Task<WarehouseDto> FindByIdAsync(int id)
        {
            var warehouse = await _warehouseRepository.FindByIdAsync(id);

            return warehouse != null ? _mapper.Map<WarehouseDto>(warehouse) : null;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllAsync()
        {
            var warehouse = await _warehouseRepository.GetAllAsync();

            return !warehouse.IsNullOrEmpty() ? warehouse.Select(_mapper.Map<WarehouseDto>) : null ;
        }

        public async Task<WarehouseDto> Update(int id, WarehouseUpdateDto warehouseUpdateDto)
        {
            var warehouse = await _warehouseRepository.FindByIdAsync(id);

            if(warehouse != null)
            {
                warehouse = _mapper.Map(warehouseUpdateDto, warehouse);
                _warehouseRepository.Update(warehouse);
                await _warehouseRepository.Save();

                return _mapper.Map<WarehouseDto>(warehouse);
            }
            return null;
        }

        public bool Validate(WarehouseInsertDto dto)
        {
           if(_warehouseRepository.Search(b => b.ProductId == dto.ProductId).Any())
            {
                Errors.Add("Already Exist.");
                return false;
            }
            return true;
        }

        public bool Validate(WarehouseUpdateDto dto)
        {
            if (_warehouseRepository.Search(b => b.Id != dto.Id && b.ProductId == dto.ProductId).Any())
            {
                Errors.Add("Already Exist.");
                return false;
            }
            return true;
        }
    }
}
