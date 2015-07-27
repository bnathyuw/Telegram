using NUnit.Framework;

namespace Telegram.Recipient.IsolatedTests.TestClerk
{
    [TestFixture]
    public class When_it_runs : IReceiveTelegrams, IDeliverTelegrams
    {
        private bool _started;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _started = false;

            var recipientController = new Clerk(this, this);

            recipientController.Run();
        }

        [Test]
        public void Then_it_starts_the_message_receiver()
        {
            Assert.That(_started, Is.True);
        }

        public event TelegramReceivedEventHandler TelegramReceived;
        
        public void Start()
        {
            _started = true;
        }

        public void Deliver(Telegram telegram)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }
}