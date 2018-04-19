using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;
using NServiceBus.ObjectBuilder;
using NServiceBus.Serialization;
using NServiceBus.Settings;
using NServiceBus.Transport;
using SignalR.Nsb.Poc.NServiceBus.Abstractions;
using Xunit;

namespace SignalR.Nsb.Poc.NServiceBus.Tests
{
    public class EndpointInstanceBuilderShould
    {
        private const string EndpointAddress = "Endpoint.Address";
        private const string Username = "username";
        private const string QueueName = "queuename";
        private readonly IEndpointInstanceBuilder _target;

        public EndpointInstanceBuilderShould()
        {
            _target = new EndpointInstanceBuilder();
        }

        [Fact]
        public void ReturnSelfOnCreate()
        {
            _target.Create(EndpointAddress)
                .Should().Be(_target);
        }

        [Fact]
        public void ReturnEndpointConfigurationOnRequest()
        {
            var configuration = _target.Create(EndpointAddress).Configuration;
            configuration.Should().NotBeNull()
                .And
                .BeOfType<EndpointConfiguration>();
        }

        [Fact]
        public void ConfigureEndpointNameOnCreate()
        {
            _target.Create(EndpointAddress);

            var settings = _target
                .Configuration.GetSettings();

            settings.EndpointName().Should().Be(EndpointAddress);
        }

        [Fact]
        public void ReturnSelfOnUsingTransport()
        {
            _target.Create(EndpointAddress)
                .WithTransport<LearningTransport>()
                .Should()
                .Be(_target);
        }

        [Fact]
        public void CallActionOnWithTransport()
        {
            TransportExtensions<LearningTransport> transport = null;
            _target.Create(EndpointAddress)
                .WithTransport<LearningTransport>(tc => transport = tc);

            transport.Should().NotBeNull();
        }

        [Fact]
        public void ConfigureTransportSettings()
        {
            _target.Create(EndpointAddress)
                .WithTransport<LearningTransport>();

            var transportDefinition = _target.Configuration.GetSettings()
                .GetOrDefault<TransportDefinition>();
            transportDefinition.Should().NotBeNull();
            transportDefinition.Should().BeAssignableTo<LearningTransport>();
        }

        [Fact]
        public void ReturnSelfOnWithInstallersEnabled()
        {
            _target.Create(EndpointAddress).WithInstallersEnabled()
                .Should().Be(_target);
        }

        [Fact]
        public void EnableInstallers()
        {
            _target.Create(EndpointAddress).WithInstallersEnabled()
                .Should().Be(_target);

            _target.Configuration.GetSettings()
                .TryGet("Installers.Enable", out bool installersEnabled)
                .Should().BeTrue();
            installersEnabled.Should().BeTrue();
        }

        [Fact]
        public void EnableInstallersWithUsername()
        {
            _target.Create(EndpointAddress).WithInstallersEnabled(Username)
                .Should().Be(_target);

            _target.Configuration.GetSettings()
                .TryGet("Installers.Username", out string username)
                .Should().BeTrue();
            username.Should().Be(Username);
        }

        [Fact]
        public void ReturnSelfOnWithPersistence()
        {
            _target.Create(EndpointAddress).WithPersistence<InMemoryPersistence>()
                .Should().Be(_target);
        }

        [Fact]
        public void ConfigurePersistence()
        {
            _target.Create(EndpointAddress).WithPersistence<InMemoryPersistence>();

            var settings = _target.Configuration.GetSettings();
            settings.HasSetting("PersistenceDefinitions").Should().BeTrue();

            var definitions = settings.Get("PersistenceDefinitions").As<IList>();
            definitions.Should().NotBeNullOrEmpty();
            definitions.Count.Should().BeGreaterOrEqualTo(1);

            // type of definitions returned is internal so
            // have to investigate elegant way to check the right type
            // is configured
        }

