using System;

namespace Telegram.Recipient
{
    public class ConsoleTelegramWriter : IDeliverTelegrams
    {
        public void Deliver(Telegram telegram)
        {
            Console.WriteLine(telegram.ToString());
        }
    }
}