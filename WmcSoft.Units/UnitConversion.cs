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

namespace WmcSoft.Units
{
    /// <summary>
    /// Converts a quantity in a source unit to a quantity in a target unit.
    /// </summary>
    public abstract class UnitConversion
    {
        #region Fields

        readonly Unit _source;
        readonly Unit _target;

        #endregion

        #region Lifecycle

        protected UnitConversion(Unit source, Unit target)
        {
            _source = source;
            _target = target;
        }

        #endregion

        #region Properties

        public Unit Source {
            get { return _source; }
        }

        public Unit Target {
            get { return _target; }
        }

        #endregion

        #region Methods

        public abstract decimal Convert(decimal value);
        public abstract decimal ConvertBack(decimal value);

        #endregion

        #region Helpers

        internal static bool CanMakePath(params UnitConversion[] conversions)
        {
            for (int i = 1; i < conversions.Length; i++) {
                if (conversions[i - 1].Target != conversions[i].Source)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
