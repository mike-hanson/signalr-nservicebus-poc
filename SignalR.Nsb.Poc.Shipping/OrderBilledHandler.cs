using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SignalR.Nsb.Poc.Messages;

namespace SignalR.Nsb.Poc.Shipping
{
    public class OrderBilledHandler : IHandleMessages<OrderBilled>
    {
        private static readonly ILog Log = LogManager.GetLogger<OrderBilledHandler>();
        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Log.Info($"Received OrderBilled, OrderId = {message.OrderId}");
            await context.Publish(new OrderShipped { OrderId = message.OrderId }).ConfigureAwait(false);
        }
    }
}
