using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Telegram.Sender.ConnectedTests.TestTelegraphist
{
    [TestFixture]
    public class When_it_sends_a_message
    {
        private RabbitManager _rabbitManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _rabbitManager = new RabbitManager();
            _rabbitManager.PurgeQueue().Wait();

            var telegraphist = new Telegraphist();

            telegraphist.Send(new Chit("I am a teapot"));

            Thread.Sleep(1000);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _rabbitManager.Dispose();
        }

        [Test]
        public async void Then_the_message_is_queued()
        {
            var messages = await _rabbitManager.GetMessage();

            Assert.That(messages.Single(), Is.EqualTo(new Message {Payload = "I am a teapot"}));
        }
    }
}
