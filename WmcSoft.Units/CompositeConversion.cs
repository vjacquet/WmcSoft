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

namespace WmcSoft.Units
{
    class CompositeConversion : UnitConversion
    {
        private readonly List<UnitConversion> _sequence;

        internal CompositeConversion(UnitConversion x, UnitConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(2);
            _sequence.Add(x);
            _sequence.Add(y);
        }

        internal CompositeConversion(UnitConversion x, CompositeConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(y._sequence.Count + 1);
            _sequence.Add(x);
            _sequence.AddRange(y._sequence);
        }

        internal CompositeConversion(CompositeConversion x, UnitConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(x._sequence.Count + 1);
            _sequence.AddRange(x._sequence);
            _sequence.Add(y);
        }

        internal CompositeConversion(CompositeConversion x, CompositeConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(x._sequence.Count + y._sequence.Count);
            _sequence.AddRange(x._sequence);
            _sequence.AddRange(y._sequence);
        }

        internal CompositeConversion(params UnitConversion[] conversions)
            : base(conversions.First().Source, conversions.Last().Target) {
            // ensures the conversions forms a path.
            for (int i = 1; i < conversions.Length; i++) {
                if (conversions[i - 1].Target != conversions[i].Source)
                    throw new ArgumentException("conversions");
            }
            _sequence = new List<UnitConversion>(conversions);
        }

        public override decimal Convert(decimal value) {
            for (int i = 0; i < _sequence.Count; i++) {
                value = _sequence[i].Convert(value);
            }
            return value;
        }

        public override decimal ConvertBack(decimal value) {
            for (int i = _sequence.Count - 1; i >= 0; i--) {
                value = _sequence[i].ConvertBack(value);
            }
            return value;
        }
    }
}
