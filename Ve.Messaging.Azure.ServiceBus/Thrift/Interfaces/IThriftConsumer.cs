using System.Collections.Generic;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public interface IThriftConsumer
    {
        IEnumerable<ThriftMessage<T>> RetrieveMessages<T>(int messageAmount, int timeout) where T : new();
    }
}