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
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Units
{
    /// <summary>
    /// Represents a recipocal unit conversion.
    /// </summary>
    /// <remarks>Use UnitConverter.Reciprocal to create a reciprocal conversion.</remarks>
    class ReciprocalUnitConversion : UnitConversion
    {
        readonly UnitConversion _base;

        internal ReciprocalUnitConversion(UnitConversion conversion)
            : base(conversion.Target, conversion.Source) {
            _base = conversion;
        }

        public UnitConversion BaseUnitConversion {
            get { return _base; }
        }

        public override decimal Convert(decimal value) {
            return _base.ConvertBack(value);
        }

        public override decimal ConvertBack(decimal value) {
            return _base.Convert(value);
        }
    }
}
