using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Ve.Messaging.Azure.ServiceBus.Publisher.Interfaces;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Messaging.Azure.ServiceBus.Publisher.Wrapper;
using Ve.Messaging.Model;

namespace Ve.Messaging.Azure.ServiceBus.Publisher
{
    public class FailoverPublisherClientResolver : IPublisherClientResolver
    {
        private const string FAILOVER_METRIC = "dependencies.servicebus.failover";
        private readonly ITopicClientWrapper _primaryClient;
        private readonly ITopicClientWrapper _failoverClient;
        private readonly IVeStatsDClient _statsDClient;

        public FailoverPublisherClientResolver(
            ITopicClientWrapper primaryClient,
            ITopicClientWrapper failoverClient)
        {
            _primaryClient = primaryClient;
            _failoverClient = failoverClient;
        }

        public FailoverPublisherClientResolver(
            ITopicClientWrapper primaryClient,
            ITopicClientWrapper failoverClient,
            IVeStatsDClient statsDClient)
        {
            _primaryClient = primaryClient;
            _failoverClient = failoverClient;
            _statsDClient = statsDClient;
        }

        public ITopicClientWrapper GetClient()
        {
            if (_primaryClient.IsHealthy())
            {
                return _primaryClient;
            }
            _statsDClient?.LogCount(FAILOVER_METRIC);
            TryToExitFailover();
            return _failoverClient;
        }

        private void TryToExitFailover()
        {
            Task.Run(() =>
            {
                var t = _primaryClient.SendAsync(new BrokeredMessage());
                t.Wait();
            });
        }

        public void ReportFailure(ITopicClientWrapper wrapper, Message message, Exception ex = null)
        {

        }

    }
}