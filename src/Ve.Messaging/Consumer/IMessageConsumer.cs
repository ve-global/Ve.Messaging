using System.Collections.Generic;
using Ve.Messaging.Model;

namespace Ve.Messaging.Consumer
{
    public interface IMessageConsumer
    {
        IEnumerable<Message> RetrieveMessages(int messageAmount, int timeout);
    }
}