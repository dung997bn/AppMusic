using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Extensions;
using Data.Models.Business;
using Data.Repositories.CloudServer.Interfaces;
using EventBus;
using EventBus.Events;
using EventBus.Producers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusinessProducerAPI.Controllers
{
    [Route("api/app-music/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "Authorize Schema")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoCloudRepository _videoRepository;
        private readonly ILogger<VideoController> _logger;
        private readonly IMapper _mapper;
        private readonly BusinessProducer _producer;
        private readonly EventBusConstants _constants;

        public VideoController(IVideoCloudRepository videoRepository, ILogger<VideoController> logger,
            IMapper mapper, BusinessProducer producer, IOptions<EventBusConstants> options)
        {
            _videoRepository = videoRepository;
            _logger = logger;
            _mapper = mapper;
            _producer = producer;
            _constants = options.Value;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            _logger.LogInformation("Uploading video");
            var result = await _videoRepository.AddVideo(file);
            return Ok(result);
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteAudio(string Id)
        {
            _logger.LogInformation("Deleting file");
            var result = await _videoRepository.DeleteVideo(Id);
            return Ok(result);
        }

        [HttpPost("complete-upload")]
        public async Task<IActionResult> CompleteUpload([FromBody] Video video)
        {
            try
            {
                _logger.LogInformation("Starting publish video event");
                var userId = User.GetUserId();
                if (userId == null)
                    return BadRequest("Failed to authorize");

                //send check out event to event bus
                var eventMessage = _mapper.Map<VideoEvent>(video);
                eventMessage.Id = Guid.NewGuid();
                eventMessage.RequestId = Guid.NewGuid();
                eventMessage.UserId = userId;
                eventMessage.CreateAt = DateTime.Now;
                eventMessage.UpdatedAt = DateTime.Now;
                eventMessage.DeletedAt = null;

                await _producer.PublishVideoEvent(exchange: _constants.VideoExchange,
                    routing: $"{_constants.VideoRouting}.create", video: eventMessage);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error check out: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
