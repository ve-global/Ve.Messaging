using System.Timers;

namespace Ve.Messaging.Azure.ServiceBus.Executors
{
    internal interface ITimer
    {
        int Interval { get; set; }
        event ElapsedEventHandler Elapsed;
        void Start();
    }
}
