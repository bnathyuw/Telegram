using System;

namespace Telegram.Journeys
{
    public class Sender : IDisposable
    {
        private readonly ConsoleApp _process;

        public Sender()
        {
            _process = ConsoleApp.FromFile("Telegram.Sender.exe");
        }

        public void Send(string message)
        {
            _process.WriteLine(message);
        }

        public void Dispose()
        {
            _process.Dispose();
        }
    }
}