using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Service.Interface;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Options.Exceptions;

namespace EcommerceApi.Service
{
    public class OrderService(AppDbContext _context) : IOrderService
    {
        public async Task<OrderResponseDto> CreateOrder(OrderDto dto, Guid UserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newOrder = new OrderModel
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    OrderTime = DateTimeOffset.UtcNow,
                    Status = "Pending!",
                    ShippingAddress =  dto.ShippingAddress ?? "",
                    TotalAmount = 0,  
                };
                decimal totalAmount = 0;
                var orderItems = new List<OrderItemModel>();

                foreach (var item in dto.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    if (product == null) throw new NotFoundException("Product not found");

                    if (product.Stock < item.Quantity) throw new BadRequestException($"Stock {product.Name} not enough");

                    product.Stock -= item.Quantity;

                    decimal subTotal = product.Price * item.Quantity;
                    totalAmount += subTotal;

                    orderItems.Add(new OrderItemModel
                    {
                        Id = Guid.NewGuid() ,
                        OrderId = newOrder.Id,
                        ProductModelId = product.Id,
                        Quantity = item.Quantity,
                        PriceAtPurchase = product.Price,
                    });
                }

                newOrder.TotalAmount = totalAmount;
                _context.Orders.Add(newOrder);
                _context.OrderItems.AddRange(orderItems);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetOrderResponseById(newOrder.Id);

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            
        }

        public async Task<List<OrderResponseDto>> GetMyOrder(Guid UserId)
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(oa => oa.Store).Where( o => o.UserId == UserId).OrderByDescending(o => o.OrderTime).ToListAsync();
            return orders.Select(o => MapToOrderResponseDto(o)).ToList();
        }

        public async Task<OrderResponseDto> GetOrderById(Guid OrderId, Guid UserId)
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(oa => oa.Store).FirstOrDefaultAsync(o => o.Id == OrderId && o.UserId == UserId);
            if (orders == null) throw new NotFoundException($"Order with Id : {OrderId} Not Found1");

            return MapToOrderResponseDto(orders);
        }

        public async Task<OrderResponseDto> UpdateOrderStatus(Guid orderId,UpdateOrderDto dto,Guid userId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(p => p.UserId == userId);
            if (store == null) throw new NotFoundException("You do not have a store.");

            var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == orderId && o.OrderItems.Any(oi => oi.Product.StoreId == store.Id));
            if (order == null) throw new ForbiddenException("Order not found or does not belong to your store!");

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            return await GetOrderResponseById(order.Id);
        }



        private async Task<OrderResponseDto> GetOrderResponseById(Guid OrderId)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstAsync(o => o.Id == OrderId);

            return MapToOrderResponseDto(order);
        }

        private static OrderResponseDto MapToOrderResponseDto (OrderModel order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                //StoreName = 
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductModelId,
                    NameStore = oi.Product?.Store?.Name ?? "Unknown",
                    Quantity = oi.Quantity,
                    ProductName = oi.Product.Name,
                    PriceAtPurchase = oi.Product.Price,

                }).ToList()

            };
        }
    }
}
