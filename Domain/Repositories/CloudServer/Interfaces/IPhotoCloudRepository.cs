﻿using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Data.Repositories.CloudServer.Interfaces
{
    public interface IPhotoCloudRepository
    {
        Task<FileUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}
