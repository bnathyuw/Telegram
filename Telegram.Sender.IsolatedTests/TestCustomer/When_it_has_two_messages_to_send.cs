using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestCustomer
{
    [TestFixture]
    public class When_it_has_two_messages_to_send : TextReader
    {
        private List<Chit> _actualChits;
        private TextReader _standardIn;
        private int _messageCounter;
        private Customer _customer;
        private const string ExpectedValue = "Hello, world!";
        private const int NumberOfMessagesToSend = 2;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _actualChits = new List<Chit>();
            _standardIn = Console.In;
            _messageCounter = 0;
            Console.SetIn(this);
            _customer = new Customer();
            _customer.ChitDelivered += (sender, args) => _actualChits.Add(args.Chit);
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
            Assert.That(_actualChits.Count, Is.EqualTo(2));
        }

        public override string ReadLine()
        {
            if (_messageCounter++ >= NumberOfMessagesToSend)
                Thread.Sleep(Timeout.Infinite);
            return ExpectedValue;
        }
    }
}