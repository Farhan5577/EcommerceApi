using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.DTOs
{
    public record OrderItemDto
    {
        public Guid ProductId { get; set; }
        public string? NameStore { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => PriceAtPurchase * Quantity;
    }

    public record OrderItemRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Minimum quantity of 1 item")]
        public required int Quantity { get; set; }
    }
}
