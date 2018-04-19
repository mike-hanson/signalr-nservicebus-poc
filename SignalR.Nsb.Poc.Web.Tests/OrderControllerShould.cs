using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SignalR.Nsb.Poc.Messages;
using SignalR.Nsb.Poc.Web.Abstractions;
using SignalR.Nsb.Poc.Web.Api;
using Xunit;

namespace SignalR.Nsb.Poc.Web.Tests
{
    public class OrderControllerShould
    {
        private const string UserId = "mike.hanson";

        private readonly OrderController _target;
        private IOrderEndpoint _orderEndPoint;
        private IUserIdentityProvider _userIdentityProvider;

        public OrderControllerShould()
        {
            AssumeOrderEndpointIsInitialised();
            AssumeUserIdentityProviderIsInitialised();
            _target = new OrderController(_orderEndPoint, _userIdentityProvider);
        }

        [Fact]
        public async Task UseProviderToGetCurrentUserId()
        {
            await _target.Post();

            _userIdentityProvider.Received().CurrentUserId();
        }

        [Fact]
        public async Task UseEndpointToSendCreateOrderCommand()
        {
            await _target.Post();

            await _orderEndPoint.Received()
                .SendLocal(Arg.Is<CreateOrder>(c => c.UserId == UserId && c.OrderId != Guid.Empty.ToString()));
        }

        [Fact]
        public async Task UseEndpointToSendPlaceOrderCommand()
        {
            await _target.Post();

            await _orderEndPoint.Received()
                .Send(Arg.Is<PlaceOrder>(c => c.OrderId != Guid.Empty.ToString()));
        }

        [Fact]
        public async Task ReturnOkResult()
        {
            var result = await _target.Post();

            result.Should().BeAssignableTo<OkResult>();
        }

        private void AssumeOrderEndpointIsInitialised()
        {
            _orderEndPoint = Substitute.For<IOrderEndpoint>();
        }

        private void AssumeUserIdentityProviderIsInitialised()
        {
            _userIdentityProvider = Substitute.For<IUserIdentityProvider>();
            _userIdentityProvider.CurrentUserId().Returns(UserId);
        }
    }
}
