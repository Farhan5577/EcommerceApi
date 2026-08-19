using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data
{
    public class UserConfiguration:IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> bulder)
        {
            bulder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            bulder.Property(x => x.Bio).HasMaxLength(300);
            bulder.Property(x => x.LogoUrl).HasMaxLength(300);
            bulder.Property(x => x.LogoPublicId).HasMaxLength(300);
            bulder.Property(x => x.Role).IsRequired().HasMaxLength(100);
        }
    }
}
