using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Service.Interface;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Options.Exceptions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Data;

namespace EcommerceApi.Service
{
    public sealed class StoreService(AppDbContext _context, IPhotoService photoService) : IStoreService
    {
        public async Task<StoreResponseDto> CreateStore (StoreDto dto, Guid UserId)
        {
            var exitingStore = await _context.Stores.FirstOrDefaultAsync (s => s.UserId == UserId);
            if (exitingStore != null)
                throw new ConflictException("User already has a store!");
            string photoUrl = "https://via.placeholder.com/150";

            if (dto.LogoFile != null)
            {
                var uploadResult = await photoService.AddPhoto(dto.LogoFile);
                photoUrl = uploadResult.SecureUrl.ToString();
            }

            var store = new StoreModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                UserId = UserId,
                LogoUrl = photoUrl,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            _context.Stores.Add (store);
            await _context.SaveChangesAsync ();

            return Mapping(store);
        }

        public async Task<StoreResponseDto?> GetStoreByUserId(Guid userId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.UserId == userId);
            if (store == null) throw new NotFoundException("Store not found!");

            return Mapping(store);
        }

        public async Task<StoreResponseDto?> GetStoreById (Guid storeId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);
            if (store == null) throw new NotFoundException($"Store with Id {storeId} Not Found!");

            return Mapping(store);
        }

        private static StoreResponseDto Mapping(StoreModel store)
        {
            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description?? string.Empty,
                UserId = store.UserId,
                LogoUrl = store.LogoUrl
            };
        }
    }
}
