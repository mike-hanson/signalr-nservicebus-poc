using System.Threading.Tasks;
using NServiceBus;
using SignalR.Nsb.Poc.Messages;

namespace SignalR.Nsb.Poc.Web.Abstractions
{
    public interface IOrderEndpoint
    {
        Task SendLocal<T>(T message) where T: ICommand;
        Task Send<T>(T message) where T: ICommand;
    }
}
