using System.IO;

namespace Ve.Messaging.Azure.ServiceBus.Infrastructure
{
    public interface ISerializer
    {
        Stream Serialize<T>(object value);
        byte[] SerializeGetBytes<T>(object value);
        T ParseMessage<T>(Stream stream) where T : new();
        T ParseMessage<T>(byte[] buffer) where T : new();
    }
}
