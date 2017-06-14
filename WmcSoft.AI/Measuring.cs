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

namespace WmcSoft.AI
{
    public class Measuring
    {
        private readonly List<double> _targets;
        private readonly double _mean;
        private readonly double _squaredMean;

        public Measuring(IEnumerable<double> targetValues)
        {
            _targets = new List<double>(targetValues);
            var n = _targets.Count;
            if (n == 0) throw new ArgumentException(nameof(targetValues));

            for (int i = 0; i < n; i++) {
                var t = _targets[i];
                _mean += t;
                _squaredMean += t * t;
            }
            _mean /= n;
            _squaredMean /= n;
        }

        public Measures Measure(IEnumerable<double> outputs)
        {
            return new Measures(_targets, _mean, _squaredMean, outputs);
        }
    }
}
