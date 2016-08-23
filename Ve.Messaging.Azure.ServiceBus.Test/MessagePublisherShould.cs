using Microsoft.ServiceBus.Messaging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ve.Messaging.Azure.ServiceBus.Publisher;
using Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces;
using Ve.Messaging.Azure.ServiceBus.Publisher.Wrapper;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Test
{
    public class MessagePublisherShould
    {
        [Test]
        public async Task Should_send_messages_in_batch()
        {
            var resolverMock = new Mock<IPublisherClientResolver>();
            var wrapperMock = new Mock<ITopicClientWrapper>();
            resolverMock.Setup((_) => _.GetClient()).Returns(wrapperMock.Object);
            var publisher = new MessagePublisher(resolverMock.Object);

            await publisher.SendBatchAsync(new List<Message> { CreateEmptyMessage(), CreateEmptyMessage() });

            wrapperMock.Verify((_) => _.SendAsync(It.IsAny<BrokeredMessage>()), Times.Exactly(2));
        }

        private Message CreateEmptyMessage()
        {
            return new Message(new Mock<Stream>().Object);
        }
    }
}