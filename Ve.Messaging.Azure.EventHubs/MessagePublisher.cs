using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Ve.Messaging.Model;
using Ve.Messaging.Publisher;

namespace Ve.Messaging.Azure.EventHubs
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly EventHubClient _client;

        public MessagePublisher(string connectionString, string eventHubName)
        {
            _client = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
        }

        public Task SendAsync(Message message)
        {
            return _client.SendAsync(new EventData(message.BodyStream));
        }

        public Task SendBatchAsync(IEnumerable<Message> messages)
        {
            return _client.SendBatchAsync(messages.Select(x => new EventData(x.BodyStream)));
        }
    }
}
