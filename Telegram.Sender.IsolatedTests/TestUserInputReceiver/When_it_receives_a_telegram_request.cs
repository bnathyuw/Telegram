using System;
using System.IO;
using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestUserInputReceiver
{
    [TestFixture]
    public class When_it_receives_a_telegram_request : TextReader
    {
        private Telegram? _actualTelegram;
        private const string ExpectedValue = "Hello, world!";

        [Test]
        public void Then_it_raises_a_telegram_request_received_event()
        {
            _actualTelegram = null;
            var standardIn = Console.In;
            Console.SetIn(this);
            var telegramRequestReceiver = new TelegramRequestReceiver();
            telegramRequestReceiver.TelegramRequestReceived += (sender, args) =>
                {
                    _actualTelegram = args.Telegram;
                };
            telegramRequestReceiver.Start();

            Assert.That(_actualTelegram, Is.EqualTo(new Telegram(ExpectedValue)));

            Console.SetIn(standardIn);
        }

        public override string ReadLine()
         {
             return ExpectedValue;
         }
    }
}