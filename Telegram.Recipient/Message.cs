namespace Telegram.Recipient
{
    public struct Message
    {
        private readonly string _value;

        public Message(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}