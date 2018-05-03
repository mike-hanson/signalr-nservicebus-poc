using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Nsb.Poc.Sales
{
    internal class Program
    {
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(0);
        
        private delegate bool HandlerRoutine(CtrlTypes ctrlType);

        private static async Task Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SetConsoleCtrlHandler(ConsoleCtrlCheck, true);
            }
            else
            {
                Console.CancelKeyPress += CancelKeyPress;
                AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            }

            var host = new Host();

            Console.Title = host.Label;

            await host.Start();
            await Console.Out.WriteLineAsync("Press Ctrl+C to exit...");

            // wait until notified that the process should exit
            await Semaphore.WaitAsync();

            await host.Stop();
        }

        static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Semaphore.Release();
        }

        static void ProcessExit(object sender, EventArgs e)
        {
            Semaphore.Release();
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            Semaphore.Release();

            return true;
        }

        // imports required for a Windows container to successfully notice when a "docker stop" command
        // has been run and allow for a graceful shutdown of the endpoint
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);
    }
}
