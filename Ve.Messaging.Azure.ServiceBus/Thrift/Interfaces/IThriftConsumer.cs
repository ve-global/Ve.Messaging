using System.Collections.Generic;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    public interface IThriftConsumer
    {
        IEnumerable<T> RetrieveMessages<T>(int messageAmount, int timeout) where T : new();
    }
}