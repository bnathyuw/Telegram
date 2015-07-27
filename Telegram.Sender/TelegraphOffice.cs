namespace Telegram.Sender
{
    public class TelegramRequestReceivedEventArgs
    {
        public TelegramRequestReceivedEventArgs(Telegram telegram)
        {
            Telegram = telegram;
        }

        public Telegram Telegram { get; private set; }
    }

    public delegate void TelegramRequestReceivedEventHandler(object sender, TelegramRequestReceivedEventArgs e);

    public interface IReceiveTelegramRequests
    {
        event TelegramRequestReceivedEventHandler TelegramRequestReceived;
    }

    public interface ISendMessages
    {
        void SendMessage(Telegram telegram);
    }

    public class TelegraphOffice
    {
        private readonly IReceiveTelegramRequests _telegramRequestReceiver;
        private readonly ISendMessages _messageSender;

        public TelegraphOffice(IReceiveTelegramRequests telegramRequestsReceiver, ISendMessages messageSender)
        {
            _telegramRequestReceiver = telegramRequestsReceiver;
            _messageSender = messageSender;
        }

        public void Run()
        {
            _telegramRequestReceiver.TelegramRequestReceived += (sender, args) => _messageSender.SendMessage(args.Telegram);
        }
    }
}