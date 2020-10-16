using Data.DbContexts.Business.Core;
using Data.Models.Business;
using Data.Repositories.BusinessServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.BusinessServer.Implementations
{
    public class AudioServerRepository : IAudioServerRepository
    {
        IBusinessDbContext _dbContext;

        public AudioServerRepository(IBusinessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Audio> CreateAudio(Audio audio)
        {
            throw new NotImplementedException();
        }

        public Task<Audio> DeleteAudio(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Audio> GetAudioById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Audio>> GetAudios()
        {
            throw new NotImplementedException();
        }
    }
}
