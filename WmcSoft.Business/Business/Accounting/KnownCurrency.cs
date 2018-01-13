#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

namespace WmcSoft.Business.Accounting
{
    class KnownCurrency : Currency
    {
        readonly string _isoCode;
        readonly string _name;
        readonly string _symbol;
        readonly int _decimalDigits;
        readonly int _numericCode;
        readonly RoundingMode _rounding;
        readonly string _format;

        public override int DecimalDigits {
            get { return _decimalDigits; }
        }

        public override string Definition {
            get { return _name; }
        }

        //static Data d { "South-African rand", "ZAR", 710, "R", "", 100, Rounding(), "%3% %1$.2f" };

        public override string Name {
            get { return _name; }
        }

        public override int NumericCode {
            get { return _numericCode; }
        }

        public override RoundingMode Rounding {
            get { return _rounding; }
        }

        public override string Symbol {
            get { return _symbol; }
        }

        public override string ThreeLetterISOCode {
            get { return _isoCode; }
        }
    }
}
