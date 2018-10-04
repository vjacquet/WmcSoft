#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Monitoring.Instruments
{
    /// <summary>
    /// Represents an instrument observing an observable and computing using a custom function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomObservingInstrument<T> : ObservingInstrument<T>
    {
        private readonly Func<T, decimal> measure;

        static decimal Convert(T value)
        {
            return System.Convert.ToDecimal(value);
        }

        public CustomObservingInstrument(string name, IObservable<T> observable, Func<T, decimal> measure)
            : base(name, observable)
        {
            this.measure = measure ?? Convert;
        }

        protected override decimal Measure(T value)
        {
            return measure(value);
        }
    }
}
