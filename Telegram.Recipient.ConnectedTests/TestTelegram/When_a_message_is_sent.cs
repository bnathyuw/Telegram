using NUnit.Framework;

namespace Telegram.Recipient.ConnectedTests.TestTelegram
{
    [TestFixture]
    public class When_a_message_is_sent
    {
        private const string ExpectedValue = "Testing, testing, 1, 2, 3!";
        private Telegram _actualTelegram;
        private Telegraph _telegraph;
        private RabbitManager _rabbitManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _rabbitManager = new RabbitManager();
            _rabbitManager.PurgeQueue().Wait();
            _rabbitManager.PublishMessage(ExpectedValue).Wait();

            _telegraph = new Telegraph();
            _telegraph.TelegramReceived += (sender, e) =>
            {
                _actualTelegram = e.Telegram;
            };
            _telegraph.Start();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _rabbitManager.Dispose();
        }

        [Test]
        public void Then_it_raises_a_message_received_event()
        {
            Assert.That(_actualTelegram, Is.EqualTo(new Telegram(ExpectedValue)));
        }
    }
}