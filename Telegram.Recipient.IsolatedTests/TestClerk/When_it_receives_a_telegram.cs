using NUnit.Framework;

namespace Telegram.Recipient.IsolatedTests.TestClerk
{
    [TestFixture]
    public class When_it_receives_a_telegram : IReceiveTelegrams, IDeliverTelegrams
    {
        private Telegram? _actualTelegram;
        private Telegram _expectedTelegram;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _actualTelegram = null;

            var recipientController = new Clerk(this, this);

            recipientController.Run();

            _expectedTelegram = new Telegram("Hello, world!");
            OnTelegramReceived(new TelegramEventArgs(_expectedTelegram));
        }

        [Test]
        public void Then_it_sends_it_to_the_output_writer()
        {
            Assert.That(_actualTelegram, Is.EqualTo(_expectedTelegram));
        }

        public event TelegramReceivedEventHandler TelegramReceived;

        public void Start()
        {
            
        }

        protected virtual void OnTelegramReceived(TelegramEventArgs e)
        {
            var handler = TelegramReceived;
            if (handler == null) Assert.Fail("No listener attached");
            handler(this, e);
        }


        public void Deliver(Telegram telegram)
        {
            _actualTelegram = telegram;
        }
    }
}