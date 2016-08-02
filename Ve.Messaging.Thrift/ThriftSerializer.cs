using System.IO;
using Thrift.Protocol;
using Thrift.Transport;

namespace Ve.Messaging.Thrift
{
    public static class ThriftSerializer 
    {
        public static Stream Serialize<T>(object value)
        {
            using (TMemoryBuffer trans = new TMemoryBuffer())
            {
                TProtocol proto = new TCompactProtocol(trans);
                TBase trade = (TBase)value;
                trade.Write(proto);
                byte[] bytes = trans.GetBuffer();
                var memoryStream = new MemoryStream(bytes);
                return memoryStream;
            }
        }
        public static byte[] SerializeGetBytes<T>(object value)
        {
            using (TMemoryBuffer trans = new TMemoryBuffer())
            {
                TProtocol proto = new TCompactProtocol(trans);
                TBase trade = (TBase)value;
                trade.Write(proto);
                byte[] bytes = trans.GetBuffer();
                return bytes;
            }
        }

        public static T Deserialize<T>(Stream stream) where T : new()
        {
            byte[] buffer;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                buffer = memoryStream.ToArray();
            }
            using (TMemoryBuffer trans = new TMemoryBuffer(buffer))
            {
                TProtocol proto = new TCompactProtocol(trans);
                TBase deserialized = (TBase)new T();
                deserialized.Read(proto);
                return (T)deserialized;
            }
        }

        public static T Deserialize<T>(byte[] buffer) where T : new()
        {
            using (TMemoryBuffer trans = new TMemoryBuffer(buffer))
            {
                TProtocol proto = new TCompactProtocol(trans);
                TBase deserialized = (TBase)new T();
                deserialized.Read(proto);
                return (T)deserialized;
            }
        }
    }
}
