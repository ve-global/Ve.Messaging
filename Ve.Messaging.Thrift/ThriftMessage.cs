using System.Collections.Generic;
using System.IO;
using Ve.Messaging.Model;

namespace Ve.Messaging.Thrift
{
    public class ThriftMessage<T> : Message where T : new()
    {
        public ThriftMessage(T content,
                             string sessionId = "",
                             string label = "",
                             string id = "",
                             IDictionary<string, object> properties = null)
                             : base(ThriftSerializer.Serialize(content), sessionId, label, id, properties)
        {
            Content = content;
        }

        public ThriftMessage(Stream bodyContent,
                             string sessionId = "",
                             string label = "",
                             string id = "",
                             IDictionary <string, object> properties = null)
                             : base(bodyContent, sessionId, label, id, properties)
        {
            Content = ThriftSerializer.Deserialize<T>(bodyContent);
        }

        public ThriftMessage(Message message) : base(message.BodyStream, 
                                                     message.SessionId,
                                                     message.Label,
                                                     message.Id,
                                                     message.Properties)
        {
            Content = ThriftSerializer.Deserialize<T>(message.BodyStream);
        }

        public T Content { get; }
    }
}