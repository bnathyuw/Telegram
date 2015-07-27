using System;
using System.IO;
using NUnit.Framework;

namespace Telegram.Sender.IsolatedTests.TestCustomer
{
    [TestFixture]
    public class When_it_has_a_new_message_to_send : TextReader
    {
        private Chit? _actualChit;
        private const string ExpectedValue = "Hello, world!";

        [Test]
        public void Then_it_delivers_a_chit()
        {
            _actualChit = null;
            var standardIn = Console.In;
            Console.SetIn(this);
            var customer = new Customer();
            customer.ChitDelivered += (sender, args) =>
                {
                    _actualChit = args.Chit;
                };
            customer.Start();

            Assert.That(_actualChit, Is.EqualTo(new Chit(ExpectedValue)));

            Console.SetIn(standardIn);
        }

        public override string ReadLine()
         {
             return ExpectedValue;
         }
    }
}