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
using System.Diagnostics;
using System.Globalization;
using WmcSoft.Units;

namespace WmcSoft.Business.Accounting
{
    /// <summary>
    /// Represents a <see cref="Metric"/> or standard value for measuring <see cref="Money"/>.
    /// </summary>
    [DebuggerDisplay("{ThreeLetterISOCode,nq}")]
    public abstract class Currency : Metric, ITemporal
    {
        #region Private fields

        private readonly List<CultureInfo> _acceptedIn;

        #endregion

        #region Lifecycle

        public Currency() {
            _acceptedIn = new List<CultureInfo>();
        }

        #endregion

        #region Properties

        IList<CultureInfo> AcceptedIn {
            get { return _acceptedIn; }
        }

        public abstract string ThreeLetterISOCode { get; }

        public abstract int DecimalDigits { get; }

        #endregion

        #region ITemporal Members

        public DateTime? ValidSince { get; set; }

        public DateTime? ValidUntil { get; set; }

        #endregion
    }
}
