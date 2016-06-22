using System.Collections.Generic;
using System.IO;

namespace Ve.Messaging.Model
{
    public class Message
    {
        public Message(Stream bodyStream,
                       string sessionId = "",
                       string label = "",
                       IDictionary<string, object> properties = null)
        {
            BodyStream = bodyStream;
            SessionId = sessionId;
            Label = label;
            Properties = properties;
        }
        public Stream BodyStream { get; }
        public string Label { get; }
        public string SessionId { get; }
        public IDictionary<string, object> Properties { get; } 
    }
}
