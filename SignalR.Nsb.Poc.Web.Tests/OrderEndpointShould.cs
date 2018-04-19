using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using NSubstitute;
using NSubstitute.Extensions;
using SignalR.Nsb.Poc.Messages;
using SignalR.Nsb.Poc.NServiceBus.Abstractions;
using SignalR.Nsb.Poc.Web.Abstractions;
using SignalR.Nsb.Poc.Web.Endpoints;
using Xunit;

namespace SignalR.Nsb.Poc.Web.Tests
{
    public class OrderEndpointShould
    {
        private readonly OrderEndpoint _target;
        private IEndpointInstanceBuilder _endpointInstanceBuilder;
        private IStartableEndpoint _startableEndpoint;
        private IEndpointInstance _endpointInstance;
        private IOrderHubMessageDispatcher _orderHubMessageDispatcher;

        public OrderEndpointShould()
        {
            AssumeEndpointInstanceBuilderIsInitialised();
            AssumeOrderHubMessageDispatcher();
            _target = new OrderEndpoint(_endpointInstanceBuilder, _orderHubMessageDispatcher);
        }

        [Fact]
        public async Task UseBuilderToCreateAndStartEndpointOnConnect()
        {
            _endpointInstanceBuilder.Received().Create("SignalR.Nsb.Poc.Web.Order");
            _endpointInstanceBuilder.Received().WithTransport(Arg.Any<Action<TransportExtensions<RabbitMQTransport>>>());
            _endpointInstanceBuilder.Received().WithRegisteredComponents(Arg.Any<Action<IConfigureComponents>>());
            await _endpointInstanceBuilder.Received().Build();
            await _startableEndpoint.Received().Start();
        }

        [Fact]
        public async Task UseEndpointInstanceToSendLocal()
        {
            var createOrderCommand = new CreateOrder {OrderId = Guid.NewGuid().ToString(), UserId = "mike.hanson"};
            
            await _target.SendLocal(createOrderCommand);

            // SendLocal on IEndpointInstance is an extension method
            // so we can only assert that it delegates to the Send method correctly
            await _endpointInstance.Received().Send(createOrderCommand, Arg.Any<SendOptions>());
        }

        [Fact]
        public async Task UseEndpointInstanceToSend()
        {
            var placeOrderCommand = new PlaceOrder() { OrderId = Guid.NewGuid().ToString()};

            await _target.Send(placeOrderCommand);

            // Send with only a message on IEndpointInstance is an extension method
            // so we can only assert that it delegates to the Send method correctly
            await _endpointInstance.Received().Send(placeOrderCommand, Arg.Any<SendOptions>());
        }

        private void AssumeEndpointInstanceBuilderIsInitialised()
        {
            _endpointInstanceBuilder = Substitute.For<IEndpointInstanceBuilder>();
            _endpointInstanceBuilder.ReturnsForAll(_endpointInstanceBuilder);

            _startableEndpoint = Substitute.For<IStartableEndpoint>();
            _endpointInstanceBuilder.Build().Returns(Task.FromResult(_startableEndpoint));

            _endpointInstance = Substitute.For<IEndpointInstance>();
            _startableEndpoint.Start().Returns(Task.FromResult(_endpointInstance));
        }

        private void AssumeOrderHubMessageDispatcher()
        {
            _orderHubMessageDispatcher = Substitute.For<IOrderHubMessageDispatcher>();
        }
    }
}
