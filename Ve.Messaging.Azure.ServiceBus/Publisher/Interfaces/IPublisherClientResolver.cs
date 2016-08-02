using System;
using Ve.Messaging.Azure.ServiceBus.Publisher.Wrapper;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces
{
    public interface IPublisherClientResolver
    {
        ITopicClientWrapper GetClient();
        void ReportFailure(ITopicClientWrapper wrapper, Message message, Exception ex = null);
    }
}
