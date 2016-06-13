namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public interface ITopicCreator
    {
        void SetTopic(string connectionString, string topicName, bool update = false);
    }
}