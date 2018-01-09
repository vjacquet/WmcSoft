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

namespace WmcSoft
{
    /// <summary>
    /// Implementation using 4 samples to obtain a sample with a better bell-shape deviate.
    /// </summary>
    public class BetterBellShapeRandom : RandomBase
    {
        private static readonly double Coeff = Math.Sqrt(12d) / 2d;

        protected override double Sample()
        {
            var r = base.Sample() + base.Sample() - base.Sample() - base.Sample();
            return Coeff * r;
        }
    }
}
