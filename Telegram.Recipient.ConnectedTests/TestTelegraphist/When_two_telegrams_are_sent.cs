using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Telegram.Recipient.ConnectedTests.TestTelegraphist
{
    [TestFixture]
    public class When_two_telegrams_are_sent
    {
        private List<Telegram> _actualTelegrams;
        private Telegraphist _telegraphist;
        private RabbitManager _rabbitManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _actualTelegrams = new List<Telegram>();

            _rabbitManager = new RabbitManager();
            _rabbitManager.PurgeQueue().Wait();

            _telegraphist = new Telegraphist();
            _telegraphist.TelegramReceived += (sender, e) => _actualTelegrams.Add(e.Telegram);
            _telegraphist.Start();

            _rabbitManager.PublishMessage("Testing, testing, 1, 2, 3!").Wait();
            _rabbitManager.PublishMessage("Testing, testing, 2, 3, 4!").Wait();

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
            Assert.That(_actualTelegrams.Count, Is.EqualTo(2));
        }
    }
}