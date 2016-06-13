namespace Ve.Messaging.Azure.ServiceBus.Infrastructure
{
    public interface IFailoverResolver
    {
        bool IsInFailover { get; }
        void ReportException();
        void ExitFailover();
    }
}