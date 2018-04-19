using System.Threading.Tasks;
using NServiceBus;
using SignalR.Nsb.Poc.Messages;
using SignalR.Nsb.Poc.Web.Abstractions;
using SignalR.Nsb.Poc.Web.Hubs;

namespace SignalR.Nsb.Poc.Web.Sagas
{
    public class OrderSaga: Saga<OrderSagaState>
        , IAmStartedByMessages<CreateOrder>
        , IHandleMessages<OrderPlaced>
        , IHandleMessages<OrderBilled>
        , IHandleMessages<OrderShipped>
    {
        private readonly IOrderHubMessageDispatcher _orderHubMessageDispatcher;

        public OrderSaga(IOrderHubMessageDispatcher orderHubMessageDispatcher)
        {
            _orderHubMessageDispatcher = orderHubMessageDispatcher;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaState> mapper)
        {
            mapper.ConfigureMapping<CreateOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
            
            mapper.ConfigureMapping<OrderPlaced>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
            
            mapper.ConfigureMapping<OrderBilled>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
            
            mapper.ConfigureMapping<OrderShipped>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
        }
        
        public async Task Handle(CreateOrder message, IMessageHandlerContext context)
        {
            Data.OrderId = message.OrderId;
            Data.UserId = message.UserId;
            Data.Status = "Pending";

            await SendStatusUpdate().ConfigureAwait(false);
        }

        public async Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            Data.Status = "Order Placed";
            await SendStatusUpdate().ConfigureAwait(false);
        }

        public async Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            Data.Status = "Order Billed";
            await SendStatusUpdate().ConfigureAwait(false);
        }

        public async Task Handle(OrderShipped message, IMessageHandlerContext context)
        {
            Data.Status = "Order Shipped";
            MarkAsComplete();
            await SendStatusUpdate().ConfigureAwait(false);
        }

        private async Task SendStatusUpdate()
        {
            await _orderHubMessageDispatcher.SendToGroupAsync(Data.UserId, "StatusUpdated", new OrderStatus
            {
                OrderId = Data.OrderId,
                Status = Data.Status
            }).ConfigureAwait(false);
        }
    }
}
