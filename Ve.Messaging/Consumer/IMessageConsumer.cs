using System;
using System.Collections.Generic;
using Ve.Messaging.Model;

namespace Ve.Messaging.Consumer
{
    public interface IMessageConsumer : IDisposable
    {
        IEnumerable<Message> RetrieveMessages(int messageAmount, int timeout, string exceptLabel = null);
        Message Peek();
    }
}