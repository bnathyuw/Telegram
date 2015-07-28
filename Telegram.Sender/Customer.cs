using System;
using System.Threading;

namespace Telegram.Sender
{
    public class Customer : IDeliverChits
    {
        private Thread _thread;
        public event ChitDeliveredEventHandler ChitDelivered;

        protected virtual void OnChitDelivered(ChitDeliveredEventArgs e)
        {
            var handler = ChitDelivered;
            if (handler != null) handler(this, e);
        }

        public void Start()
        {
            _thread = new Thread(Foo);
            _thread.Start();
            while (!_thread.IsAlive)
            {
            }
        }

        private void Foo()
        {
            while (true)
            {
                var line = Console.ReadLine();
                var chit = new Chit(line);
                var chitDeliveredEventArgs = new ChitDeliveredEventArgs(chit);
                OnChitDelivered(chitDeliveredEventArgs);
            }
        }

        public void Dispose()
        {
            _thread.Interrupt();
            _thread.Join();
        }
    }
}