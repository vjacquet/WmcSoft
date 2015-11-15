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

namespace WmcSoft.AI
{
    /// <summary>
    /// Implements the end points strategy to determine the trend's slope and intercept.
    /// </summary>
    public struct EndPoints : ITrendEvaluator
    {
        #region ITrendEvaluator Members

        public void Eval(double[] input, out double slope, out double intercept) {
            var n = input.Length - 1;
            intercept = input[0];
            slope = (input[n] - intercept) / n;
        }

        #endregion
    }
}
