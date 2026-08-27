using EcommerceApi.DTOs;
using EcommerceApi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Options.Exceptions;
namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var message = await _authService.Register(dto);
            return Ok(new { Message = message });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.Login(dto);
            return Ok(new { Token = token, Message = "Login successful!" });
        }
    }
}
