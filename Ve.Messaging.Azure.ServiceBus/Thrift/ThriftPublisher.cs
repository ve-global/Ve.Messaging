using System.Collections.Generic;
using System.Threading.Tasks;
using Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces;
using Ve.Messaging.Model;
using Ve.Messaging.Publisher;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public class ThriftPublisher : IThriftPublisher
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
            foreach (var message in messages)
            {
                await SendAsync(message).ConfigureAwait(false);
            }
        }
    }
}