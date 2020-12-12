using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    //This interface contain method to upload a photo to cloudinary and to delete a photo using the photo publicId
    public interface IPhotoService
    {
        //ds method takes a file as argument and return a task of ImageUploadResult which cloudinary returns as response
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        //ds method takes a publicId of the photo to be deleted (cloudinary attaches a publicId to every uploaded foto) and delete the photo
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
