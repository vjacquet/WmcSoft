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

namespace WmcSoft.Memory
{
    public static class ModifyingSequence
    {

        static int Min(int x, int y)
        {
            return (y < x) ? y : x;
        }

        static int Min(int x, int y, int z)
        {
            return (y < x) ? Math.Min(y, z) : Math.Min(x, z);
        }

        public static void Fill<T>(Span<T> output, T value = default)
        {
            var length = output.Length;
            for (int i = 0; i < length; i++) {
                output[i] = value;
            }
        }

        public static int Transform<T, U>(ReadOnlySpan<T> input, Span<U> output, Func<T, U> transform)
        {
            var length = Min(input.Length, output.Length);
            for (int i = 0; i < length; i++) {
                output[i] = transform(input[i]);
            }
            return length;
        }

        public static int Transform<T, U, V>(ReadOnlySpan<T> input1, ReadOnlySpan<U> input2, Span<V> output, Func<T, U, V> transform)
        {
            var length = Min(input1.Length, input2.Length, output.Length);
            for (int i = 0; i < length; i++) {
                output[i] = transform(input1[i], input2[i]);
            }
            return length;
        }

    }
}
