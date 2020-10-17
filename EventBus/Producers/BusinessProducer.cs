using EventBus.Core;
using EventBus.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Producers
{
    public class BusinessProducer
    {
        private readonly IEventBusConnection _connection;

        public BusinessProducer(IEventBusConnection connection)
        {
            _connection = connection;
        }

        public Task PublishVideoEvent(string exchange, string routing, VideoEvent video)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, arguments: null);
                var message = JsonConvert.SerializeObject(video);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: exchange, routingKey: routing, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent to Event Bus");
                    //implement ack handle
                };
                channel.ConfirmSelect();
                return Task.CompletedTask;
            }
        }

        public Task PublishAudioEvent(string exchange, string routing, AudioEvent audio)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, arguments: null);
                var message = JsonConvert.SerializeObject(audio);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: exchange, routingKey: routing, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent to Event Bus");
                    //implement ack handle
                };
                channel.ConfirmSelect();
                return Task.CompletedTask;
            }
        }

    }
}
