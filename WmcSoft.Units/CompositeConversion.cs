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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Units
{
    class CompositeConversion : UnitConversion, IEnumerable<UnitConversion>
    {
        private readonly UnitConversion[] _sequence;

        internal CompositeConversion(UnitConversion x, UnitConversion y)
            : base(x.Source, y.Target)
        {
            Debug.Assert(CanMakePath(x, y));

            _sequence = new UnitConversion[] { x, y };
        }

        internal CompositeConversion(UnitConversion x, CompositeConversion y)
            : base(x.Source, y.Target)
        {
            Debug.Assert(CanMakePath(x, y));

            _sequence = new UnitConversion[y._sequence.Length + 1];
            _sequence[0] = x;
            y._sequence.CopyTo(_sequence, 1);
        }

        internal CompositeConversion(CompositeConversion x, UnitConversion y)
            : base(x.Source, y.Target)
        {
            Debug.Assert(CanMakePath(x, y));

            _sequence = new UnitConversion[x._sequence.Length + 1];
            x._sequence.CopyTo(_sequence, 0);
            _sequence[x._sequence.Length] = y;
        }

        internal CompositeConversion(CompositeConversion x, CompositeConversion y)
            : base(x.Source, y.Target)
        {
            Debug.Assert(CanMakePath(x, y));

            _sequence = new UnitConversion[x._sequence.Length + y._sequence.Length];
            x._sequence.CopyTo(_sequence, 0);
            y._sequence.CopyTo(_sequence, x._sequence.Length);
        }


        internal CompositeConversion(params UnitConversion[] conversions)
            : base(conversions.First().Source, conversions.Last().Target)
        {
            Debug.Assert(CanMakePath(conversions));

            _sequence = new UnitConversion[conversions.Length];
            conversions.CopyTo(_sequence, 0);
        }

        public override decimal Convert(decimal value)
        {
            for (int i = 0; i < _sequence.Length; i++) {
                value = _sequence[i].Convert(value);
            }
            return value;
        }

        public override decimal ConvertBack(decimal value)
        {
            for (int i = _sequence.Length - 1; i >= 0; i--) {
                value = _sequence[i].ConvertBack(value);
            }
            return value;
        }

        public IEnumerator<UnitConversion> GetEnumerator()
        {
            return ((IEnumerable<UnitConversion>)_sequence).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<UnitConversion>)_sequence).GetEnumerator();
        }
    }
}
