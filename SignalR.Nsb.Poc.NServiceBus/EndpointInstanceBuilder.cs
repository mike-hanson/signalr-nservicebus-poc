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
            
            var tansport = _configuration.UseTransport<RabbitMQTransport>();

            // swap these lines when running in containers
            // this doesn't work at the moment due to issues accessing RabbitMQ from containers
            tansport.ConnectionString("host=rabbitmq;username=admin;password=password");
//            tansport.ConnectionString("host=WP29007.flprod.co.uk");
            tansport.UseConventionalRoutingTopology();

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