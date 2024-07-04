using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using STRaceLifePG.Interface;

namespace STRaceLifePG.Helpers.Services
{
    public class PhotoService : IPhotoSerice
    {
        private readonly Cloudinary _cloudinary; 
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName,config.Value.ApiKey,config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        
        }
        //IOptions<CloudinarySettings>: Этот интерфейс предоставляет доступ к конфигурационным настройкам вашего приложения.
        //config: Параметр конструктора, через который передаются настройки. Он имеет тип IOptions<CloudinarySettings>.
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
           var uloadResult=new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };
                uloadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uloadResult; 
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result=await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}
