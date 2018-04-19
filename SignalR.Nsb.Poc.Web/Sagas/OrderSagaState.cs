using System;
using NServiceBus;

namespace SignalR.Nsb.Poc.Web.Sagas
{
    public class OrderSagaState : ContainSagaData
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
    }
}