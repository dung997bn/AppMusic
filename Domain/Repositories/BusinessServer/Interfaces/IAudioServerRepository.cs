using Data.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.BusinessServer.Interfaces
{
    public interface IAudioServerRepository
    {
        Task<Audio> GetAudioById(Guid Id);
        Task<IEnumerable<Audio>> GetAudios();
        Task<Audio> CreateAudio(Audio audio);
        Task<Audio> DeleteAudio(Guid Id);
    }
}
