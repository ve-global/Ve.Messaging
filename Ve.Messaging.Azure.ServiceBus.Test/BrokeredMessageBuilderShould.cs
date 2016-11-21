using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Shouldly;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Test
{
    [TestFixture]
    public class BrokeredMessageBuilderShould
    {
        private string _sessionId;
        private string _label;

        [SetUp]
        public void Setup()
        {
            _sessionId = Guid.NewGuid().ToString();
            _label = Guid.NewGuid().ToString();
        }
        
        [Test]
        public void It_should_set_the_label()
        {
            var result = BrokeredMessageBuilder.SerializeToBrokeredMessage(GetMessage());

            result.Label.ShouldBe(_label);
        }
        
        [Test]
        public void It_should_set_the_sessionId()
        {
            var result = BrokeredMessageBuilder.SerializeToBrokeredMessage(GetMessage());

            result.SessionId.ShouldBe(_sessionId);
        }

        [Test]
        public void It_should_set_the_label_property()
        {
            var result = BrokeredMessageBuilder.SerializeToBrokeredMessage(GetMessage());

            result.Properties["Label"].ShouldBe(_label);
        }

        [Test]
        public void It_should_set_the_sessionId_property()
        {
            var result = BrokeredMessageBuilder.SerializeToBrokeredMessage(GetMessage());

            result.Properties["SessionId"].ShouldBe(_sessionId);
        }
        
        [Test]
        public void It_should_set_the_custom_properties()
        {
            var result = BrokeredMessageBuilder.SerializeToBrokeredMessage(new Message(new MemoryStream(), _sessionId, _label, new Dictionary<string, object>()
            {
                { "foo", "bar" }
            }));

            result.Properties["foo"].ShouldBe("bar");
        }

        private Message GetMessage()
        {
            return new Message(new MemoryStream(), _sessionId, _label);
        }
    }
}
