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

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WmcSoft.Memory
{
    public static class NonModifyingSequence
    {
        public static int Count<T>(ReadOnlySpan<T> input, in T value, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var length = input.Length;
            var count = 0;
            for (int i = 0; i < length; i++) {
                if (comparer.Equals(input[i], value))
                    ++count;
            }
            return count;
        }

        public static int CountIf<T>(ReadOnlySpan<T> input, Predicate<T> predicate)
        {
            var length = input.Length;
            var count = 0;
            for (int i = 0; i < length; i++) {
                if (predicate(input[i]))
                    ++count;
            }
            return count;
        }

        public static int CountIf<T, TPredicate>(ReadOnlySpan<T> input, TPredicate predicate)
            where TPredicate : IPredicate<T>
        {
            var length = input.Length;
            var count = 0;
            for (int i = 0; i < length; i++) {
                if (predicate.Match(input[i]))
                    ++count;
            }
            return count;
        }

        public static int Mismatch<T>(ReadOnlySpan<T> input1, ReadOnlySpan<T> input2, IEqualityComparer<T> comparer = null)
        {
            Debug.Assert(input1.Length <= input2.Length);

            comparer = comparer ?? EqualityComparer<T>.Default;
            var length = input1.Length;
            for (int i = 0; i < length; i++) {
                if (!comparer.Equals(input1[i], input2[2]))
                    return i;
            }
            return -1;
        }

        public static int Find<T>(ReadOnlySpan<T> input, in T value, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var length = input.Length;
            for (int i = 0; i < length; i++) {
                if (comparer.Equals(input[i], value))
                    return i;
            }
            return -1;
        }

        public static int FindIf<T>(ReadOnlySpan<T> input, Predicate<T> predicate)
        {
            var length = input.Length;
            for (int i = 0; i < length; i++) {
                if (predicate(input[i]))
                    return i;
            }
            return -1;
        }

        public static int FindIf<T, TPredicate>(ReadOnlySpan<T> input, TPredicate predicate)
            where TPredicate : IPredicate<T>
        {
            var length = input.Length;
            for (int i = 0; i < length; i++) {
                if (predicate.Match(input[i]))
                    return i;
            }
            return -1;
        }

        public static int FindIfNot<T>(ReadOnlySpan<T> input, Predicate<T> predicate)
        {
            var length = input.Length;
            for (int i = 0; i < length; i++) {
                if (!predicate(input[i]))
                    return i;
            }
            return -1;
        }

        public static int FindIfNot<T, TPredicate>(ReadOnlySpan<T> input, TPredicate predicate)
            where TPredicate : IPredicate<T>
        {
            var length = input.Length;
            for (int i = 0; i < length; i++) {
                if (!predicate.Match(input[i]))
                    return i;
            }
            return -1;
        }

        public static int AdjacentFind<T>(ReadOnlySpan<T> input, IEqualityComparer<T> comparer = null)
        {
            var length = input.Length;
            if (length > 0) {
                comparer = comparer ?? EqualityComparer<T>.Default;
                for (int i = 1; i < length; i++) {
                    if (comparer.Equals(input[i - 1], input[i]))
                        return i - 1;
                }
            }
            return -1;
        }
    }
}
