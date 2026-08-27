namespace EcommerceApi.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public int Stock { get; set; }
        public required string PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public StoreModel? Store { get; set; } = null!;


    }
}
