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
        public DateTime CreatAt { get; set; } = DateTime.UtcNow;
        public StoreModel? Store { get; set; } = null!;


    }
}
