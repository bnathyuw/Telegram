using NUnit.Framework;

namespace Telegram.Recipient.IsolatedTests
{
    public class Test_recipient_controller
    {
        [TestFixture]
        public class When_a_message_is_received : IReceiveMessages, IWriteOutput
        {
            private Message? _actualMessage;
            private Message _expectedMessage;

            [SetUp]
            public void SetUp()
            {
                _actualMessage = null;
                
                var recipientController = new RecipientController(this, this);

                recipientController.Run();

                _expectedMessage = new Message("Hello, world!");
                OnMessageReceived(new MessageEventArgs(_expectedMessage));
            }

            [Test]
            public void Outputs_the_message()
            {
                Assert.That(_actualMessage, Is.EqualTo(_expectedMessage));
            }

            public event MessageReceivedEventHandler MessageReceived;

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
}