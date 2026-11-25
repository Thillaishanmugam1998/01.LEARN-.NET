using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;

namespace ProductManagementApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToResponseDto);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToResponseDto(product);
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                SKU = dto.SKU,
                CategoryId = dto.CategoryId
            };

            var created = await _productRepository.AddAsync(product);
            var fullProduct = await _productRepository.GetByIdAsync(created.ProductId);
            return MapToResponseDto(fullProduct ?? created);
        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.ProductName = dto.ProductName;
            existing.Price = dto.Price;
            existing.StockQuantity = dto.StockQuantity;
            existing.SKU = dto.SKU;
            existing.CategoryId = dto.CategoryId;

            await _productRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) return false;

            await _productRepository.DeleteAsync(existing);
            return true;
        }

        private static ProductResponseDto MapToResponseDto(Product p) => new()
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            SKU = p.SKU,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.CategoryName
        };
    }
}
