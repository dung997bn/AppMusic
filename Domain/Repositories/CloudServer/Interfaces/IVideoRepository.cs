using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Data.Repositories.CloudServer.Interfaces
{
    public interface IVideoRepository
    {
        Task<FileUploadResult> AddVideo(IFormFile file);
        Task<string> DeleteVideo(string publicId);
    }
}
