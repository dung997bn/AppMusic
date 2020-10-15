using RabbitMQ.Client;
using System;

namespace EventBus.Core
{
    public interface IEventBusConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}
