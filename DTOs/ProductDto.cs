using System.ComponentModel.DataAnnotations;


namespace EcommerceApi.DTOs
{
    public record ProductDto 
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required, Range(0.01,double.MaxValue, ErrorMessage = " The price must be greater than 0.")]
        public required decimal Price { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public required IFormFile? PhotoFile { get; set; }
    }

    public record ModProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }

    public record ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? PhotoUrl { get; set; }
        public Guid StoreId { get; set; }
    }
}
