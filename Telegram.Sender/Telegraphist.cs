using System.Text;
using RabbitMQ.Client;

namespace Telegram.Sender
{
    public class Telegraphist:ISendTelegrams
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Telegraphist()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost", VirtualHost = "test" };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

        }

        public void Send(Chit chit)
        {
            var bytes = Encoding.UTF8.GetBytes(chit.Value);
            _channel.BasicPublish("telegram", "", null, bytes);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}