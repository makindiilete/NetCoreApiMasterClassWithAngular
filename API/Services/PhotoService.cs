using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        public readonly Cloudinary _cloudinary;

        // IOptions : - we use ds inside a class to access settings in appSettings.json
        // so we accessing our CloudinarySettings object/session from appsettings.json
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            // Here we set our configuration parameters
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            // we create a new instance of Cloudinary with our defined acc (account)
            _cloudinary = new Cloudinary(acc);
        }
        // Method to add a photo
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            //file.Length = The size of the file being uploaded... We check if the file size is greater than zero
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    //we crop the image to a square and we set the focus to the face
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        // method to delete a photo
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams =  new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}
