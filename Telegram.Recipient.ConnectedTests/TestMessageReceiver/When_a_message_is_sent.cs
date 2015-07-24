using System.Text;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Telegram.Recipient.ConnectedTests.TestMessageReceiver
{
    [TestFixture]
    public class When_a_message_is_sent
    {
        private const string ExpectedValue = "Testing, testing, 1, 2, 3!";
        private Message _actualMessage;

        [Test]
        public void Then_it_raises_a_message_received_event()
        {
            var messageReceiver = new MessageReceiver();

            messageReceiver.MessageReceived += (sender, e) =>
                {
                    _actualMessage = e.Message;
                };

            messageReceiver.Start();

            SendMessage(ExpectedValue);

            Assert.That(_actualMessage, Is.EqualTo(new Message(ExpectedValue)));
        }

        private void SendMessage(string message)
        {
            var connectionFactory = new ConnectionFactory {HostName = "localhost", VirtualHost = "test"};
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("telegram", "", null, body);
            }
        }
    }
}