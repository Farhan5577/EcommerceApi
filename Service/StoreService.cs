using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Service.Interface;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Options.Exceptions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace EcommerceApi.Service
{
    public sealed class StoreService(AppDbContext _context) : IStoreService
    {
        public async Task<StoreResponseDto> CreateStore (StoreDto dto, Guid UserId)
        {
            var exitingStore = await _context.Stores.FirstOrDefaultAsync (s => s.UserId == UserId);
            if (exitingStore != null)
                throw new ConflictException("User already has a store!");

            var store = new StoreModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                UserId = UserId,
                LogoUrl = "https://via.placeholder.com/150",
                CreatAt = DateTime.UtcNow,
            };

            _context.Stores.Add (store);
            await _context.SaveChangesAsync ();

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                UserId = store.UserId,
                LogoUrl = store.LogoUrl,
            };
        }

        public async Task<StoreResponseDto?> GetStoreByUserId(Guid userId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.UserId == userId);
            if (store == null) throw new NotFoundException("Store not found!");

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                UserId = store.UserId,
                LogoUrl = store.LogoUrl,
            };
        }

        public async Task<StoreResponseDto?> GetStoreById (Guid storeId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
            if (store == null) throw new NotFoundException($"Store with Id {storeId} Not Found!");

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                UserId = store.UserId,
                LogoUrl = store.LogoUrl,
            };
        }
    }
}
