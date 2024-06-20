using FluentValidation;
using MiApi.DTOs;
using MiApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace MiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto> _productService;
        private IValidator<ProductInsertDto> _productInsertValidator;
        private IValidator<ProductUpdateDto> _productUpdateValidator;
        private readonly ImgurService _imgurService;

        public ProductsController(IValidator<ProductInsertDto> productInsertValidator,
                                  IValidator<ProductUpdateDto> productUpdateValidator,
                                  ImgurService imgurService,
            [FromKeyedServices("productService")] ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto> productService
                                  )
        {
            _productInsertValidator = productInsertValidator;
            _productUpdateValidator = productUpdateValidator;
            _productService = productService;
            _imgurService = imgurService;
        }

        [HttpGet]
        public async Task<ActionResult<ProductDto>> Get()
        {
            var product = await _productService.GetAllAsync();
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _productService.FindByIdAsync(id);
            return product == null ? NotFound($"No existe el ID:{id}.") : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Add(ProductInsertDto productDto, IFormFile file)
        {
            var validatorResult = await _productInsertValidator.ValidateAsync(productDto);
            if (!validatorResult.IsValid) return BadRequest(new { success = false, message = validatorResult.Errors });
            if (!_productService.Validate(productDto)) return BadRequest(new { success = false, message = _productService.Errors });
            if (file == null) return BadRequest(new { success = false, message = "Picture is Required." });

            var fileName = $"{Guid.NewGuid()}-{file.FileName}";
            var fileUrl = await _imgurService.UploadImageAsync(file.OpenReadStream(), fileName);

            productDto.MediaURL = fileUrl;

            var product = await _productService.AddAsync(productDto);

            //return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

            return Ok(new { success = true, message = " Product was added sucessfuly." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, ProductUpdateDto productDto)
        {
            var validatorResult = await _productUpdateValidator.ValidateAsync(productDto);
            if (!validatorResult.IsValid) return BadRequest(validatorResult.Errors);
            if (!_productService.Validate(productDto)) return BadRequest(_productService.Errors);

            var product = await _productService.Update(id, productDto);

            return product == null ? NotFound($"{productDto.Id} no encontrado") : Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>>Detele(int id)
        {
            var product = await _productService.Delete(id);

            return product == null ? NotFound() : Ok(product); 

        }

        
    }
}
