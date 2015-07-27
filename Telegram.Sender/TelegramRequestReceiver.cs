using System;

namespace Telegram.Sender
{
    public class TelegramRequestReceiver : IReceiveTelegramRequests
    {
        public event TelegramRequestReceivedEventHandler TelegramRequestReceived;

        protected virtual void OnTelegramRequestReceived(TelegramRequestReceivedEventArgs e)
        {
            var handler = TelegramRequestReceived;
            if (handler != null) handler(this, e);
        }

        public void Start()
        {
            var line = Console.ReadLine();
            OnTelegramRequestReceived(new TelegramRequestReceivedEventArgs(new Telegram(line)));
        }
    }
}