using EcommerceApi.DTOs;

namespace EcommerceApi.Service.Interface
{
    public interface IAuthService
    {
        Task<RegisterDto> Register(RegisterDto dto);
        Task<ResponseToken> Login(LoginDto dto);
        Task<ResponseToken> RefreshToken(TokenDto dto);
    }
}
