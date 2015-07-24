using NUnit.Framework;

namespace Telegram.Recipient.UnitTests
{
    public class Test_recipient_controller
    {
        [TestFixture]
        public class When_a_message_is_received : IReceiveMessages, IWriteOutput
        {
            private Message _output;

            [Test]
            public void Outputs_the_message()
            {
                var recipientController = new RecipientController(this, this);

                recipientController.Run();

                var message = new Message("Hello, world!");
                OnMessageReceived(message);

                Assert.That(_output, Is.EqualTo(message));
            }

            public event MessageReceivedEvent MessageReceived;

            protected virtual void OnMessageReceived(Message message)
            {
                MessageReceived(this, message);
            }

            public void WriteMessage(Message message)
            {
                _output = message;
            }
        }
    }

    public interface IWriteOutput
    {
        void WriteMessage(Message message);
    }

    public interface IReceiveMessages
    {
        event MessageReceivedEvent MessageReceived;
    }

    public delegate void MessageReceivedEvent(object sender, Message message);

    public struct Message
    {
        private readonly string _value;

        public Message(string value)
        {
            _value = value;
        }
    }

    public class RecipientController
    {
        private readonly IReceiveMessages _messageReceiver;
        private readonly IWriteOutput _writeOutput;

        public RecipientController(IReceiveMessages messageReceiver, IWriteOutput writeOutput)
        {
            _messageReceiver = messageReceiver;
            _writeOutput = writeOutput;
        }

        private void WriteMessageToOutput(Message message)
        {
            _writeOutput.WriteMessage(message);
        }

        public void Run()
        {
            _messageReceiver.MessageReceived += (sender, message) => WriteMessageToOutput(message);
        }
    }
}