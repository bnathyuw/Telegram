using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Telegram.Recipient
{
    public class Telegraph : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly QueueingBasicConsumer _consumer;
        public event TelegramReceivedEventHandler TelegramReceived;

        protected virtual void OnTelegramReceived(TelegramReceivedEventArgs e)
        {
            var handler = TelegramReceived;
            if (handler != null) handler(this, e);
        }

        public Telegraph()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost", VirtualHost = "test" };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _consumer = new QueueingBasicConsumer(_channel);
        }

        public void ConsumeMessages()
        {
            _channel.BasicConsume("telegram", false, _consumer);

            while (true)
            {
                var result = GetMessageFromQueue();
                HandleMessage(result);
                AcknowledgeReceipt(result);
            }
        }

        private BasicDeliverEventArgs GetMessageFromQueue()
        {
            return _consumer.Queue.Dequeue();
        }

        private void HandleMessage(BasicDeliverEventArgs result)
        {
            var data = Encoding.UTF8.GetString(result.Body);
            var telegram = new Telegram(data);
            var telegramEventArgs = new TelegramReceivedEventArgs(telegram);
            OnTelegramReceived(telegramEventArgs);
        }

        private void AcknowledgeReceipt(BasicDeliverEventArgs result)
        {
            _channel.BasicAck(result.DeliveryTag, false);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}