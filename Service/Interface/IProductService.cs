using EcommerceApi.DTOs;

namespace EcommerceApi.Service.Interface
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProduct(ProductDto dto, Guid UserId);
        Task<List<ProductResponseDto>> GetAllProduct();
        Task<ProductResponseDto?> GetAllProductById(Guid ProductId);
        Task<ProductResponseDto> UpdateProduct(Guid ProductId, ModProductDto dto, Guid UserId);
        Task<bool> DeleteProduct(Guid ProductId, Guid UserId);
    }
}
