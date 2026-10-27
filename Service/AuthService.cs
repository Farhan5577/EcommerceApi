using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceApi.Options.Exceptions;
using System.Security.Cryptography;
using EcommerceApi.Migrations;

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

        public async Task<ResponseToken> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new BadRequestException("Incorrect email or password.");

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new ResponseToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiryTime.Value
            };
        }

        public async Task <ResponseToken> RefreshToken(TokenDto dto)
        {
            var principal = GetPrincipalFromExperiedToken(dto.AccsessToken);
            if (principal == null)
                throw new BadRequestException("Invalid access token");

            var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                throw new BadRequestException("Invalid token claims");

            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
                throw new BadRequestException("Invalid or expired refresh token");

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new ResponseToken
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiryTime.Value
            };
        }

        private string GenerateJwtToken(UserModel user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Role, user.Role!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var JwtSec = _configuration["Jwt:SecretKey"] ?? "SuperSecretDefaultKeyqwertyuiopasdfghjklz";
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSec));

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"] ?? "EcommerceApi",
                audience: _configuration["Jwt:Audience"] ?? "EcommerceUser",
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExperiedToken(string token)
        {
            var JwtSec = _configuration["Jwt:SecretKey"] ?? "SuperSecretDefaultKeyqwertyuiopasdfghjklz";
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt :Audience"] ?? "EcommerceUser",
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt :Issuer"] ?? "EcommerceApi",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSec)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Incvalid token algorithm.");
            }
            return principal;
        }


    }
}
