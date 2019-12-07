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

using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Provides a set of static methods to extend the stack class.
    /// </summary>
    public static class StackExtensions
    {
        public static bool TryPeek<T>(this Stack<T> stack, out T result)
        {
            if (stack.Count > 0) {
                result = stack.Peek();
                return true;
            }

            result = default;
            return false;
        }

        public static bool TryPop<T>(this Stack<T> stack, out T result)
        {
            if (stack.Count > 0) {
                result = stack.Pop();
                return true;
            }

            result = default;
            return false;
        }
    }
}
