using System;
using System.Collections.Generic;
using System.Linq;
using Thrift.Protocol;
using Ve.Messaging.Consumer;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    public static class ThriftMessageConsumerExts
    {
        public static IEnumerable<T> RetrieveMessages<T>(this IMessageConsumer consumer, int messageAmount, int timeout) where T : TBase, new()
        {
            return consumer.RetrieveMessages(messageAmount, timeout).Select(m => ThriftSerializer.Deserialize<T>(m.BodyStream));
        }

        public static IEnumerable<TransactionalMessage<T>> RetrieveTransactionalMessages<T>(this IMessageConsumer consumer, int messageAmount, int timeout) where T : TBase, new()
        {
            return consumer.RetrieveMessages(messageAmount, timeout)
                           .Select(x=> new TransactionalMessage<T>(ThriftSerializer.Deserialize<T>(x.BodyStream), x.Complete));
        }
    }
}