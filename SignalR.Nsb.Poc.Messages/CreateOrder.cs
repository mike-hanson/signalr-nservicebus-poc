using NServiceBus;
using SignalR.Nsb.Poc.Messages.Abstractions;

namespace SignalR.Nsb.Poc.Messages
{
    public class CreateOrder : OrderMessage, ICommand
    {
        public string UserId { get; set; }
    }
}
