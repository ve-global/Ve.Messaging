namespace Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces
{
    public interface ITopicCreator
    {
        void SetTopic(string connectionString, string topicName, bool update = false);
    }
}