namespace Telegram.Sender
{
    public struct Chit
    {
        private readonly string _value;

        public Chit(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}