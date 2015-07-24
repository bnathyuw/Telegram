using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Telegram.Recipient.IsolatedTests.TestOutputWriter
{
    [TestFixture]
    public class When_it_receives_a_message : TextWriter
    {
        private TextWriter _standardOut;
        private string _actualValue;
        private const string ExpectedValue = "expectedValue";

        [SetUp]
        public void SetUp()
        {
            _standardOut = Console.Out;
            Console.SetOut(this);

            var outputWriter = new OutputWriter();
            outputWriter.WriteMessage(new Message(ExpectedValue));
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(_standardOut);
        }

        [Test]
        public void Then_it_writes_it_to_console()
        {
            Assert.That(_actualValue, Is.EqualTo(ExpectedValue));
        }

        public override void WriteLine(string value)
        {
            _actualValue = value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}