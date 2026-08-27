using Microsoft.AspNetCore.Identity;

namespace EcommerceApi.Models
{
    public class UserModel : IdentityUser<Guid>
    {
        public required string Name { get; set; }
        public string? Bio {  get; set; }
        public string? Phone { get; set; }
        public string? LogoUrl { get; set; }
        public string? LogoPublicId { get; set; }
        public required string Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public StoreModel? Store { get; set; }

    }
}
