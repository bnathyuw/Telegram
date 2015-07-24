using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Telegram.Recipient
{
    public class MessageReceiver : IReceiveMessages
    {
        private Thread _thread;
        public event MessageReceivedEventHandler MessageReceived;

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            var handler = MessageReceived;
            if (handler != null) handler(this, e);
        }

        public void Start()
        {
            _thread = new Thread(ListenForMessages);
            _thread.Start();

            while (!_thread.IsAlive)
            {
            }
        }

        private void ListenForMessages()
        {
            var connectionFactory = new ConnectionFactory() {HostName = "localhost", VirtualHost = "test"};
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume("telegram", false, consumer);
                while (true)
                {
                    var args = consumer.Queue.Dequeue();
                    var data = Encoding.UTF8.GetString(args.Body);
                    OnMessageReceived(new MessageEventArgs(new Message(data)));
                    channel.BasicAck(args.DeliveryTag, false);
                }
            }
        }
    }
}