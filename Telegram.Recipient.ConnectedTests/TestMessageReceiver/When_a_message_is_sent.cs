using NUnit.Framework;

namespace Telegram.Recipient.ConnectedTests.TestMessageReceiver
{
    [TestFixture]
    public class When_a_message_is_sent
    {
        private const string ExpectedValue = "Testing, testing, 1, 2, 3!";
        private Message _actualMessage;
        private MessageReceiver _messageReceiver;
        private RabbitManager _rabbitManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _rabbitManager = new RabbitManager();
            _rabbitManager.PurgeQueue().Wait();
            _rabbitManager.SendMessage(ExpectedValue).Wait();

            _messageReceiver = new MessageReceiver();
            _messageReceiver.MessageReceived += (sender, e) =>
            {
                _actualMessage = e.Message;
            };
            _messageReceiver.Start();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _rabbitManager.Dispose();
        }

        [Test]
        public void Then_it_raises_a_message_received_event()
        {
            Assert.That(_actualMessage, Is.EqualTo(new Message(ExpectedValue)));
        }
    }
}