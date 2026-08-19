using EcommerceApi.DTOs;
namespace EcommerceApi.Service.Interface
{
    public interface IStoreService
    {
        Task<StoreResponseDto> CreateStore(StoreDto dto, Guid Userid);
        Task<StoreResponseDto?> GetStoreByUserId(Guid UserId);
        Task<StoreResponseDto?> GetStoreById(Guid StoreId);
    }
}
