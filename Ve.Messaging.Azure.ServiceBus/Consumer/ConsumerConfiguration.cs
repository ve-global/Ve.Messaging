using System;

namespace Ve.Messaging.Azure.ServiceBus.Consumer
{
    public class ConsumerConfiguration
    {
        public ConsumerConfiguration(string conectionString, string topicPath, string subscriptionName, TimeSpan? timeToExpire = null, string sqlFilter = null)
        {
            ConectionString = conectionString;
            TopicPath = topicPath;
            SubscriptionName = subscriptionName;
            TimeToExpire = timeToExpire;
            SqlFilter = sqlFilter;
        }

        public string ConectionString { get; }

        public string TopicPath { get; }

        public string SubscriptionName { get; }

        public TimeSpan? TimeToExpire { get; }

        public string SqlFilter { get; }
    }
}
