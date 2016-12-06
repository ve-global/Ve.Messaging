using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using Ve.Messaging.Model;
using Ve.Messaging.Samples;
using Ve.Messaging.Thrift;

namespace Ve.Messaging.Tests
{
    [TestFixture]
    public class ThriftMessageShould
    {
        [Test]
        public void Set_Content()
        {
            var houseDto = GetHouseDto();
            var thriftMessage = new ThriftMessage<HouseDto>(houseDto);

            thriftMessage.Content.ShouldNotBe(null);
        }

        [Test]
        public void Set_Content_And_Properties()
        {
            var properties = new Dictionary<string, object>()
            {
                { "whatever", 1 }
            };
            string sessionId = Guid.NewGuid().ToString();
            string label = Guid.NewGuid().ToString();
            string messageId = Guid.NewGuid().ToString();
            var houseDto = GetHouseDto();
            var thriftMessage = new ThriftMessage<HouseDto>(houseDto, sessionId, label, messageId, properties);

            thriftMessage.SessionId.ShouldBe(sessionId);
            thriftMessage.Label.ShouldBe(label);
            thriftMessage.Id.ShouldBe(messageId);
            AssertProperties(thriftMessage, properties);
        }

        [Test]
        public void Set_Message()
        {
            var houseDto = GetHouseDto();
            var bodyStream = ThriftSerializer.Serialize(houseDto);
            var message = new Message(bodyStream);

            var thriftMessage = new ThriftMessage<HouseDto>(message);

            thriftMessage.BodyStream.ShouldNotBe(null);
        }

        [Test]
        public void Set_Message_And_Properties()
        {
            var houseDto = GetHouseDto();
            var properties = new Dictionary<string, object>()
            {
                { "whatever", 1 }
            };
            string sessionId = Guid.NewGuid().ToString();
            string label = Guid.NewGuid().ToString();
            string messageId = Guid.NewGuid().ToString();
            var bodyStream = ThriftSerializer.Serialize(houseDto);
            var message = new Message(bodyStream, sessionId, label, messageId, properties);

            var thriftMessage = new ThriftMessage<HouseDto>(message);

            thriftMessage.Label.ShouldBe(label);
            thriftMessage.SessionId.ShouldBe(sessionId);
            thriftMessage.Id.ShouldBe(messageId);
            AssertProperties(thriftMessage, properties);
        }


        private static void AssertProperties(ThriftMessage<HouseDto> thriftMessage,
                                             Dictionary<string, object> properties)
        {
            thriftMessage.Properties.Count.ShouldBe(properties.Count);
            foreach (var property in thriftMessage.Properties)
            {
                var key = property.Key;
                properties.ContainsKey(key).ShouldBe(true);
                var value = property.Value;
                value.ShouldBe(properties[key]);
            }
        }

        private static HouseDto GetHouseDto()
        {
            var houseDto = new HouseDto()
            {
                Name = "Baker Row",
                Id = new Random().Next(),
                Owner = Guid.NewGuid().ToString()
            };
            return houseDto;
        }
    }
}
