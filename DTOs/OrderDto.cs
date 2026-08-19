using EcommerceApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EcommerceApi.DTOs
{
    public record OrderDto
    {
        [Required]
        public required List<OrderItemRequestDto> Items { get; set; } = new();

        public string? ShippingAddress { get; set; }
    }

    public record UpdateOrderDto
    {
        [Required]
        public required string Status { get; set; }
    }

    public record OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        //public string? StoreName { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ShippingAddress { get; set;}
        public List<OrderItemDto>? Items { get; set; }

    }
}