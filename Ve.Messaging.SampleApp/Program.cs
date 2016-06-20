using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Ve.Metrics.StatsDClient;
using Ve.Messaging.Azure.ServiceBus.Consumer;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Azure.ServiceBus.Publisher;
using Ve.Messaging.Azure.ServiceBus.Thrift;
using Ve.Messaging.Serializer;
using Ve.Messaging.Samples;

namespace Ve.Messaging.SampleApp
{
    public class Program
    {
        static string YOUR_PRIMARY_CONNECTION_STRING = "ServiceBus.ConnectionString.Primary";
        static string YOUR_FAILOVER_CONNECTION_STRING = "ServiceBus.ConnectionString.Failover";
        
        static void Main(string[] args)
        {
            var statsdConfig = InstantiateStatsdConfig();
            var client  = new VeStatsDClient(statsdConfig);

            var publisherFactory = GetPublisher(client);
            var sender = GetSender(publisherFactory);

            var consumer = GetConsumer();
            

            SendMultipleMessages(sender);
            Task.Run(() =>
            {
                while (true)
                {
                    var messages = consumer.RetrieveMessages<HouseDto>(int.MaxValue, 100);

                    foreach (var message in messages)
                    {
                        Console.WriteLine($"House: {message.Content.Name} of {message.Content.Owner}");
                    }
                }
            });
            Console.ReadLine();
        }

        private static ThriftConsumer GetConsumer()
        {
            string primaryConnectionString = ConfigurationManager.AppSettings[YOUR_PRIMARY_CONNECTION_STRING];
            var factory = new ConsumerFactory();
            var consumer = factory.GetConsumer(new ConsumerConfiguration(primaryConnectionString, "testtopic3", "", TimeSpan.MaxValue, "testsubsccription"));
            return new ThriftConsumer(consumer);

        }

        private static void SendMultipleMessages(ThriftPublisher sender)
        {
            for (int i = 0; i < 10; i++)
            {
                var house = new HouseDto()
                {
                    Id = i,
                    Name = i.ToString(),
                    Owner = "Mr. " + (char)(i + 65)
                };
                var serializer = new BinaryFormatter();
                var stream = new MemoryStream();
                serializer.Serialize(stream,house);
                sender.SendAsync(
                    new ThriftMessage<HouseDto>(house)
                    { Label = "HouseTest" + i, SessionId = "House Session"})
                    .Wait();
            }
        }

        private static PublisherFactory GetPublisher(VeStatsDClient client)
        {
            var publisherFactory = new PublisherFactory(
                client,
                new TopicClientCreator(new TopicCreator())
                );
            return publisherFactory;
        }

        private static ThriftPublisher GetSender(PublisherFactory publisherFactory)
        {
            string primaryConnectionString = ConfigurationManager.AppSettings[YOUR_PRIMARY_CONNECTION_STRING];
            var sender = publisherFactory.CreatePublisher(new ServiceBusPublisherConfiguration()
            {
                PrimaryConfiguration = new TopicConfiguration()
                {
                    ConnectionString = primaryConnectionString,
                    TopicName = "testtopic3",
                },
                ServiceBusPublisherStrategy = ServiceBusPublisherStrategy.Simple
            });
            var publisher = new ThriftPublisher(sender);
            return publisher;
        }

        private static StatsdConfig InstantiateStatsdConfig()
        {
            FakeConfigurationManager();
            var statsdConfig = new StatsdConfig()
            {
                AppName = "testapp",
                Datacenter = ConfigurationManager.AppSettings["statsd.datacenter"],
                Host = ConfigurationManager.AppSettings["statsd.host"]
            };
            return statsdConfig;
        }

        private static void FakeConfigurationManager()
        {
            ConfigurationManager.AppSettings["statsd.datacenter"] = "A";
            ConfigurationManager.AppSettings["statsd.host"] = "A";
        }
    }
}
