using AutoMapper;
using Data.Models.Business;
using Data.Repositories.BusinessServer.Interfaces;
using EventBus;
using EventBus.Core;
using EventBus.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessConsumerAPI.Consumer
{
    public class BusinessConsumer
    {
        private readonly IEventBusConnection _connection;
        private readonly IMapper _mapper;
        private readonly IAudioServerRepository _audioRepository;
        private readonly IVideoServerRepository _videoRepository;
        private readonly IPhotoServerRepository _photoRepository;
        private readonly EventBusConstants _constants;

        public BusinessConsumer(IEventBusConnection connection, IMapper mapper, IAudioServerRepository audioRepository,
            IVideoServerRepository videoRepository, IPhotoServerRepository photoRepository, IOptions<EventBusConstants> constants)
        {
            _connection = connection;
            _mapper = mapper;
            _audioRepository = audioRepository;
            _videoRepository = videoRepository;
            _photoRepository = photoRepository;
            _constants = constants.Value;
        }

        public void ConsumeAudio(string exchange, string routing, string queue)
        {
            var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queue, exchange: exchange, routingKey: $"{routing}.*");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var audioEvent = JsonConvert.DeserializeObject<AudioEvent>(message);

                var audio = _mapper.Map<Audio>(audioEvent);
                var result = await _audioRepository.CreateAudio(audio);
            };
            channel.BasicConsume(queue, true, consumer);
        }

        public void ConsumeVideo(string exchange, string routing, string queue)
        {
            var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queue, exchange: exchange, routingKey: $"{routing}.*");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var videoEvent = JsonConvert.DeserializeObject<VideoEvent>(message);

                var video = _mapper.Map<Video>(videoEvent);
                var result = await _videoRepository.CreateVideo(video);
            };
            channel.BasicConsume(queue, true, consumer);
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
