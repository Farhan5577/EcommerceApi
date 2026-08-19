using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcommerceApi.Options;
using EcommerceApi.Service.Interface;
using Microsoft.Extensions.Options;

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
            var upload = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                upload = await _cloudinary.UploadAsync(uploadParams);
            }

            return upload;
        }

        public async Task<DeletionResult> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
