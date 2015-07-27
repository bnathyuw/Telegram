using System;

namespace Telegram.Sender
{
    public class Customer : IDeliverChits
    {
        public event ChitDeliveredEventHandler ChitDelivered;

        protected virtual void OnChitDelivered(ChitDeliveredEventArgs e)
        {
            var handler = ChitDelivered;
            if (handler != null) handler(this, e);
        }

        public void Start()
        {
            var line = Console.ReadLine();
            OnChitDelivered(new ChitDeliveredEventArgs(new Chit(line)));
        }
    }
}