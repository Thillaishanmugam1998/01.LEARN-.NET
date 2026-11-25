using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DTOs;
using ProductManagementApi.Services;

namespace ProductManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            _logger.LogInformation("Retrieved {CategoryCount} categories", categories.Count());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved category {CategoryId} ({CategoryName})", category.CategoryId, category.CategoryName);
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> PostCategory(CategoryCreateDto dto)
        {
            var created = await _categoryService.CreateCategoryAsync(dto);
            _logger.LogInformation("Created new category {CategoryName} with ID {CategoryId}", created.CategoryName, created.CategoryId);
            return CreatedAtAction(nameof(GetCategory), new { id = created.CategoryId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryUpdateDto dto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, dto);
            if (!updated)
            {
                _logger.LogWarning("Failed to update: Category with ID {CategoryId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Updated category with ID {CategoryId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete: Category with ID {CategoryId} not found", id);
                return NotFound();
            }

            _logger.LogWarning("Deleted category with ID {CategoryId}", id);
            return NoContent();
        }
    }
}
