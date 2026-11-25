using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [MaxLength(50)]
        public string? SKU { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }

    public class ProductUpdateDto
    {
        [Required]
        [MaxLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [MaxLength(50)]
        public string? SKU { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }

    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? SKU { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
