using Ve.Messaging.Publisher;

namespace Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces
{
    public interface IPublisherFactory
    {
        IMessagePublisher CreatePublisher(ServiceBusPublisherConfiguration config);
    }
}