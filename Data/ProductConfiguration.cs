using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EcommerceApi.Data
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).HasColumnType("numeric(18,2)");

            builder.HasOne(p => p.Store).WithMany(s => s.products).HasForeignKey(p => p.StoreId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
