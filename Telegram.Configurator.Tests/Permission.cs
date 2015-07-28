namespace Telegram.Configurator.Tests
{
    public struct Permission
    {
        public string User { get; set; }
        public string VHost { get; set; }
        public string Configure { get; set; }
        public string Write { get; set; }
        public string Read { get; set; }

        public override string ToString()
        {
            return string.Format("Permission<User: {0}; VHost: {1}; Configure: {2}; Write: {3}; Read: {4}>", User, VHost, Configure, Write, Read);
        }
    }
}