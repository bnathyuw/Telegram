using System;

namespace Telegram.Recipient
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; private set; }
    }

    public delegate void MessageReceivedEventHandler(object sender, MessageEventArgs e);

    public interface IReceiveMessages
    {
        event MessageReceivedEventHandler MessageReceived;
    }

    public interface IWriteOutput
    {
        void WriteMessage(Message message);
    }

    public class RecipientController
    {
        private readonly IReceiveMessages _messageReceiver;
        private readonly IWriteOutput _outputWriter;

        public RecipientController(IReceiveMessages messageReceiver, IWriteOutput outputWriter)
        {
            _messageReceiver = messageReceiver;
            _outputWriter = outputWriter;
        }

        private void WriteMessageToOutput(Message message)
        {
            _outputWriter.WriteMessage(message);
        }

        public void Run()
        {
            _messageReceiver.MessageReceived += (sender, e) => WriteMessageToOutput(e.Message);
        }
    }
}