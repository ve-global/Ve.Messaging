using System;
using Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces;
using Ve.Messaging.Publisher;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public class PublisherFactory : IPublisherFactory
    {
        private readonly IVeStatsDClient _statsDClient;
        private readonly ITopicClientCreator _topicClientCreator;

        public PublisherFactory(IVeStatsDClient statsDClient, ITopicClientCreator topicClientCreator)
        {
            _statsDClient = statsDClient;
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
                primaryTopicClient,
                failoverTopicClient,
                _statsDClient);

           return new MessagePublisher(failoverPublisher);
        }
    }
}