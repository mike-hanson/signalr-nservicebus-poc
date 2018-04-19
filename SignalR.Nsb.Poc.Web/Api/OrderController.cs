using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalR.Nsb.Poc.Messages;
using SignalR.Nsb.Poc.Web.Abstractions;

namespace SignalR.Nsb.Poc.Web.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderEndpoint _orderEndPoint;
        private readonly IUserIdentityProvider _userIdentityProvider;

        public OrderController(IOrderEndpoint orderEndPoint, 
            IUserIdentityProvider userIdentityProvider)
        {
            _orderEndPoint = orderEndPoint;
            _userIdentityProvider = userIdentityProvider;
        }

        public async Task<IActionResult> Post()
        {
            var orderId = Guid.NewGuid().ToString();
            await _orderEndPoint.SendLocal(new CreateOrder
            {
                OrderId = orderId,
                UserId = _userIdentityProvider.CurrentUserId()
            });

            await _orderEndPoint.Send(new PlaceOrder {OrderId = orderId});

            return Ok();
        }
    }
}