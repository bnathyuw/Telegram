using System;

namespace Telegram.Recipient
{
    public class OutputWriter : IWriteOutput
    {
        public void WriteMessage(Message message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}