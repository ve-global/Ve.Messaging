using Ve.Messaging.Azure.ServiceBus.Publisher.Wrapper;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces
{
    public interface ITopicClientCreator
    {
        ITopicClientWrapper CreateTopicClient(
            TopicConfiguration config);

        ITopicClientWrapper CreateTopicClient(
            TopicConfiguration config,
            IVeStatsDClient statsDClient);
    }
}