using System.Collections.Generic;
using System.IO;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public class ThriftMessage<T> : Message where T : new()
    {
        public ThriftMessage(T content,
                             string sessionId = "",
                             string label = "",
                             Dictionary<string, object> properties = null)
                             : base(ThriftSerializer.Serialize<T>(content), sessionId, label, properties)
        {
            Content = content;
        }

        public ThriftMessage(Stream bodyContent,
                             string sessionId = "",
                             string label = "",
                             Dictionary<string, object> properties = null)
                             : base(bodyContent, sessionId, label, properties)
        {
            Content = ThriftSerializer.Deserialize<T>(bodyContent);
        }

        public ThriftMessage(Message message) : base(message.BodyStream, 
                                                     message.SessionId,
                                                     message.Label,
                                                     message.Properties)
        {
            Content = ThriftSerializer.Deserialize<T>(message.BodyStream);
        }

        public T Content { get; }
    }
}