using System.Timers;
using Ve.Messaging.Azure.ServiceBus.Infrastructure;

namespace Ve.Messaging.Azure.ServiceBus.Executors
{
    public class FailoverExecutor
    {
        private static ITimer _timer;

        public int Interval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                _timer.Interval = value;
            }
        }

        public FailoverExecutor(IFailoverResolver client)
        {
            Interval = 10000;
        }

        internal FailoverExecutor(ITimer timer)
        {
            _timer = timer;
            _timer.Elapsed += new ElapsedEventHandler(this.Invoke);
            _timer.Start();
        }

        private void Invoke(object sender, ElapsedEventArgs e)
        {

        }
    }
}
