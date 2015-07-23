using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Telegram.Journeys
{
    public class Recipient : IDisposable
    {
        private readonly List<string> _messages;
        private readonly ConsoleApp _process;

        public Recipient()
        {
            _messages = new List<string>();
            _process = ConsoleApp.FromFile("Telegram.Recipient.exe");
            _process.OutputDataReceived += (sender, args) => _messages.Add(args.Data);
        }

        public void WaitforMessage()
        {
            for (var sleepCount = 0; _messages.Count == 0 && sleepCount < 99; sleepCount++)
                Thread.Sleep(10);
        }

        public void AssertMessageMatches(string expected)
        {
            Assert.That(_messages, Has.Member(expected));
        }

        public void Dispose()
        {
            _process.Dispose();
        }
    }
}