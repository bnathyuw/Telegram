namespace Telegram.Configurator.Tests
{
    public struct VirtualHost
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("VirtualHost<Name: {0}>", Name);
        }
    }
}