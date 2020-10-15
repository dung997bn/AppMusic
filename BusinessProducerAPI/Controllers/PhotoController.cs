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
    [Route("api/app-music/v1/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly ILogger<PhotoController> _logger;

        public PhotoController(IPhotoRepository photoRepository, ILogger<PhotoController> logger)
        {
            _photoRepository = photoRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            _logger.LogInformation("Uploading image");
            var result = await _photoRepository.AddPhoto(file);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAudio(string Id)
        {
            _logger.LogInformation("Deleting file");
            var result = await _photoRepository.DeletePhoto(Id);
            return Ok(result);
        }
    }
}
