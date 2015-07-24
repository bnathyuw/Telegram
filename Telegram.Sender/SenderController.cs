namespace Telegram.Sender
{
    public class UserInputReceivedEventArgs
    {
        public UserInputReceivedEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; private set; }
    }

    public delegate void UserInputReceivedEventHandler(object sender, UserInputReceivedEventArgs e);

    public interface IReceiveUserInput
    {
        event UserInputReceivedEventHandler UserInputReceived;
    }

    public interface ISendMessages
    {
        void SendMessage(Message message);
    }

    public class SenderController
    {
        private readonly IReceiveUserInput _userInputReceiver;
        private readonly ISendMessages _messageSender;

        public SenderController(IReceiveUserInput userInputReceiver, ISendMessages messageSender)
        {
            _userInputReceiver = userInputReceiver;
            _messageSender = messageSender;
        }

        public void Run()
        {
            _userInputReceiver.UserInputReceived += (sender, args) => _messageSender.SendMessage(args.Message);
        }
    }
}