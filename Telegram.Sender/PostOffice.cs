using System;

namespace Telegram.Sender
{
    public static class PostOffice
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                var message = Console.ReadLine();
                if (message == "exit") break;
                Console.WriteLine("received message {0}", message);
            }
        }
    }
}
