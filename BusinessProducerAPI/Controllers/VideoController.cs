using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories.CloudServer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusinessProducerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository _videoRepository;
        private readonly ILogger<VideoController> _logger;

        public VideoController(IVideoRepository videoRepository, ILogger<VideoController> logger)
        {
            _videoRepository = videoRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            _logger.LogInformation("Uploading video");
            var result = await _videoRepository.AddVideo(file);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAudio(string Id)
        {
            _logger.LogInformation("Deleting file");
            var result = await _videoRepository.DeleteVideo(Id);
            return Ok(result);
        }
    }
}
