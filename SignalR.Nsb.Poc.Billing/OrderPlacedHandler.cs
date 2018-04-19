using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SignalR.Nsb.Poc.Messages;

namespace SignalR.Nsb.Poc.Billing
{
    public class OrderPlacedHandler: IHandleMessages<OrderPlaced>
    {
        private static readonly ILog Log = LogManager.GetLogger<OrderPlacedHandler>();
        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Log.Info($"Received OrderPlaced, OrderId = {message.OrderId}");
            await context.Publish(new OrderBilled
            {
                OrderId = message.OrderId
            });
        }
    }
}
