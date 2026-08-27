namespace EcommerceApi.Models
{
    public class StoreModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? LogoPublicId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public List<ProductModel> products { get; set; } = new List<ProductModel>();
        public UserModel User { get; set; } = null!;

    }
}
