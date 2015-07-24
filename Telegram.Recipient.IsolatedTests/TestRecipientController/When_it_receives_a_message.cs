using NUnit.Framework;

namespace Telegram.Recipient.IsolatedTests.TestRecipientController
{
    [TestFixture]
    public class When_it_receives_a_message : IReceiveMessages, IWriteOutput
    {
        private Message? _actualMessage;
        private Message _expectedMessage;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _actualMessage = null;

            var recipientController = new RecipientController(this, this);

            recipientController.Run();

            _expectedMessage = new Message("Hello, world!");
            OnMessageReceived(new MessageEventArgs(_expectedMessage));
        }

        [Test]
        public void Then_it_sends_it_to_the_output_writer()
        {
            Assert.That(_actualMessage, Is.EqualTo(_expectedMessage));
        }

        public event MessageReceivedEventHandler MessageReceived;

        public void Start()
        {
            
        }

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            var handler = MessageReceived;
            if (handler == null) Assert.Fail("No listener attached");
            handler(this, e);
        }


        public void WriteMessage(Message message)
        {
            _actualMessage = message;
        }
    }
}