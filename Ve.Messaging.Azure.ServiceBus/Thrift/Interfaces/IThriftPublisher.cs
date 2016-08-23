using System.Collections.Generic;
using System.Threading.Tasks;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    public interface IThriftPublisher
    {
        Task SendAsync(Message thriftMessage);
        Task SendBatchAsync(IEnumerable<Message> messages);
    }
}