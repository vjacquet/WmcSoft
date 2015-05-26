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
    /// Represents an amount of work corresponding to one person
    /// working for one hour.
    /// </summary>
    public sealed class ManHour : Unit
    {
        public ManHour() {
        }

        public override SystemOfUnits SystemOfUnits {
            get {
                // TODO: why returning null ? Is it because it is strange to create a unit system for a single unit?
                //       Shouldn't we just also add ManMonth, as it is also an average value to plan projects?
                return null;
            }
        }

        public override string Definition {
            get { return RM.GetDefinition("ManHour"); }
        }

        public override string Name {
            get { return RM.GetDefinition("Name"); }
        }

        public override string Symbol {
            get { return null; }
        }
    }
}
