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
    public sealed class CountdownLatch : IDisposable
    {
        private readonly EventWaitHandle _event;
        private int _remain;

        public CountdownLatch(int count)
        {
            _remain = count;
            _event = new ManualResetEvent(false);
        }

        public void Signal()
        {
            // The last thread to signal also sets the event.
            if (Interlocked.Decrement(ref _remain) == 0)
                _event.Set();
        }

        public void Wait()
        {
            _event.WaitOne();
        }

        #region IDisposable Membres

        ~CountdownLatch()
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
            _event.Close();
        }

        #endregion
    }
}
