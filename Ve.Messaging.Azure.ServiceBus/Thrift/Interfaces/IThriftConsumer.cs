using System;
using System.Collections.Generic;

namespace Ve.Messaging.Azure.ServiceBus.Thrift.Interfaces
{
    [Obsolete("Prefer extension method")]
    public interface IThriftConsumer
    {
        IEnumerable<T> RetrieveMessages<T>(int messageAmount, int timeout, string exceptLabel = null) where T : new();
    }
}