using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EcommerceApi.Data
{
    public class StoreConfiguration : IEntityTypeConfiguration<StoreModel>
    {
        public void Configure(EntityTypeBuilder<StoreModel> builder) 
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(300);
            builder.Property(x => x.LogoUrl).HasMaxLength(200);
            builder.Property(x => x.LogoPublicId).HasMaxLength(200);

            builder.HasOne(s => s.User).WithOne(u => u.Store).HasForeignKey<StoreModel>(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
