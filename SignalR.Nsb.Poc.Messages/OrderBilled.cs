using NServiceBus;
using SignalR.Nsb.Poc.Messages.Abstractions;

namespace SignalR.Nsb.Poc.Messages
{
    public class OrderBilled: OrderMessage, IEvent
    {
    }
}
