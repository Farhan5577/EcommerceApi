using EcommerceApi.DTOs;

namespace EcommerceApi.Service.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
