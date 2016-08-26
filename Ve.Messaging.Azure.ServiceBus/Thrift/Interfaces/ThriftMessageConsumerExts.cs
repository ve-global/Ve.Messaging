using System.Collections.Generic;
using System.Linq;
using Ve.Messaging.Consumer;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    public static class ThriftMessageConsumerExts
    {
        public static IEnumerable<T> RetrieveMessages<T>(this IMessageConsumer consumer, int messageAmount, int timeout) where T : new()
        {
            return consumer.RetrieveMessages(messageAmount, timeout).Select(m => ThriftSerializer.Deserialize<T>(m.BodyStream));
        }
    }
}