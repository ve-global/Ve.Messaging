using System;
using Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces;
using Ve.Messaging.Azure.ServiceBus.Publisher.Wrapper;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public class SimplePublisherClientResolver : IPublisherClientResolver
    {
        private readonly ITopicClientWrapper _topicClient;

        public SimplePublisherClientResolver(ITopicClientWrapper topicClient)
        {
            _topicClient = topicClient;
        }
        public ITopicClientWrapper GetClient()
        {
            return _topicClient;
        }

        public void ReportFailure(ITopicClientWrapper wrapper, Message message, Exception ex = null)
        {
            if (ex != null) throw ex;
        }
    }
}
