using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WmcSoft.Threading
{
    public class JobQueue : IJobQueue
    {
        #region Private fields

        private Queue<IJob> m_queue = new Queue<IJob>();

        #endregion

        #region IJobQueue Membres

        public void Enqueue(IJob item) {
            lock (m_queue) {
                m_queue.Enqueue(item);
                Monitor.Pulse(m_queue);
                //Monitor.Wait(m_queue);
            }
        }

        public IJob Dequeue() {
            IJob item;
            lock (m_queue) {
                while (m_queue.Count == 0)
                    Monitor.Wait(m_queue);
                item = m_queue.Dequeue();
                Monitor.Pulse(m_queue);
            }
            return item;
        }

        public bool TryDequeue(out IJob item, TimeSpan timeout) {
            lock (m_queue) {
                if (m_queue.Count == 0) {
                    if (!Monitor.Wait(m_queue, timeout) || m_queue.Count == 0) {
                        item = null;
                        return false;
                    }
                }
                item = m_queue.Dequeue();
                Monitor.Pulse(m_queue);
            }
            return true;
        }

        public bool IsEmpty {
            get {
                return m_queue.Count == 0;
            }
        }

        public void Clear(Action<IJob> action) {
            lock (m_queue) {
                while (m_queue.Count > 0) {
                    IJob job = m_queue.Dequeue();
                    action(job);
                }

                Monitor.PulseAll(m_queue);
            }
        }

        #endregion
    }

}
