using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CategoryUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;
    }

    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
