using EcommerceApi.DTOs;

namespace EcommerceApi.Service.Interface
{
    public interface IAuthService
    {
        Task<RegisterDto> Register(RegisterDto dto);
        Task<string> Login(LoginDto dto);
    }
}
