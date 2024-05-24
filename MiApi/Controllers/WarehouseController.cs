using FluentValidation;
using MiApi.DtoS;
using MiApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private ICommonService<WarehouseDto, WarehouseInsertDto, WarehouseUpdateDto> _warehouseService;
        private IValidator<WarehouseInsertDto> _warehouseInsertValidator;
        private IValidator<WarehouseUpdateDto> _wareHouseUpdateValidator;

        public WarehouseController(
            [FromKeyedServices("warehouseService")]ICommonService<WarehouseDto, WarehouseInsertDto, WarehouseUpdateDto> warehouseService, 
            IValidator<WarehouseInsertDto> warehouseInsertValidator, 
            IValidator<WarehouseUpdateDto> wareHouseUpdateValidator
                                  )
        {
            _warehouseService = warehouseService;
            _warehouseInsertValidator = warehouseInsertValidator;
            _wareHouseUpdateValidator = wareHouseUpdateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<WarehouseDto>> Get()
        {
            var warehouse = await _warehouseService.GetAllAsync();

            return warehouse != null ? Ok(warehouse) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseDto>> GetById(int id)
        {
            var warehouse = await _warehouseService.FindByIdAsync(id);

            return warehouse == null ? NotFound() : Ok(warehouse);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> Add(WarehouseInsertDto warehouseInsertDto)
        {
            var validationResult = _warehouseInsertValidator.Validate(warehouseInsertDto);
            if(!validationResult.IsValid) return BadRequest(validationResult.Errors);
            if (!_warehouseService.Validate(warehouseInsertDto)) return BadRequest(_warehouseService.Errors);

            var warehouse = await _warehouseService.AddAsync(warehouseInsertDto);

            return CreatedAtAction(nameof(GetById), new {id = warehouse.Id, warehouse});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WarehouseDto>> Update(int id, WarehouseUpdateDto warehouseUpdateDto)
        {
            var validationResult = _wareHouseUpdateValidator.Validate(warehouseUpdateDto);
            if(!validationResult.IsValid ) return BadRequest(validationResult.Errors);
            if(!_warehouseService.Validate(warehouseUpdateDto)) return BadRequest(_warehouseService.Errors);

            var warehouse = await _warehouseService.Update(id,warehouseUpdateDto);

            return warehouse == null ? NotFound($"{warehouseUpdateDto.Id} no encontrado") : Ok(warehouse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<WarehouseDto>>Delete(int id)
        {
            var warehouse = await _warehouseService.Delete(id);

            return warehouse == null ? NotFound() : Ok(warehouse);
        }
    }
}
