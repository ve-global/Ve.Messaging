using System.Collections.Generic;
using System.Threading.Tasks;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    public interface IThriftPublisher
    {
        Task SendAsync<T>(ThriftMessage<T> thriftMessage) where T : new();
        Task SendBatchAsync<T>(IEnumerable<ThriftMessage<T>> messages) where T : new();
    }
}