/*
 * Les coulisses du CLR: 9 structures et algorithmes de données parallèles réutilisables
 * 
 * http://msdn.microsoft.com/msdnmag/issues/07/05/CLRInsideOut/default.aspx?loc=fr
 * 
 */

// 2015-03-24 VJA
//  - renamed fields using this library conventions.

using System;
using System.Collections.Generic;
using System.Threading;

namespace WmcSoft.Threading
{
    public struct ThinEvent
    {
        private const int SpinCount = 4000;
        private EventWaitHandle _event;
        private bool _state; // false means unset, true means set.

        public void Set()
        {
            _state = true;
            Thread.MemoryBarrier(); // required.
            if (_event != null)
                _event.Set();
        }

        public void Reset()
        {
            _state = false;
            if (_event != null)
                _event.Reset();
        }

        public void Wait()
        {
            SpinWait s = new SpinWait();
            while (!_state) {
                if (s.Spin() >= SpinCount) {
                    if (_event == null) {
                        ManualResetEvent newEvent = new ManualResetEvent(_state);
                        if (Interlocked.CompareExchange<EventWaitHandle>(ref _event, newEvent, null) == null) {
                            // If someone set the flag before seeing the new
                            // event obj, we must ensure it’s been set.
                            if (_state)
                                _event.Set();
                        } else {
                            // Lost the race w/ another thread. Just use
                            // its event.
                            newEvent.Close();
                        }
                    }
                    _event.WaitOne();
                }
            }
        }
    }
}
