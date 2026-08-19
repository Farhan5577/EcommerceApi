using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderModel> 
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ShippingAddress).HasMaxLength(500);

            builder.HasOne(b => b.Buyer).WithMany(o => o.Orders).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
