using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SignalR.Nsb.Poc.Messages;

namespace SignalR.Nsb.Poc.Shipping
{
    public class OrderPlacedHandler: IHandleMessages<OrderPlaced>
    {
        private static ILog log = LogManager.GetLogger<OrderPlacedHandler>();
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
