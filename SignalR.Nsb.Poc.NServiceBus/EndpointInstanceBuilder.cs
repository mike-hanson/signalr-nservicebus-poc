using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.Persistence;
using NServiceBus.Serialization;
using NServiceBus.Transport;
using SignalR.Nsb.Poc.NServiceBus.Abstractions;

namespace SignalR.Nsb.Poc.NServiceBus
{
    public class EndpointInstanceBuilder: IEndpointInstanceBuilder
    {
        private EndpointConfiguration _configuration;

        public EndpointConfiguration Configuration => _configuration;

        public IEndpointInstanceBuilder Create(string endpointAddress)
        {
            _configuration = new EndpointConfiguration(endpointAddress);
            
            var transport = _configuration.UseTransport<RabbitMQTransport>();

            // swap these lines to switch between running in containers or running locally
            // not to run locally you wil need to install RabbitMQ
            //transport.ConnectionString("host=localhost");
            transport.ConnectionString("host=rabbitmq;username=admin;password=password");
            transport.UseConventionalRoutingTopology();

            _configuration.EnableInstallers();
            _configuration.AuditProcessedMessagesTo("audit");
            _configuration.UsePersistence<InMemoryPersistence>();
            _configuration.UseSerialization<NewtonsoftSerializer>();
            return this;
        }

        public Task<IStartableEndpoint> Build()
        {
            return Endpoint.Create(_configuration);
        }

        public IEndpointInstanceBuilder WithTransport<T>(Action<TransportExtensions<T>> transportConfigurator = null) 
            where T : TransportDefinition, new()
        {
            var transport = _configuration.UseTransport<T>();
            transportConfigurator?.Invoke(transport);
            return this;
        }

        public IEndpointInstanceBuilder WithInstallersEnabled(string username = null)
        {
            _configuration.EnableInstallers(username);
            return this;
        }

        public IEndpointInstanceBuilder WithPersistence<T>() where T : PersistenceDefinition
        {
            _configuration.UsePersistence<T>();
            return this;
        }

        public IEndpointInstanceBuilder WithSerialization<T>() where T : SerializationDefinition, new()
        {
            _configuration.UseSerialization<T>();
            return this;
        }

        public IEndpointInstanceBuilder WithFailedMessagesTo(string queueName)
        {
            _configuration.SendFailedMessagesTo(queueName);
            return this;
        }

        public IEndpointInstanceBuilder WithAuditProcessMessageTo(string queueName)
        {
            _configuration.AuditProcessedMessagesTo(queueName);
            return this;
        }

        public IEndpointInstanceBuilder WithRegisteredComponents(Action<IConfigureComponents> containerConfigurator)
        {
            _configuration.RegisterComponents(containerConfigurator);
            return this;
        }
    }
}