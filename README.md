# Ve.Messaging
One abstraction layer for messaging transport to apply the better practices on azure

# How to use it

The Ve Messaging it is fundamentally using the consumer publisher model, so what you are going to have it is an `IMessageConsumer` and an `IMessagePublisher`.

`IMessageConsumer`: to get data from your message stream.

`IMessagePublisher`: to insert new messages on your message stream

This basic infrastructure and abstraction will be present on the base package ```Install-Package Ve.Messaging```

To install the actual implementation you will need to install one of our providers for this message stream.

For example ```Install-Package Ve.Messaging.Azure.ServiceBus```

#Using Azure Service bus

TO install the package you should use: ```Install-Package Ve.Messaging.Azure.ServiceBus```

Part of the prerequisite for this provider we assume that you have a valid connection string to servicebus able to manage and send messages in the case od the publishers and manage and receive in the case of the readers

## Creating the publisher for Azure ServiceBus

To create the publisher for azure service bus the code required it is

```csharp
var statsdConfig = InstantiateStatsdConfig();
var client  = new VeStatsDClient(statsdConfig);
var publisherFactory = new PublisherFactory(
                client,
                new TopicClientCreator(new TopicCreator())
                );

var sender = publisherFactory.CreatePublisher(new ServiceBusPublisherConfiguration()
            {
                PrimaryConfiguration = new TopicConfiguration()
                {
                    ConnectionString = "YOUR CONNECTION STRING",
                    TopicName = "YOUR TOPIC NAME",
                },
                ServiceBusPublisherStrategy = ServiceBusPublisherStrategy.Simple
            });
```


## Creating the consumer for Azure ServiceBus

To create the consumer for azure service bus you need to use a code similar to:

```csharp
var factory = new ConsumerFactory();
var consumer = factory.GetCosumer(
    "YOUR CONNECTION STRING",
    "YOUR TOPIC NAME",
    "YOUR SUBSCRIPTION NAME")
```