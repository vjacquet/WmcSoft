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

namespace WmcSoft.AI
{
    public class AutoassociativeFilter
    {
        private readonly double[] _work;
        private readonly Action<double[], int, double[]> _evalute;

        public AutoassociativeFilter(Action<double[], int, double[]> evaluate, int length) {
            if (evaluate == null)
                throw new ArgumentNullException("evaluate");
            _evalute = evaluate;
            _work = new double[length];
        }

        public void Execute(double[] input, double[] output) {
            if (input == null)
                throw new ArgumentNullException("input");
            if (output == null)
                throw new ArgumentNullException("output");
            if (output.Length != input.Length)
                throw new InvalidOperationException();
            if (output.Length < _work.Length)
                throw new InvalidOperationException();

            // zero
            int i, j;
            var m = output.Length;
            for (i = 0; i < output.Length; i++) {
                output[i] = 0d;
            }

            // then cumulate the output
            var n = _work.Length;
            var count = m - n + 1;
            for (i = 0; i < count; i++) {
                _evalute(input, i, _work);
                for (j = 0; j < n; j++) {
                    output[i + j] += _work[j];
                }
            }

            // and finally get the means
            if (count > 1) {
                var lim = Math.Min(count, n);
                for (i = 0; i < m; i++) {
                    j = m - i;
                    if ((i + 1) < j)
                        j = i + 1;
                    if (j > lim)
                        j = lim;
                    output[i] /= j;
                }
            }
        }
    }
}
