/*
 * Les coulisses du CLR: 9 structures et algorithmes de données parallèles réutilisables
 * 
 * http://msdn.microsoft.com/msdnmag/issues/07/05/CLRInsideOut/default.aspx?loc=fr
 * 
 */

// 2009-02-26 VJA
//  - renamed fields using this library conventions;

using System;
using System.Threading;

namespace WmcSoft.Threading
{
    public struct SpinWait
    {
        private static readonly bool IsSingleProcessor = (Environment.ProcessorCount == 1);
        private const int YieldFrequency = 4000;
        private const int YieldOneFrequency = 3 * YieldFrequency;
        private int _count;

        public int Spin()
        {
            int oldCount = _count;

            // On a single-CPU machine, we ensure our counter is always
            // a multiple of ‘s_yieldFrequency’, so we yield every time.
            // Else, we just increment by one.
            _count += (IsSingleProcessor ? YieldFrequency : 1);

            // If not a multiple of ‘s_yieldFrequency’ spin (w/ backoff).
            int countModFrequency = _count % YieldFrequency;
            if (countModFrequency > 0)
                Thread.SpinWait((int)(1 + (countModFrequency * 0.05f)));
            else
                Thread.Sleep(_count <= YieldOneFrequency ? 0 : 1);

            return oldCount;
        }

        private void Yield()
        {
            Thread.Sleep(_count < YieldOneFrequency ? 0 : 1);
        }
    }

}
