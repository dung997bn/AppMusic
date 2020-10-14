using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories.CloudServer.Interfaces
{
    public interface IVideoRepository
    {
        FileUploadResult AddVideo(IFormFile file);
        string DeleteVideo(string publicId);
    }
}
