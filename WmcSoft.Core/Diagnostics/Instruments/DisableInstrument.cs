﻿#region Licence

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

namespace WmcSoft.Diagnostics.Instruments
{
    /// <summary>
    /// Decorates an instrument to support enable and disable state.
    /// </summary>
    public sealed class DisableInstrument : InstrumentBase, IObserver<decimal>
    {
        private readonly IInstrument underlying;
        private IDisposable subscription;

        public DisableInstrument(IInstrument instrument, bool enabled = false) : base(instrument.Name)
        {
            Enabled = enabled;
            underlying = instrument;
            subscription = instrument.Subscribe(this);
        }

        public bool Enabled { get; set; }

        #region IObserver<SentryStatus> members

        void IObserver<decimal>.OnCompleted()
        {
            OnCompleted();
            subscription = null;
        }

        void IObserver<decimal>.OnError(Exception error)
        {
            OnError(error);
            subscription = null;
        }

        void IObserver<decimal>.OnNext(decimal value)
        {
            if (Enabled)
                OnNext(value);
        }

        #endregion
    }
}
