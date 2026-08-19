using EcommerceApi.DTOs;
using EcommerceApi.Data;
using EcommerceApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using EcommerceApi.Service.Interface;
namespace EcommerceApi.Service
{
    public sealed class ProductService (AppDbContext _context, IPhotoService photoService):IProductService
    {
        public async Task<ProductResponseDto> CreateProduct(ProductDto dto, Guid UserId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(a => a.UserId == UserId);
            if (store == null)
                throw new Exception("You must have a store first. ");

            string photoUrl = "https://via.placeholder.com/150";

            if(dto.PhotoFile != null)
            {
                var uploadResult = await photoService.AddPhoto(dto.PhotoFile);
                if(uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                photoUrl = uploadResult.SecureUrl.ToString();
            }

            var product = new ProductModel
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                PhotoUrl = photoUrl,
                StoreId = store.Id,
                CreatAt = DateTime.UtcNow,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return (MapToResponseDto(product));
        }

        public async Task<List<ProductResponseDto>> GetAllProduct()
        {
            var product = await _context.Products.ToListAsync();
            return product.Select(p => MapToResponseDto(p)).ToList();
        }

        public async Task<ProductResponseDto?> GetAllProductById(Guid ProductID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductID);
            if (product == null) return null;

            return MapToResponseDto(product);
        }

        public async Task<ProductResponseDto> UpdateProduct(Guid productId, ModProductDto dto, Guid UserId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(p => p.UserId == UserId);
            if (store == null) throw new Exception("User dont know");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.StoreId == store.Id);
            if (product == null) throw new Exception("Product not found");

            if(!string.IsNullOrEmpty(dto.Name)) product.Name = dto.Name;
            if(!string.IsNullOrEmpty(dto.Description)) product.Description = dto.Description;
            if(dto.Price.HasValue) product.Price = dto.Price.Value;
            if(dto.Stock.HasValue) product.Stock = dto.Stock.Value;

            await _context.SaveChangesAsync();
            return MapToResponseDto(product);
        }

        public async Task<bool> DeleteProduct(Guid productId, Guid UserId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(p => p.UserId == UserId);
            if (store == null) return false;

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.StoreId == store.Id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ProductResponseDto MapToResponseDto(ProductModel product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                PhotoUrl = product.PhotoUrl,
                StoreId = product.StoreId,
            };
        }
    }
}
