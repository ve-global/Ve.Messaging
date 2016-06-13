using System.IO;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public class ThriftMessage<T> : Message where T : new()
    {
        public ThriftMessage(T content) : base(ThriftSerializer.Serialize<T>(content))
        {   
        }

        public ThriftMessage(Stream bodyContent) : base(bodyContent)
        {
            Content = ThriftSerializer.Deserialize<T>(bodyContent);
        }

        public T Content { get; }
    }
}