using FluentValidation;
using MiApi.DTOs;
using MiApi.Models;
using MiApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICommonService<CategoryDto, CategoryInsertDto, CategoryUpdateDto> _categoryService;
        private IValidator<CategoryInsertDto> _categoryInsertValidator;
        private IValidator<CategoryUpdateDto> _categoryUpdateValidator;

        public CategoryController(
        [FromKeyedServices("categoryService")] ICommonService<CategoryDto, CategoryInsertDto, CategoryUpdateDto> categoryService,
        IValidator<CategoryInsertDto> categoryInsertValidator,
        IValidator<CategoryUpdateDto> categoryUpdateValidator
                                  )
        {
            _categoryService = categoryService;
            _categoryInsertValidator = categoryInsertValidator;
            _categoryUpdateValidator = categoryUpdateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryDto>> GetAll()
        {
            var category = await _categoryService.GetAllAsync();

            return category == null ? NotFound( new {success = false, message = "Database Error Connection"}) : Ok(new { category, success = true });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.FindByIdAsync(id);

            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Add(CategoryInsertDto categoryInsertDto)
        {
            var validatorResult = _categoryInsertValidator.Validate(categoryInsertDto);

            if (!validatorResult.IsValid) return BadRequest(validatorResult.Errors);
            if(!_categoryService.Validate(categoryInsertDto)) return BadRequest(_categoryService.Errors);

            var category = await _categoryService.AddAsync(categoryInsertDto);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, CategoryUpdateDto categoryUpdateDto)
        {
            var validatorResult = _categoryUpdateValidator.Validate(categoryUpdateDto);
            if (!validatorResult.IsValid) return BadRequest(validatorResult.Errors);
            if(!_categoryService.Validate(categoryUpdateDto)) return BadRequest(_categoryService.Errors);

            var category = await _categoryService.Update(id,categoryUpdateDto);
            return category == null ? NotFound($"{categoryUpdateDto.Id} no encontrado") : Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryDto>> Delete(int id)
        {
            var category = await _categoryService.Delete(id);

            return category == null  ? BadRequest() : Ok(category);
        }
    }
}
