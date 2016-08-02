using System.Collections.Generic;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    public interface IThriftConsumer
    {
        IEnumerable<ThriftMessage<T>> RetrieveMessages<T>(int messageAmount, int timeout) where T : new();
    }
}