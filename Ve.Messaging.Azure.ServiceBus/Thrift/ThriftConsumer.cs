using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ve.Messaging.Consumer;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public class ThriftConsumer
    {
        private IMessageConsumer _consumer;

        public ThriftConsumer(IMessageConsumer consumer)
        {
            _consumer = consumer;
        }
        public IEnumerable<ThriftMessage<T>> RetrieveMessages<T>(int messageAmount, int timeout) where T : new()
        {
            return _consumer.RetrieveMessages(messageAmount, timeout).Select(
                m => new ThriftMessage<T>(m));
        }

    }
}
