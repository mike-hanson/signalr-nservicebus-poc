using System.Threading.Tasks;
using SignalR.Nsb.Poc.Web.Hubs;

namespace SignalR.Nsb.Poc.Web.Abstractions
{
    public interface IOrderHubClient
    {
        Task SendNotification(OrderStatus orderStatus);
    }
}