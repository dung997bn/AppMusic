using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories.CloudServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusinessProducerAPI.Controllers
{
    [Route("api/app-music/v1/[controller]")]
    //[Authorize(AuthenticationSchemes = "Authorize Schema")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IAudioRepository _audioRepository;
        private readonly ILogger<AudioController> _logger;

        public AudioController(IAudioRepository audioRepository, ILogger<AudioController> logger)
        {
            _audioRepository = audioRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAudio(IFormFile file)
        {
            _logger.LogInformation("Uploading file");
            var result = await _audioRepository.AddAudio(file);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAudio(string Id)
        {
            _logger.LogInformation("Deleting file");
            var result = await _audioRepository.DeleteAudio(Id);
            return Ok(result);
        }
    }
}
