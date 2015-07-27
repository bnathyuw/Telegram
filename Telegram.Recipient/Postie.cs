using System;

namespace Telegram.Recipient
{
    public class Postie : IDeliverTelegrams
    {
        public void Deliver(Telegram telegram)
        {
            Console.WriteLine(telegram.ToString());
        }
    }
}