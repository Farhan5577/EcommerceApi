using System.ComponentModel.DataAnnotations;
namespace EcommerceApi.DTOs
{
    public record RegisterDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }
        [Required]
        public required string UserName { get; set; }
    }
}
