using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SignalR.Nsb.Poc.NServiceBus;

namespace SignalR.Nsb.Poc.Shipping
{
    internal class Host
    {
        static readonly ILog Log = LogManager.GetLogger<Host>();

        IEndpointInstance _endpointInstance;

        public string Label => "SignalR.Nsb.Poc.Shipping";

        public async Task Start()
        {

            try
            {
                var builder = new EndpointInstanceBuilder();
                var startableEndpoint = await builder.Create(Label).Build();
                _endpointInstance = await startableEndpoint.Start();
            }
            catch (Exception ex)
            {
                FailFast("Failed to start.", ex);
            }
        }

        public async Task Stop()
        {
            try
            {
                await _endpointInstance?.Stop();
            }
            catch (Exception ex)
            {
                FailFast("Failed to stop correctly.", ex);
            }
        }

        internal async Task OnCriticalError(ICriticalErrorContext context)
        {
            try
            {
                await context.Stop();
            }
            finally
            {
                FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }

        private void FailFast(string message, Exception exception)
        {
            try
            {
                Log.Fatal(message, exception);
            }
            finally
            {
                Environment.FailFast(message, exception);
            }
        }
    }
}
