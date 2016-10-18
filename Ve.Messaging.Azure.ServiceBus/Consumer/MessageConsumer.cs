using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using Ve.Messaging.Consumer;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Consumer
{
    public class MessageConsumer : IMessageConsumer
    {
        private readonly SubscriptionClient _client;

        public MessageConsumer(SubscriptionClient client)
        {
            _client = client;
        }

        public IEnumerable<Message> RetrieveMessages(int messageAmount, int timeout, string exceptLabel = null)
        {
            var brokeredMessages = _client.ReceiveBatch(messageAmount, TimeSpan.FromSeconds(timeout));
            var messages = new List<Message>();

            foreach (var brokeredMessage in brokeredMessages)
            {
                if (hasExceptLabel(brokeredMessage.Label, exceptLabel)) continue;

                var stream = brokeredMessage.GetBody<Stream>();
                var message = new Message(stream,
                    brokeredMessage.SessionId,
                    brokeredMessage.Label,
                    brokeredMessage.Properties);

                messages.Add(message);
            }

            return messages;
        }

        private bool hasExceptLabel(string label, string exceptLabel)
        {
            if (string.IsNullOrEmpty(label))
            {
                return false;
            }

            if (string.IsNullOrEmpty(exceptLabel))
            {
                return false;
            }

            return label.Equals(exceptLabel);
        }

        public void Dispose()
        {
            _client.Close();
        }

        public Message Peek()
        {
            var message = _client.Peek();

            return message == null
                ? null
                : new Message(
                    message.GetBody<Stream>(),
                    message.SessionId,
                    message.Label,
                    message.Properties);
        }
    }
}