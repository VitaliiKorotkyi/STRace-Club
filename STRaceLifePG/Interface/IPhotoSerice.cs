using CloudinaryDotNet.Actions;

namespace STRaceLifePG.Interface
{
    public interface IPhotoSerice
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
