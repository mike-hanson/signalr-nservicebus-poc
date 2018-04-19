using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Nsb.Poc.Web.Abstractions;

namespace SignalR.Nsb.Poc.Web.Hubs
{
    public class OrderHubMessageDispatcher : HubMessageDispatcher<OrderHub>, IOrderHubMessageDispatcher
    {
        public OrderHubMessageDispatcher(IHubContext<OrderHub> hubContext) : base(hubContext)
        {
        }
    }
}
