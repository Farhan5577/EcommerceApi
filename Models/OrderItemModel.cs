namespace EcommerceApi.Models
{
    public class OrderItemModel
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductModelId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase {  get; set; }

        public OrderModel? Order { get; set; }
        public ProductModel? Product { get; set; }



    }
}
