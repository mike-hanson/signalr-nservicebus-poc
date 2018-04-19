using System;
using System.Threading.Tasks;
using SignalR.Nsb.Poc.NServiceBus;

namespace SignalR.Nsb.Poc.Sales
{
    class Program
    {
        private const string Label = "SignalR.Nsb.Poc.Sales";

        static async Task Main()
        {
            Console.Title = Label;
            
            var builder = new EndpointInstanceBuilder();
            var startableEndpoint = await builder.Create(Label).Build();
            var endpointInstance = await startableEndpoint.Start();

            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
