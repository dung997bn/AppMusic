using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Data.Repositories.CloudServer.Interfaces
{
    public interface IVideoCloudRepository
    {
        Task<FileUploadResult> AddVideo(IFormFile file);
        Task<string> DeleteVideo(string publicId);
    }
}
