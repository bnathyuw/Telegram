using System;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestCustomer
{
    [TestFixture]
    public class When_it_has_a_new_message_to_send : TextReader
    {
        private Chit? _actualChit;
        private TextReader _standardIn;
        private int _messageCounter;
        private const string ExpectedValue = "Hello, world!";
        private const int NumberOfMessagesToSend = 1;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _actualChit = null;
            _standardIn = Console.In;
            _messageCounter = 0;
            Console.SetIn(this);
            var customer = new Customer();
            customer.ChitDelivered += (sender, args) => _actualChit = args.Chit;
            customer.Start();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.SetIn(_standardIn);
        }

        [Test]
        public void Then_it_delivers_a_chit()
        {
            Assert.That(_actualChit, Is.EqualTo(new Chit(ExpectedValue)));
        }

        public override string ReadLine()
        {
            if (_messageCounter++ >= NumberOfMessagesToSend)
                Thread.Sleep(Timeout.Infinite);
            return ExpectedValue;
        }
    }
}