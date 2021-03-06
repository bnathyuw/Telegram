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
        private Customer _customer;
        private const string ExpectedValue = "Hello, world!";
        private const int NumberOfMessagesToSend = 1;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _actualChit = null;
            _standardIn = Console.In;
            _messageCounter = 0;
            Console.SetIn(this);
            _customer = new Customer();
            _customer.ChitDelivered += (sender, args) => _actualChit = args.Chit;
            _customer.Start();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _customer.Dispose();
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