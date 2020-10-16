using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Data.Repositories.CloudServer.Interfaces;
using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Ultilities.Constants;

namespace Data.Repositories.CloudServer.Implementations
{
    public class AudioCloudRepository : IAudioCloudRepository
    {
        private readonly Cloudinary _cloudinary;
        public AudioCloudRepository(IOptions<CloudinarySettings> setting)
        {
            var account = new Account(setting.Value.CloudName, setting.Value.ApiKey, setting.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<FileUploadResult> AddAudio(IFormFile file)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(file.FileName, stream)
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            if (uploadResult.Error != null)
                return new FileUploadResult
                {
                    PublicId = "",
                    Url = "",
                    Message = uploadResult.Error.Message
                };

            return new FileUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri,
                Message = ""
            };
        }

        public async Task<string> DeleteAudio(string publicId)
        {
            var deletePrams = new DeletionParams(publicId);
            deletePrams.ResourceType = ResourceType.Video;
            var result = await _cloudinary.DestroyAsync(deletePrams);
            return result.Result == "ok" ? result.Result : null;
        }
    }
}
