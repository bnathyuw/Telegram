using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestTelegraphOffice
{
    [TestFixture]
    public class When_it_receives_input : IReceiveTelegramRequests, ISendMessages
    {
        private Telegram? _actualTelegram;

        [Test]
        public void Then_it_sends_a_message()
        {
            _actualTelegram = null;

            var senderController = new TelegraphOffice(this, this);

            senderController.Run();

            OnTelegramRequestReceived(new TelegramRequestReceivedEventArgs(new Telegram("Hello, world!")));
        }

        public event TelegramRequestReceivedEventHandler TelegramRequestReceived;

        protected virtual void OnTelegramRequestReceived(TelegramRequestReceivedEventArgs e)
        {
            var handler = TelegramRequestReceived;
            if (handler == null) Assert.Fail("Nothing listening");
            handler(this, e);
        }

        public void SendMessage(Telegram telegram)
        {
            _actualTelegram = telegram;
        }
    }
}
