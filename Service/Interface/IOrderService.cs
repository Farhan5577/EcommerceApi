using EcommerceApi.DTOs;
namespace EcommerceApi.Service.Interface
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrder(OrderDto dto, Guid UserId);
        Task<List<OrderResponseDto>> GetMyOrder(Guid UserId);
        Task<OrderResponseDto> GetOrderById(Guid OrderId, Guid UserId);
        Task<OrderResponseDto> UpdateOrderStatus(Guid OrderId, UpdateOrderDto dto, Guid UserId);
    }
}
