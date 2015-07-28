namespace Telegram.Configurator.Tests
{
    public struct Queue
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("Queue<Name: {0}>", Name);
        }
    }
}