using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestSenderController
{
    [TestFixture]
    public class When_it_receives_input : IReceiveUserInput, ISendMessages
    {
        private Message? _actualMessage;

        [Test]
        public void Then_it_sends_a_message()
        {
            _actualMessage = null;

            var senderController = new SenderController(this, this);

            senderController.Run();

            OnUserInputReceived(new UserInputReceivedEventArgs(new Message("Hello, world!")));
        }

        public event UserInputReceivedEventHandler UserInputReceived;

        protected virtual void OnUserInputReceived(UserInputReceivedEventArgs e)
        {
            var handler = UserInputReceived;
            if (handler == null) Assert.Fail("Nothing listening");
            handler(this, e);
        }

        public void SendMessage(Message message)
        {
            _actualMessage = message;
        }
    }
}
