using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces;
using Ve.Messaging.Model;
using Ve.Messaging.Publisher;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    [Obsolete("Redundant interface, prefer IMessagePublisher")]
    public class ThriftPublisher : IThriftPublisher // TODO: why does this exist? Does it even do anything different to, IMessagePublisher? Seems to prevent the EventHub behaviour
    {
        private readonly IMessagePublisher _publisher;

        public ThriftPublisher(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public Task SendAsync(Message message)
        {
            return _publisher.SendAsync(message);
        }

        public async Task SendBatchAsync(IEnumerable<Message> messages)
        {
            await Task.WhenAll(messages.Select(SendAsync)).ConfigureAwait(false);
        }
    }
}