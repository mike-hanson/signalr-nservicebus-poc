using System;
using System.Threading.Tasks;
using FluentAssertions;
using NServiceBus.Testing;
using NSubstitute;
using SignalR.Nsb.Poc.Messages;
using SignalR.Nsb.Poc.Web.Abstractions;
using SignalR.Nsb.Poc.Web.Hubs;
using SignalR.Nsb.Poc.Web.Sagas;
using Xunit;

namespace SignalR.Nsb.Poc.Web.Tests
{
    public class OrderSagaShould
    {
        private const string UserId = "user";
        private IOrderHubMessageDispatcher _orderHubMessageDispatcher;
        private readonly OrderSaga _target;
        private readonly string _orderId = Guid.NewGuid().ToString();

        public OrderSagaShould()
        {
            AssumeOrderHubMessageDispatcherIsInitialised();
            _target = new OrderSaga(_orderHubMessageDispatcher) {Data = new OrderSagaState()};
        }

        [Fact]
        public void ConfigureHowToFindSagaCorrectly()
        {
           // testing of mappings is not supported at the moment :-(
        }

        [Fact]
        public async Task HandleCreateOrderMessageCorrectly()
        {
            var context = new TestableMessageHandlerContext();
            var message = new CreateOrder
            {
                OrderId = _orderId,
                UserId = UserId
            };

            await _target.Handle(message, context);

            await _orderHubMessageDispatcher.Received().SendToGroupAsync(UserId, "StatusUpdated",
                Arg.Is<OrderStatus>(s => s.OrderId == message.OrderId && s.Status == "Pending"));
        }

        [Fact]
        public async Task HandleOrderPlacedMessageCorrectly()
        {
            AssumeSagaCanBeMatched();
            var context = new TestableMessageHandlerContext();
            var message = new OrderPlaced
            {
                OrderId = _orderId,
            };

            await _target.Handle(message, context);

            await _orderHubMessageDispatcher.Received().SendToGroupAsync(UserId, "StatusUpdated",
                Arg.Is<OrderStatus>(s => s.OrderId == message.OrderId && s.Status == "Order Placed"));
        }

        [Fact]
        public async Task HandleOrderBilledMessageCorrectly()
        {
            AssumeSagaCanBeMatched();
            var context = new TestableMessageHandlerContext();
            var message = new OrderBilled
            {
                OrderId = _orderId,
            };

            await _target.Handle(message, context);

            await _orderHubMessageDispatcher.Received().SendToGroupAsync(UserId, "StatusUpdated",
                Arg.Is<OrderStatus>(s => s.OrderId == message.OrderId && s.Status == "Order Billed"));
        }

        [Fact]
        public async Task HandleOrderShippedMessageCorrectly()
        {
            AssumeSagaCanBeMatched();
            var context = new TestableMessageHandlerContext();
            var message = new OrderShipped
            {
                OrderId = _orderId,
            };

            await _target.Handle(message, context);

            await _orderHubMessageDispatcher.Received().SendToGroupAsync(UserId, "StatusUpdated",
                Arg.Is<OrderStatus>(s => s.OrderId == message.OrderId && s.Status == "Order Shipped"));
            _target.Completed.Should().BeTrue();
        }

        private void AssumeOrderHubMessageDispatcherIsInitialised()
        {
            _orderHubMessageDispatcher = Substitute.For<IOrderHubMessageDispatcher>();
        }

        private void AssumeSagaCanBeMatched()
        {
            _target.Data.OrderId = _orderId;
            _target.Data.UserId = UserId;
        }
    }
}
