using System;
using System.Collections.Generic;
using System.IO;

namespace Ve.Messaging.Model
{
    public class Message
    {
        private readonly Action _complete;

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
            _complete = complete;
        }

        public string Id { get; }
        public Stream BodyStream { get; }
        public string Label { get; }
        public string SessionId { get; }
        public IDictionary<string, object> Properties { get; }

        public Action Complete
        {
            get { return _complete ?? (() => { }); }
        }
    }
}
