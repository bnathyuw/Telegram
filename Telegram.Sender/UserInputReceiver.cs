using System;

namespace Telegram.Sender
{
    public class UserInputReceiver : IReceiveUserInput
    {
        public event UserInputReceivedEventHandler UserInputReceived;

        protected virtual void OnUserInputReceived(UserInputReceivedEventArgs e)
        {
            var handler = UserInputReceived;
            if (handler != null) handler(this, e);
        }

        public void Start()
        {
            var line = Console.ReadLine();
            OnUserInputReceived(new UserInputReceivedEventArgs(new Message(line)));
        }
    }
}