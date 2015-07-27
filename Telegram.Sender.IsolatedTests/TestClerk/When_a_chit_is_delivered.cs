using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestClerk
{
    [TestFixture]
    public class When_a_chit_is_delivered : IDeliverChits, ISendTelegrams
    {
        private Chit? _actualChit;

        [Test]
        public void Then_it_sends_a_telegram()
        {
            _actualChit = null;

            var senderController = new Clerk(this, this);

            senderController.Run();

            OnTelegramRequestReceived(new ChitDeliveredEventArgs(new Chit("Hello, world!")));
        }

        public event ChitDeliveredEventHandler ChitDelivered;

        protected virtual void OnTelegramRequestReceived(ChitDeliveredEventArgs e)
        {
            var handler = ChitDelivered;
            if (handler == null) Assert.Fail("Nothing listening");
            handler(this, e);
        }

        public void Send(Chit chit)
        {
            _actualChit = chit;
        }

        public void Dispose()
        {
            
        }
    }
}
