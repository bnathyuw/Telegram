using System;
using System.Diagnostics;

namespace Telegram.Journeys
{
    internal class ConsoleApp : IDisposable
    {
        private readonly Process _process;

        private ConsoleApp(Process process)
        {
            process.OutputDataReceived += (sender, args) => OnOutputDataReceived(args);
            _process = process;
        }

        public event DataReceivedEventHandler OutputDataReceived;

        protected virtual void OnOutputDataReceived(DataReceivedEventArgs e)
        {
            var handler = OutputDataReceived;
            if (handler != null) handler(this, e);
        }

        public void WriteLine(string message)
        {
            _process.StandardInput.WriteLine(message);
        }

        public void Dispose()
        {
            WriteLine("exit");
            _process.WaitForExit();
            _process.Dispose();
        }

        public static ConsoleApp FromFile(string fileName)
        {
            var process = new Process
                {
                    StartInfo = new ProcessStartInfo(fileName)
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true
                        }
                };

            process.Start();
            process.BeginOutputReadLine();
            return new ConsoleApp(process);
        }
    }
}