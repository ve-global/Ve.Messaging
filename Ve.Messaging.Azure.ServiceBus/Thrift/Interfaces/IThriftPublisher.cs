using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public interface IThriftPublisher
    {
        Task SendAsync<T>(ThriftMessage<T> thriftMessage) where T : new();
        Task SendBatchAsync<T>(IEnumerable<ThriftMessage<T>> messages) where T : new();
    }
}