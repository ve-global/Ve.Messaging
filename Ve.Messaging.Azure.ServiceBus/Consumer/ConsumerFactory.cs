using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;
using Ve.Messaging.Consumer;

namespace Ve.Messaging.Azure.ServiceBus.Consumer
{
    public class ConsumerFactory
    {
        public IMessageConsumer GetConsumer(ConsumerConfiguration consumerConfiguration)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(consumerConfiguration.ConectionString);
            var description = GetSubscriptionDescription(consumerConfiguration.TopicPath, consumerConfiguration.SubscriptionName, consumerConfiguration.TimeToExpire);
            
            var client = GetSubscriptionClient(consumerConfiguration.TopicPath, consumerConfiguration.SubscriptionName, namespaceManager, description, consumerConfiguration.SqlFilter);
            var result = new MessageConsumer(client);
            return result;
        }

        private static SubscriptionClient GetSubscriptionClient(string topicName,
                                                                string subscriptionName,
                                                                NamespaceManager namespaceManager,
                                                                SubscriptionDescription description,
                                                                string sqlFilter = null)
        {
            if (namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                return GetSubscriptionClient(topicName, subscriptionName, namespaceManager);
            }

            CreateSubscriptionIfNotExists(namespaceManager, description, sqlFilter);

            return GetSubscriptionClient(topicName, subscriptionName, namespaceManager);
        }

        private static void CreateSubscriptionIfNotExists(NamespaceManager namespaceManager,
                                                          SubscriptionDescription description,
                                                          string sqlFilter)
        {
            if (string.IsNullOrWhiteSpace(sqlFilter))
            {
                namespaceManager.CreateSubscription(description);
            }
            else
            {
                namespaceManager.CreateSubscription(description, new SqlFilter(sqlFilter));
            }
        }

        private static SubscriptionClient GetSubscriptionClient(string topicName, string subscriptionName,
            NamespaceManager namespaceManager)
        {
            var mfs = new MessagingFactorySettings
            {
                TokenProvider = namespaceManager.Settings.TokenProvider
            };
            MessagingFactory messagingFactory = MessagingFactory.Create(namespaceManager.Address, mfs);
            return messagingFactory.CreateSubscriptionClient(topicName,
                subscriptionName,
                ReceiveMode.ReceiveAndDelete);
        }


        private static SubscriptionDescription GetSubscriptionDescription(string topicName,
                                                                          string subscriptionName,
                                                                          TimeSpan? timeToExpire)
        {
            return new SubscriptionDescription(topicName, subscriptionName)
            {
                EnableDeadLetteringOnMessageExpiration = false,
                EnableDeadLetteringOnFilterEvaluationExceptions = false,
                DefaultMessageTimeToLive = timeToExpire ?? TimeSpan.FromDays(4)
            };
        }
    }
}
