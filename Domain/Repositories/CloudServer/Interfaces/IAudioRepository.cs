using Data.ViewModels.CloudServer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.CloudServer.Interfaces
{
    public interface IAudioRepository
    {
        FileUploadResult AddAudio(IFormFile file);
        string DeleteAudio(string publicId);
    }
}
