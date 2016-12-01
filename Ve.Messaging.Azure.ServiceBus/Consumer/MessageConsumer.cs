using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public IEnumerable<Message> RetrieveMessages(int messageAmount, int timeout)
        {
            var stopwatch = Stopwatch.StartNew();
            var messages = new List<Message>();
            var tm = TimeSpan.FromSeconds(timeout);

            while (messages.Count < messageAmount && stopwatch.Elapsed < tm)
            {
                var brokeredMessages = _client.ReceiveBatch(messageAmount, tm);

                messages.AddRange(
                    from brokeredMessage in brokeredMessages
                    let stream = brokeredMessage.GetBody<Stream>()
                    select new Message(
                        stream,
                        brokeredMessage.SessionId,
                        brokeredMessage.Label,
                        brokeredMessage.Properties)
                );
            }

            return messages;
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