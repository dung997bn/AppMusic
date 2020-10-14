using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Data.Repositories.CloudServer.Interfaces;
using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using Ultilities.Constants;

namespace Data.Repositories.CloudServer.Implementations
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly Cloudinary _cloudinary;
        public PhotoRepository(IOptions<CloudinarySettings> setting)
        {
            var account = new Account(setting.Value.CloudName, setting.Value.ApiKey, setting.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public FileUploadResult AddPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
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

        public string DeletePhoto(string publicId)
        {
            var deletePrams = new DeletionParams(publicId);
            var result = _cloudinary.Destroy(deletePrams);
            return result.Result == "ok" ? result.Result : null;
        }
    }
}
