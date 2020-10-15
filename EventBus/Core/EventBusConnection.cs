﻿using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;

namespace EventBus.Core
{
    public class EventBusConnection : IEventBusConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public EventBusConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("No rabbit connection");
            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
                return true;

            else
                return false;
        }

        public void Dispose()
        {
            if (!_disposed)
                return;

            try
            {
                _connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
