using Data.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.BusinessServer.Interfaces
{
    public interface IVideoServerRepository
    {
        Task<Video> GetVideoById(Guid Id);
        Task<IEnumerable<Video>> GetVideos();
        Task<Video> CreateVideo(Video video);
        Task<Video> DeleteVideo(Guid Id);
    }
}
