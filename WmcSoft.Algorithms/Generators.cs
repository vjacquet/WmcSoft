#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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



namespace WmcSoft.Algorithms
{
    public static class Generators
    {
        #region Iota

        /// <summary>
        /// Returns an array of <paramref name="n"/> integerss increasing from zero.
        /// </summary>
        /// <param name="n">The number of integers in the array.</param>
        /// <returns>The array of n integers.</returns>
        public static int[] Iota(int n)
        {
            var array = new int[n];
            // array[0] = 0;
            for (var i = 1; i < n; i++) {
                array[i] = i;
            }
            return array;
        }

        #endregion
    }
}
