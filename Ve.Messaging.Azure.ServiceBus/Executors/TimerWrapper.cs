using System.Timers;

namespace Ve.Messaging.Azure.ServiceBus.Executors
{
    internal sealed class TimerWrapper : ITimer
    {
        private readonly Timer _timer;

        public int Interval
        {
            get
            {
                return (int)_timer.Interval;
            }
            set
            {
                _timer.Interval = (double)value;
            }
        }

        public event ElapsedEventHandler Elapsed
        {
            add
            {
                _timer.Elapsed += value;
            }
            remove
            {
                _timer.Elapsed -= value;
            }
        }

        public TimerWrapper(int interval = 10000)
        {
            _timer = new Timer((double)interval)
            {
                AutoReset = true,
                Enabled = true
            };
        }

        public void Start()
        {
            _timer.Start();
        }
    }
}
