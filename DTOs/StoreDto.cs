using EcommerceApi.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.DTOs
{
    public record StoreDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public required IFormFile? LogoFile { get; set; }
    }

    public record ModStoreDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? LogoPublicId { get; set; }
    }

    public record StoreResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public Guid UserId { get; set; }
    }
}
