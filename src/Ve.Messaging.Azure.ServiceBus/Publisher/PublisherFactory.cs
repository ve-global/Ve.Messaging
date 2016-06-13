using System;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Publisher;
using Ve.Metrics.StatsDClient.Abstract;
using ISerializer = Ve.Messaging.Serializer.ISerializer;

namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public class PublisherFactory
    {
        private readonly IVeStatsDClient _statsDClient;
        private readonly IFailoverResolver _failoverResolver;
        private readonly ITopicClientCreator _topicClientCreator;

        public PublisherFactory(IVeStatsDClient statsDClient,
            IFailoverResolver failoverResolver,
            ITopicClientCreator topicClientCreator)
        {
            _statsDClient = statsDClient;
            _failoverResolver = failoverResolver;
            _topicClientCreator = topicClientCreator;
        }

        public IMessagePublisher CreatePublisher(ServiceBusPublisherConfiguration config)
        {
            IMessagePublisher publisher;
            switch (config.ServiceBusPublisherStrategy)
            {
                case ServiceBusPublisherStrategy.Simple:
                    publisher = GetSimpleMessagePublisher(config);
                    break;

                case ServiceBusPublisherStrategy.FailOverEnable:
                    publisher = GetFailoverMessagePublisher(config);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return publisher;
        }

        private IMessagePublisher GetSimpleMessagePublisher(ServiceBusPublisherConfiguration config)
        {
            var simpleTopicClient = _topicClientCreator.CreateTopicClient(config.PrimaryConfiguration,
                _statsDClient);
            var resolver = new SimplePublisherClientResolver(simpleTopicClient);
            return new MessagePublisher(resolver);
        }

        private IMessagePublisher GetFailoverMessagePublisher(ServiceBusPublisherConfiguration config)
        {
            var primaryTopicClient = _topicClientCreator.CreateTopicClient(config.PrimaryConfiguration,
                _statsDClient);

            var failoverTopicClient = _topicClientCreator.CreateTopicClient(config.FailOverConfiguration,
                _statsDClient);

            var failoverPublisher = new FailoverPublisherClientResolver(
                _failoverResolver,
                primaryTopicClient,
                failoverTopicClient,
                _statsDClient);

           return new MessagePublisher(failoverPublisher);
        }
    }
}