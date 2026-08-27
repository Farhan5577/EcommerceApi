using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceApi.Options.Exceptions;

namespace EcommerceApi.Service
{
    public sealed class AuthService(UserManager<UserModel> _userManager, IConfiguration _configuration) : IAuthService
    {

        public async Task<RegisterDto> Register(RegisterDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
            {
                throw new ConflictException("Email is already registered.");
            }

            var user = new UserModel
            {
                Email = dto.Email,
                Name = dto.UserName,
                UserName = dto.UserName,
                Role = "customer"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(".", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"Failed to register : {errors}");
            }

            return dto;
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new BadRequestException("Incorrect email or password.");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(UserModel user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JwtSec = _configuration["Jwt:SecretKey"] ?? "SuperSecretDefaultKeyqwertyuiopasdfghjklz";
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSec));

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"] ?? "EcommerceApi",
                audience: _configuration["Jwt:Audience"] ?? "EcommerceApi",
                expires: DateTime.UtcNow.AddHours(8),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
