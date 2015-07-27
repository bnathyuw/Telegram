namespace Telegram.Recipient
{
    public struct Telegram
    {
        private readonly string _value;

        public Telegram(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}