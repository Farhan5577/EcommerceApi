using System.ComponentModel.DataAnnotations;
namespace EcommerceApi.DTOs
{
    public record TokenDto
    {
        [Required]
        public required string AccsessToken { get; set; }
        [Required]
        public required string RefreshToken { get; set; }

    }
    public record ResponseToken
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTimeOffset RefreshTokenExpiration { get; set; }
    }
}
