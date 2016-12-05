using System;
using Thrift.Protocol;

namespace Ve.Messaging.Azure.ServiceBus.Thrift
{
    public class TransientMessage<T> where T : TBase
    {
        public T Message { get; }
        public Action Complete { get; }

        public TransientMessage(T message, Action complete)
        {
            Message = message;
            Complete = complete;
        }
    }
}