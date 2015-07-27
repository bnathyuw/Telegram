namespace Telegram.Sender.ConnectedTests.TestTelegraphist
{
    public struct Message
    {
        public string Payload { get; set; }

        public override string ToString()
        {
            return string.Format("Message<Payload: {0}>", Payload);
        }
    }
}