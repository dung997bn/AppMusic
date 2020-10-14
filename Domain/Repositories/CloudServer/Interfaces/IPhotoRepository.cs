using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories.CloudServer.Interfaces
{
    public interface IPhotoRepository
    {
        FileUploadResult AddPhoto(IFormFile file);
        string DeletePhoto(string publicId);
    }
}
