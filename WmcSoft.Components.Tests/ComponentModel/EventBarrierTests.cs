using System;
using System.ComponentModel;
using Xunit;

namespace WmcSoft.ComponentModel
{
    public class EventBarrierTests
    {
        [Fact]
        public void CanPreventEvent()
        {
            var bench = new EventBarrierBench();
            var a = new Listener();
            var b = new Listener();

            bench.Notified += a.Listen;
            bench.Notified += b.Listen;

            bench.Notify();
            Assert.Equal(1, a.Called);
            Assert.Equal(1, b.Called);

            using (bench.RaiseBarrier(b)) {
                bench.Notify();
            }
            Assert.Equal(2, a.Called);
            Assert.Equal(1, b.Called);

            bench.Notify();
            Assert.Equal(3, a.Called);
            Assert.Equal(2, b.Called);
        }
    }

    class Listener
    {
        public void Listen(object sender, EventArgs e)
        {
            _called++;
        }

        public int Called {
            get { return _called; }
        }
        int _called;
    }

    class EventBarrierBench : Component
    {
        public event EventHandler Notified {
            add { Events.AddHandler(NotifiedEvent, value); }
            remove { Events.RemoveHandler(NotifiedEvent, value); }
        }
        private readonly object NotifiedEvent = new object();

        protected virtual void OnNotified(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[NotifiedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        public void Notify()
        {
            OnNotified(EventArgs.Empty);
        }

        public IDisposable RaiseBarrier(object target)
        {
            return new EventBarrier(Events, target, NotifiedEvent);
        }
    }
}
