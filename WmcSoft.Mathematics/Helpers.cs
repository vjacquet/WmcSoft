using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Numerics;

namespace WmcSoft
{
    internal class Helpers
    {
        public static bool Even(int n)
        {
            return (n & 1) == 0;
        }

        public static bool Odd(int n)
        {
            return (n & 1) == 1;
        }

        public static int HalfNonNegative(int n)
        {
            return n / 2;
        }

        public static void Fill<T>(T[] data, T value)
        {
            Fill(data, 0, data.Length, value);
        }

        public static void Fill<T>(T[] data, int begin, int end, T value)
        {
            for (int i = begin; i < end; i++) {
                data[i] = value;
            }
        }

        public static double DotProductNotEmpty(int length, double[] data1, int startIndex1, double[] data2, int startIndex2)
        {
            var r = data1[startIndex1] * data2[startIndex2];

            while (--length > 0) {
                r += data1[++startIndex1] * data2[++startIndex2];
            }
            return r;
        }

        public static double DotProductNotEmpty(int length, double[] data1, int startIndex1, int stride1, double[] data2, int startIndex2, int stride2)
        {
            var r = data1[startIndex1] * data2[startIndex2];

            while (--length > 0) {
                startIndex1 += stride1;
                startIndex2 += stride2;
                r += data1[startIndex1] * data2[startIndex2];
            }
            return r;
        }

        public static double DotProductNotEmpty(int length, Band<double> x, Band<double> y)
        {
            using (var ex = x.GetEnumerator())
            using (var ey = y.GetEnumerator()) {
                return DotProductNotEmpty(length, ex, ey);
            }
        }
        public static double DotProductNotEmpty<E>(int length, E ex, E ey)
            where E : IEnumerator<double>
        {
            ex.MoveNext();
            ey.MoveNext();

            var result = ex.Current * ey.Current;
            length--;
            while (length-- > 0) {
                ex.MoveNext();
                ey.MoveNext();
                result += ex.Current * ey.Current;
            }
            return result;
        }
    }
}
