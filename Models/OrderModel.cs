namespace EcommerceApi.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset OrderTime { get; set; } = DateTimeOffset.UtcNow;
        public decimal TotalAmount { get; set; } 
        public required string Status { get; set; }
        public string? ShippingAddress { get; set; }
        public UserModel? Buyer { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
