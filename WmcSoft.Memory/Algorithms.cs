using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WmcSoft.Memory
{
    public static class Algorithms
    {
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

        public static int FindIfNot<T>(ReadOnlySpan<T> input, Predicate<T> predicate)
        {
            var length = input.Length;
            for (int i = 0; i < length; i++) {
                if (!predicate(input[i]))
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

         static int Min(int x, int y)
        {
            return (y < x) ? y : x;
        }

         static int Min(int x, int y, int z)
        {
            return (y < x) ? Math.Min(y, z) : Math.Min(x, z);
        }
    }
}
