using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DTOs;
using ProductManagementApi.Services;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            _logger.LogInformation("Retrieved {ProductCount} products", products.Count());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved product {ProductId} ({ProductName})", product.ProductId, product.ProductName);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> PostProduct(ProductCreateDto dto)
        {
            var created = await _productService.CreateProductAsync(dto);
            _logger.LogInformation("Created new product {ProductName} with ID {ProductId} at price {Price}", created.ProductName, created.ProductId, created.Price);
            return CreatedAtAction(nameof(GetProduct), new { id = created.ProductId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto dto)
        {
            var updated = await _productService.UpdateProductAsync(id, dto);
            if (!updated)
            {
                _logger.LogWarning("Failed to update: Product with ID {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Updated product with ID {ProductId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete: Product with ID {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogWarning("Deleted product with ID {ProductId}", id);
            return NoContent();
        }
    }
}
