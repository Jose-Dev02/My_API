using AutoMapper;
using MiApi.DTOs;
using MiApi.Models;
using MiApi.Repository;
using Microsoft.IdentityModel.Tokens;


namespace MiApi.Service
{
    public class ProductService : ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto>
    {
        private IRepository<Product> _productRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        
        public ProductService([FromKeyedServices("productRepository")]IRepository<Product> productRepository,
                              IMapper mapper) 
        {
            Errors = [];
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddAsync(ProductInsertDto productInsertDto)
        {
            var product = _mapper.Map<Product>(productInsertDto);
            await _productRepository.AddAsync(product);
            await _productRepository.Save();

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<ProductDto> Delete(int id)
        {
            var product = await _productRepository.FindByIdAsync(id);

            if(product != null)
            {
                _productRepository.Delete(product);
                await _productRepository.Save();

                return _mapper.Map<ProductDto>(product);
            }
            return null;
        }

        public async Task<ProductDto> FindByIdAsync(int id)
        {
            var product = await _productRepository.FindByIdAsync(id);

            return product != null ? _mapper.Map<ProductDto>(product) : null;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var producto = await _productRepository.GetAllAsync();

            return !producto.IsNullOrEmpty() ? producto.Select(_mapper.Map<ProductDto>) : null;
        }

        public async Task<ProductDto> Update(int id, ProductUpdateDto productUpdateDto)
        {
            var product = await _productRepository.FindByIdAsync(id);
            
            if(product != null)
            {
                product = _mapper.Map(productUpdateDto, product);
                _productRepository.Update(product);
                await _productRepository.Save();

                var productDto = _mapper.Map<ProductDto>(product);
                return productDto; 
            }
            return null;

        }

        public bool Validate(ProductInsertDto dto)
        {
            if (_productRepository.Search(b => b.Name == dto.Name).Any())
            {
                Errors.Add($"Cannot be repeated elemnts [{dto.Name}].");
                return false;
            }
            return true;
        }

        public bool Validate(ProductUpdateDto dto)
        {
            if (_productRepository.Search(b => b.Name == dto.Name && dto.Id != b.Id).Any())
            {
                Errors.Add($"Cannot be repeated elemnts [{dto.Name}].");
                return false;
            }
            return true;
        }

        public Task<ProductDto> Find(string name)
        {
            throw new NotImplementedException();
        }
    }
}
