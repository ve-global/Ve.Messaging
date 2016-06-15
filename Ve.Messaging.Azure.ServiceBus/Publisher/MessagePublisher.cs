using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Publisher;
using Ve.Messaging.Serializer;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IPublisherClientResolver _publisherClientResolver;

        public MessagePublisher(IPublisherClientResolver publisherClientResolver)
        {
            _publisherClientResolver = publisherClientResolver;
        }

        public async Task SendAsync(Message message)
        {
            var topicClient = _publisherClientResolver.GetClient();
            var brokeredMessage = BrokeredMessageBuilder.SerializeToBrokeredMessage(message);

            try
            {
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