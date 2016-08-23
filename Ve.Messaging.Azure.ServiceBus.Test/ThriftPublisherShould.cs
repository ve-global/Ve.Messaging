using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ve.Messaging.Azure.ServiceBus.Thrift;
using Ve.Messaging.Model;
using Ve.Messaging.Publisher;

namespace Ve.Messaging.Azure.ServiceBus.Test
{
    public class ThriftPublisherShould
    {

        [Test]
        public async Task Should_send_messages_in_batch()
        {
            var messagePublisherMock = new Mock<IMessagePublisher>();
            var publisher = new ThriftPublisher(messagePublisherMock.Object);

            await publisher.SendBatchAsync(new List<Message> { CreateEmptyMessage(), CreateEmptyMessage() });

            messagePublisherMock.Verify((_) => _.SendAsync(It.IsAny<Message>()), Times.Exactly(2));
        }

        private Message CreateEmptyMessage()
        {
            return new Message(new Mock<Stream>().Object);
        }
    }
}
