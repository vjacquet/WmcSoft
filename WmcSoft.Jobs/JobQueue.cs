#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

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

        private Queue<IJob> _queue = new Queue<IJob>();

        #endregion

        #region IJobQueue Membres

        public void Enqueue(IJob item) {
            lock (_queue) {
                _queue.Enqueue(item);
                Monitor.Pulse(_queue);
                //Monitor.Wait(m_queue);
            }
        }

        public IJob Dequeue() {
            IJob item;
            lock (_queue) {
                while (_queue.Count == 0)
                    Monitor.Wait(_queue);
                item = _queue.Dequeue();
                Monitor.Pulse(_queue);
            }
            return item;
        }

        public bool TryDequeue(out IJob item, TimeSpan timeout) {
            lock (_queue) {
                if (_queue.Count == 0) {
                    if (!Monitor.Wait(_queue, timeout) || _queue.Count == 0) {
                        item = null;
                        return false;
                    }
                }
                item = _queue.Dequeue();
                Monitor.Pulse(_queue);
            }
            return true;
        }

        public bool IsEmpty {
            get {
                return _queue.Count == 0;
            }
        }

        public void Clear(Action<IJob> action) {
            lock (_queue) {
                while (_queue.Count > 0) {
                    IJob job = _queue.Dequeue();
                    action(job);
                }

                Monitor.PulseAll(_queue);
            }
        }

        #endregion
    }

}
