using System;
using System.Collections.Generic;
using System.IO;

namespace Ve.Messaging.Model
{
    public class Message
    {
        public Message(Stream bodyStream,
                       string sessionId = "",
                       string label = "",
                       string id = "",
                       IDictionary<string, object> properties = null,
                       Action complete = null)
        {
            BodyStream = bodyStream;
            Id = id;
            SessionId = sessionId;
            Label = label;
            Properties = properties;
            Complete = complete;
        }

        public string Id { get; }
        public Stream BodyStream { get; }
        public string Label { get; }
        public string SessionId { get; }
        public IDictionary<string, object> Properties { get; }
        public Action Complete { get; }
    }
}
