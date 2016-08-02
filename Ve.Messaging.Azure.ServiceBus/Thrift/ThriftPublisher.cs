using System.Collections.Generic;
using System.Threading.Tasks;
using Ve.Messaging.Publisher;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public class ThriftPublisher : IThriftPublisher
    {
        private readonly IMessagePublisher _publisher;

        public ThriftPublisher(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public Task SendAsync<T>(ThriftMessage<T> thriftMessage) where T : new()
        {
            return _publisher.SendAsync(thriftMessage);
        }

        public async Task SendBatchAsync<T>(IEnumerable<ThriftMessage<T>> messages) where T : new()
        {
            foreach (var message in messages)
            {
                await SendAsync(message).ConfigureAwait(false);
            }
        }
    }
}