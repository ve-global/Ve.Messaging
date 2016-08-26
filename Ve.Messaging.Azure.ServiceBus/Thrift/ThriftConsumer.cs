using System;
using System.Collections.Generic;
using System.Linq;
using Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces;
using Ve.Messaging.Consumer;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    [Obsolete("Prefer extension method: RetrieveThriftMessages")]
    public class ThriftConsumer : IThriftConsumer 
    {
        private readonly IMessageConsumer _consumer;

        public ThriftConsumer(IMessageConsumer consumer)
        {
            _consumer = consumer;
        }

        public IEnumerable<T> RetrieveMessages<T>(int messageAmount, int timeout) where T : new()
        {
            return _consumer.RetrieveMessages(messageAmount, timeout).Select(
                m => ThriftSerializer.Deserialize<T>(m.BodyStream));
        }
    }


}
