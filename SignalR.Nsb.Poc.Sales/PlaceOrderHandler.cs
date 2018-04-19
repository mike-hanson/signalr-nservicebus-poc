using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using SignalR.Nsb.Poc.Messages;

namespace SignalR.Nsb.Poc.Sales
{
    public class PlaceOrderHandler: IHandleMessages<PlaceOrder>
    {
        private static readonly ILog Log = LogManager.GetLogger<PlaceOrderHandler>();
        
        public Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            Log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderPlaced);
        }
    }
}
