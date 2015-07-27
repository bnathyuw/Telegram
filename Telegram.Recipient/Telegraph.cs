using System.Text;
using RabbitMQ.Client;

namespace Telegram.Recipient
{
    public class Telegraph : IReceiveTelegrams
    {
        public event TelegramReceivedEventHandler TelegramReceived;

        protected virtual void OnMessageReceived(TelegramEventArgs e)
        {
            var handler = TelegramReceived;
            if (handler != null) handler(this, e);
        }

        public void Start()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "test" };
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume("telegram", false, consumer);

                var args = consumer.Queue.Dequeue();
                var data = Encoding.UTF8.GetString(args.Body);
                OnMessageReceived(new TelegramEventArgs(new Telegram(data)));
                channel.BasicAck(args.DeliveryTag, false);
            }
        }
    }
}