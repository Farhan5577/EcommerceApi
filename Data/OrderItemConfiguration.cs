using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceApi.Data
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemModel>
    {
        public void Configure(EntityTypeBuilder<OrderItemModel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PriceAtPurchase).HasPrecision(18, 2);

            builder.HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductModelId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
