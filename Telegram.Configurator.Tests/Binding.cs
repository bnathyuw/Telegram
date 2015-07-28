namespace Telegram.Configurator.Tests
{
    public struct Binding
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public override string ToString()
        {
            return string.Format("Binding<Source: {0}; Destination: {1}>", Source, Destination);
        }
    }
}