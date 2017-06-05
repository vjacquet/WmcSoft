/*
 * Les coulisses du CLR: 9 structures et algorithmes de données parallèles réutilisables
 * 
 * http://msdn.microsoft.com/msdnmag/issues/07/05/CLRInsideOut/default.aspx?loc=fr
 * 
 */

// 2015-03-24 VJA
//  - renamed fields using this library conventions.
//  - added readonly where applicable.
//  - added sealed.
//  - implemented IDisposable.

using System;
using System.Threading;

namespace WmcSoft.Threading
{
    public sealed class Barrier : IDisposable
    {
        private const int SpinCount = 4000;

        private readonly int _originalCount;
        private volatile int _count;
        private readonly EventWaitHandle _oddEvent;
        private readonly EventWaitHandle _evenEvent;
        private volatile bool _sense = false; // false==even, true==odd.

        public Barrier(int count)
        {
            _originalCount = count;
            _count = count;
            _oddEvent = new ManualResetEvent(false);
            _evenEvent = new ManualResetEvent(false);
        }

#pragma warning disable 0420

        public void Await()
        {
            bool sense = _sense;

            // The last thread to signal also sets the event.
            if (_count == 1 || Interlocked.Decrement(ref _count) == 0) {
                _count = _originalCount;
                _sense = !sense; // Reverse the sense.
                if (sense == true) { // odd
                    _evenEvent.Set();
                    _oddEvent.Reset();
                } else { // even
                    _oddEvent.Set();
                    _evenEvent.Reset();
                }
            } else {
                SpinWait s = new SpinWait();
                while (sense == _sense) {
                    if (s.Spin() >= SpinCount) {
                        if (sense == true)
                            _oddEvent.WaitOne();
                        else
                            _evenEvent.WaitOne();
                    }
                }
            }
        }

#pragma warning restore 0420

        #region IDisposable Membres

        ~Barrier()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            _oddEvent.Close();
            _evenEvent.Close();
        }

        #endregion
    }
}
