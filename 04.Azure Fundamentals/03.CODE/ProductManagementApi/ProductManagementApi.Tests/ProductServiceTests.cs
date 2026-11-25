using Moq;
using ProductManagementApi.DTOs;
using ProductManagementApi.Models;
using ProductManagementApi.Repositories;
using ProductManagementApi.Services;

namespace ProductManagementApi.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            var products = new List<Product>
            {
                new() { ProductId = 1, ProductName = "Laptop", Price = 1500, StockQuantity = 5, CategoryId = 1, Category = new Category { CategoryId = 1, CategoryName = "Electronics" } },
                new() { ProductId = 2, ProductName = "Mouse", Price = 25, StockQuantity = 20, CategoryId = 1, Category = new Category { CategoryId = 1, CategoryName = "Electronics" } }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = (await _service.GetAllProductsAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].ProductName);
            Assert.Equal("Electronics", result[0].CategoryName);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
        {
            var product = new Product { ProductId = 1, ProductName = "Laptop", Price = 1500, StockQuantity = 5, CategoryId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.GetProductByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.ProductId);
            Assert.Equal("Laptop", result.ProductName);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

            var result = await _service.GetProductByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProductAsync_ReturnsCreatedProduct()
        {
            var createDto = new ProductCreateDto { ProductName = "Keyboard", Price = 75, StockQuantity = 10, SKU = "KB-01", CategoryId = 1 };
            var created = new Product { ProductId = 10, ProductName = "Keyboard", Price = 75, StockQuantity = 10, SKU = "KB-01", CategoryId = 1 };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(created);
            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(created);

            var result = await _service.CreateProductAsync(createDto);

            Assert.Equal(10, result.ProductId);
            Assert.Equal("Keyboard", result.ProductName);
            Assert.Equal(75, result.Price);
        }

        [Fact]
        public async Task DeleteProductAsync_ReturnsTrue_WhenExists()
        {
            var product = new Product { ProductId = 1, ProductName = "Laptop" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockRepo.Setup(r => r.DeleteAsync(product)).Returns(Task.CompletedTask);

            var result = await _service.DeleteProductAsync(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(product), Times.Once);
        }
    }
}
