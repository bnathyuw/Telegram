using System.Threading;
using NUnit.Framework;

namespace Telegram.Recipient.ConnectedTests.TestTelegraphist
{
    [TestFixture]
    public class When_a_telegram_is_sent
    {
        private const string ExpectedValue = "Testing, testing, 1, 2, 3!";
        private Telegram _actualTelegram;
        private Telegraphist _telegraphist;
        private RabbitManager _rabbitManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _rabbitManager = new RabbitManager();
            _rabbitManager.PurgeQueue().Wait();

            _telegraphist = new Telegraphist();
            _telegraphist.TelegramReceived += (sender, e) => _actualTelegram = e.Telegram;
            _telegraphist.Start();

            _rabbitManager.PublishMessage(ExpectedValue).Wait();

            Thread.Sleep(1000);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _telegraphist.Dispose();
            _rabbitManager.Dispose();
        }

        [Test]
        public void Then_it_raises_a_message_received_event()
        {
            Assert.That(_actualTelegram, Is.EqualTo(new Telegram(ExpectedValue)));
        }
    }
}