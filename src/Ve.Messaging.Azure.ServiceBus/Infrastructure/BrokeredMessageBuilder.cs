using Microsoft.ServiceBus.Messaging;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Infrastructure
{
    public static class BrokeredMessageBuilder
    {
        public static BrokeredMessage SerializeToBrokeredMessage(Message message)
        {
            var bodyStream = ThriftSerializer.Serialize<Message>(message.Content);
            var brokeredMessage = new BrokeredMessage(bodyStream);
            brokeredMessage.SessionId = message.SessionId;
            brokeredMessage.Label = message.Label;
            
            AddProperties(message, brokeredMessage);
            return brokeredMessage;
        }

        private static void AddProperties(Message message, BrokeredMessage brokeredMessage)
        {
            if (message.Properties != null && message.Properties.Count > 0)
            {
                foreach (var item in message.Properties)
                {
                    brokeredMessage.Properties.Add(item.Key, item.Value);
                }
            }
        }
    }
}
