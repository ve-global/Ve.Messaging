using System.Collections.Generic;
using System.IO;

namespace Ve.Messaging.Model
{
    public class Message
    {
        public Message(Stream bodyStream)
        {
            BodyStream = bodyStream;
        }
        public Stream BodyStream { get; set; }
        public string Label { get; set; }
        public string SessionId { get; set; }
        public Dictionary<string, object> Properties { get; set; } 
    }
}
