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
    //[Authorize(AuthenticationSchemes = "Authorize Schema")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IAudioRepository _audioRepository;
        private readonly ILogger<AudioController> _logger;
        private readonly IMapper _mapper;
        private readonly BusinessProducer _producer;
        private readonly EventBusConstants _constants;
        public AudioController(IAudioRepository audioRepository, ILogger<AudioController> logger,
            IMapper mapper, BusinessProducer producer, IOptions<EventBusConstants> options)
        {
            _audioRepository = audioRepository;
            _logger = logger;
            _mapper = mapper;
            _producer = producer;
            _constants = options.Value;
        }

        [HttpPost("upload")]
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

        [HttpPost("complete-upload")]
        public async Task<IActionResult> CompleteUpload([FromBody] Audio audio)
        {
            try
            {
                _logger.LogInformation("Starting publish audio event");
                var userId = User.GetUserId();
                if (userId == null)
                    return BadRequest("Failed to authorize");

                //send check out event to event bus
                var eventMessage = _mapper.Map<AudioEvent>(audio);
                eventMessage.RequestId = Guid.NewGuid();

                await _producer.PublishAudioEvent(_constants.AudioExchange, _constants.AudioRouting, eventMessage);
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
