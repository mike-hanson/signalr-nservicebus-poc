using System;
using System.Threading.Tasks;
using NServiceBus;
using SignalR.Nsb.Poc.NServiceBus;

namespace SignalR.Nsb.Poc.Shipping
{
    class Program
    {
        private const string Label = "SignalR.Nsb.Poc.Shipping";

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
