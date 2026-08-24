using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcommerceApi.Options.Cloudinary;
using EcommerceApi.Service.Interface;
using Microsoft.Extensions.Options;
using EcommerceApi.Options.Exceptions;
namespace EcommerceApi.Service
{
    public sealed class PhotoService(IOptions<CloudinaryOptions> config) : IPhotoService
    {
        private readonly Cloudinary _cloudinary = new(new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
            ));

        public async Task<ImageUploadResult> AddPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0) throw new BadRequestException("The content cannot be empty.");
            
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null) throw new BadRequestException($"Failed to Upload Photo : {uploadResult.Error.Message}"); 

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhoto(string publicId)
        {
            if (string.IsNullOrEmpty(publicId)) throw new BadRequestException("Invalid image public ID!");
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Error != null) throw new BadRequestException($"Failed to delete photo : {result.Error.Message}");
            return result;
        }
    }
}
