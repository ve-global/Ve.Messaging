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
        protected readonly SubscriptionClient _client;

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
                var brokeredMessages = ReceiveMessages(messageAmount, tm);
                messages.AddRange(brokeredMessages);
            }

            return messages;
        }

        protected virtual IEnumerable<Message> ReceiveMessages(int messageAmount, TimeSpan tm)
        {
            return _client.ReceiveBatch(messageAmount, tm)
                .Select(x => new Message(x.GetBody<Stream>(), x.SessionId, x.Label, x.MessageId, x.Properties));
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
                    message.MessageId,
                    message.Properties);
        }

        public void Dispose()
        {
            _client.Close();
        }
    }
}