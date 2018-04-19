using SignalR.Nsb.Poc.Web.Hubs;

namespace SignalR.Nsb.Poc.Web.Abstractions
{
    public interface IOrderHubMessageDispatcher: IHubMessageDispatcher<OrderHub>
    {
    }
}