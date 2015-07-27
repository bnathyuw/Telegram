using System.Threading;

namespace Telegram.Recipient
{
    public class Telegraphist : IReceiveTelegrams
    {
        private Thread _thread;
        private readonly Telegraph _telegraph;
        public event TelegramReceivedEventHandler TelegramReceived;

        protected virtual void OnTelegramReceived(TelegramReceivedEventArgs e)
        {
            var handler = TelegramReceived;
            if (handler != null) handler(this, e);
        }

        public Telegraphist()
        {
            _telegraph = new Telegraph();
            _telegraph.TelegramReceived += (sender, args) => OnTelegramReceived(args);
        }

        public void Start()
        {
            _thread = new Thread(_telegraph.ConsumeMessages);
            _thread.Start();
            while (!_thread.IsAlive)
            {
            }   
        }

        public void Dispose()
        {
            _thread.Interrupt();
            _telegraph.Dispose();
            _thread.Join();
        }
    }
}