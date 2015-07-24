using System;
using System.IO;
using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestUserInputReceiver
{
    [TestFixture]
    public class When_it_receives_user_input : TextReader
    {
        private Message? _actualMessage;
        private const string ExpectedValue = "Hello, world!";

        [Test]
        public void Then_it_raises_a_user_input_received_event()
        {
            _actualMessage = null;
            var standardIn = Console.In;
            Console.SetIn(this);
            var userInputReceiver = new UserInputReceiver();
            userInputReceiver.UserInputReceived += (sender, args) =>
                {
                    _actualMessage = args.Message;
                };
            userInputReceiver.Start();

            Assert.That(_actualMessage, Is.EqualTo(new Message(ExpectedValue)));

            Console.SetIn(standardIn);
        }

        public override string ReadLine()
         {
             return ExpectedValue;
         }
    }
}