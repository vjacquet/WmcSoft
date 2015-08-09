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
using System.Globalization;
using WmcSoft.Properties;

namespace WmcSoft.Business.Accounting
{
    public sealed class CultureInfoCurrency : Currency
    {
        #region Private fields

        private readonly CultureInfo _culture;
        private readonly RegionInfo _regionInfo;

        #endregion

        #region Lifecycle

        public CultureInfoCurrency(CultureInfo culture) {
            _culture = culture;
            _regionInfo = new RegionInfo(culture.LCID);
        }

        #endregion

        #region Properties

        public override string Symbol {
            get { return _culture.NumberFormat.CurrencySymbol; }
        }

        public override string Name {
            get { return _regionInfo.CurrencyNativeName; }
        }

        public override string Definition {
            get { return String.Format(Resources.CurrencyFromRegionDescriptionFormat, _regionInfo.NativeName); }
        }

        public override string ThreeLetterISOCode {
            get { return _regionInfo.ISOCurrencySymbol; }
        }
        public override int DecimalDigits {
            get { return _culture.NumberFormat.CurrencyDecimalDigits; }
        }

        #endregion
    }
}
