using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Ve.Messaging.Consumer;

namespace Ve.Messaging.Azure.ServiceBus.Consumer
{
    public class ConsumerFactory
    {
        public IMessageConsumer GetCosumer(string conectionString, string topicPath,string subscriptionName,TimeSpan? timeToExpire = null, string sqlFilter = null)
        {
            if(!timeToExpire.HasValue)
            {
                timeToExpire = TimeSpan.MaxValue;
            }
            var namespaceManager = NamespaceManager.CreateFromConnectionString(conectionString);
            var description = GetSubscriptionDescription(topicPath, subscriptionName, timeToExpire.Value);
            
            var client = GetSubscriptionClient(topicPath, subscriptionName, namespaceManager, description);
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

        private static void CreateSubscriptionIfNotExists(NamespaceManager namespaceManager, SubscriptionDescription description,
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
                                                                   TimeSpan timeToExpire)
        {
            return new SubscriptionDescription(topicName, subscriptionName)
            {
                EnableDeadLetteringOnMessageExpiration = false,
                EnableDeadLetteringOnFilterEvaluationExceptions = false,
                DefaultMessageTimeToLive = timeToExpire,
            };
        }
    }
}
