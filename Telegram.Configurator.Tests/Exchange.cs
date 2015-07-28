namespace Telegram.Configurator.Tests
{
    public struct Exchange
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("Exchange<Name: {0}>", Name);
        }
    }
}