using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.Persistence;
using NServiceBus.Serialization;
using NServiceBus.Transport;

namespace SignalR.Nsb.Poc.NServiceBus.Abstractions
{
    public interface IEndpointInstanceBuilder
    {
        IEndpointInstanceBuilder Create(string endpointAddress);
        EndpointConfiguration Configuration { get; }
        Task<IStartableEndpoint> Build();
        IEndpointInstanceBuilder WithTransport<T>(Action<TransportExtensions<T>> transportConfigurator = null) where T: TransportDefinition, new();
        IEndpointInstanceBuilder WithInstallersEnabled(string username = null);
        IEndpointInstanceBuilder WithPersistence<T>() where T: PersistenceDefinition;
        IEndpointInstanceBuilder WithSerialization<T>() where T: SerializationDefinition, new();
        IEndpointInstanceBuilder WithFailedMessagesTo(string queueName);
        IEndpointInstanceBuilder WithAuditProcessMessageTo(string queueName);
        IEndpointInstanceBuilder WithRegisteredComponents(Action<IConfigureComponents> containerConfigurator);
    }
}
