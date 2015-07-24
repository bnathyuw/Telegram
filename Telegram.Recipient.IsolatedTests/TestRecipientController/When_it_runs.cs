using NUnit.Framework;

namespace Telegram.Recipient.IsolatedTests.TestRecipientController
{
    [TestFixture]
    public class When_it_runs : IReceiveMessages, IWriteOutput
    {
        private bool _started;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _started = false;

            var recipientController = new RecipientController(this, this);

            recipientController.Run();
        }

        [Test]
        public void Then_it_starts_the_message_receiver()
        {
            Assert.That(_started, Is.True);
        }

        public event MessageReceivedEventHandler MessageReceived;
        
        public void Start()
        {
            _started = true;
        }

        public void WriteMessage(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}