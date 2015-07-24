using NUnit.Framework;

namespace Telegram.Recipient.ConnectedTests.TestMessageReceiver
{
    [TestFixture]
    public class When_foo
    {
        [Test]
        public void Then_bar()
        {
            var messageReceiver = new MessageReceiver();
        }
    }
}

namespace Telegram.Recipient
{
    public class MessageReceiver : IReceiveMessages
    {
        public event MessageReceivedEventHandler MessageReceived;
        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}