        [Fact]
        public void ReturnSelfOnWithSerialization()
        {
            _target.Create(EndpointAddress).WithSerialization<NewtonsoftSerializer>()
                .Should().Be(_target);
        }

        [Fact]
        public void ConfigureSerialization()
        {
            _target.Create(EndpointAddress).WithSerialization<NewtonsoftSerializer>();

            var settings = _target.Configuration.GetSettings();
            settings.HasSetting("MainSerializer").Should().BeTrue();

            var mainSerializer = (Tuple<SerializationDefinition, SettingsHolder>)settings.Get("MainSerializer");
            mainSerializer.Should().NotBeNull();
            mainSerializer.Item1.Should().BeOfType<NewtonsoftSerializer>();
        }

        [Fact]
        public void ReturnSelfOnWithFailedMessagesTo()
        {
            _target.Create(EndpointAddress).WithFailedMessagesTo(QueueName)
                .Should().Be(_target);
        }

        [Fact]
        public void ConfigureFailedMessageQueue()
        {
            _target.Create(EndpointAddress).WithFailedMessagesTo(QueueName);

            _target.Configuration.GetSettings().ErrorQueueAddress().Should().Be(QueueName);
        }

        [Fact]
        public void ReturnSelfOnWithAuditProcessMessageTo()
        {
            _target.Create(EndpointAddress).WithAuditProcessMessageTo(QueueName)
                .Should().Be(_target);
        }

        [Fact]
        public void ConfigureAuditProcessMessageQueue()
        {
            _target.Create(EndpointAddress).WithAuditProcessMessageTo(QueueName);

            _target.Configuration.GetSettings().TryGetAuditQueueAddress(out string queueAddress);
            queueAddress.Should().Be(QueueName);
        }

        [Fact]
        public void ReturnSelfOnWithRegisteredComponents()
        {
            _target.Create(EndpointAddress).WithRegisteredComponents(c => { }).Should().Be(_target);
        }

        [Fact]
        public async Task CallActionOnBuildWhenComponentRegistrationsDefined()
        {
            IConfigureComponents configureComponents = null;
            var endpoint = await _target.Create(EndpointAddress)
                .WithTransport<LearningTransport>(tc => tc.StorageDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
                .WithRegisteredComponents(c => configureComponents = c)
                .Build();

            configureComponents.Should().NotBeNull();
            configureComponents.Should().BeAssignableTo<IConfigureComponents>();
        }

        [Fact]
        public async Task ReturnStartableInstanceOnBuild()
        {
            var endpoint = await _target.Create(EndpointAddress)
                .WithTransport<LearningTransport>(tc => tc.StorageDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
                .Build();

            endpoint.Should()
            .BeAssignableTo<IStartableEndpoint>();
        }

        [Fact]
        public void ApplyDefaultsForPocOnCreate()
        {
            var configuration = _target.Create(EndpointAddress).Configuration;

            var settings = configuration.GetSettings();
            settings.ErrorQueueAddress().Should().Be("error");
            settings.TryGetAuditQueueAddress(out string queueAddress);
            queueAddress.Should().Be("audit");

            var transportDefinition = settings.GetOrDefault<TransportDefinition>();
            transportDefinition.Should().NotBeNull();
            transportDefinition.Should().BeAssignableTo<RabbitMQTransport>();

            settings.TryGet("Installers.Enable", out bool installersEnabled)
               .Should().BeTrue();
            installersEnabled.Should().BeTrue();

            settings.HasSetting("PersistenceDefinitions").Should().BeTrue();

            var definitions = settings.Get("PersistenceDefinitions").As<IList>();
            definitions.Should().NotBeNullOrEmpty();
            definitions.Count.Should().Be(1);

            settings.HasSetting("MainSerializer").Should().BeTrue();

            var mainSerializer = (Tuple<SerializationDefinition, SettingsHolder>)settings.Get("MainSerializer");
            mainSerializer.Should().NotBeNull();
            mainSerializer.Item1.Should().BeOfType<NewtonsoftSerializer>();
        }
    }
}
