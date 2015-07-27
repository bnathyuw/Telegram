using System.Text;
using RabbitMQ.Client;

namespace Telegram.Recipient
{
    public class MessageReceiver : IReceiveMessages
    {
        public event MessageReceivedEventHandler MessageReceived;

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            var handler = MessageReceived;
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
                OnMessageReceived(new MessageEventArgs(new Message(data)));
                channel.BasicAck(args.DeliveryTag, false);
            }
        }
    }
}