using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Publisher;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public class ThriftPublisher : IMessagePublisher
    {
        private readonly IPublisherClientResolver _publisherClientResolver;

        public ThriftPublisher(IPublisherClientResolver publisherClientResolver)
        {
            _publisherClientResolver = publisherClientResolver;
        }

        public async Task SendAsync(Message message)
        {
            var topicClient = _publisherClientResolver.GetClient();
            try
            {
                var brokeredMessage = BrokeredMessageBuilder.SerializeToBrokeredMessage(message);
                await topicClient.SendAsync(brokeredMessage).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _publisherClientResolver.ReportFailure(topicClient, message, ex);
            }
        }

        public async Task SendBatchAsync(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
            {
                await SendAsync(message).ConfigureAwait(false);
            }
        }
    }
}