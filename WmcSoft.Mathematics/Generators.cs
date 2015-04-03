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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class Generators
    {
        public static IEnumerable<int> Iota(int value = 0) {
            checked {
                while (true) {
                    yield return value;
                    ++value;
                }
            }
        }

        public static IEnumerable<BigInteger> Fibonacci() {
            BigInteger Fn = 0;
            BigInteger Fn1 = 1;

            yield return Fn;
            yield return Fn1;

            BigInteger Fn2;
            while (true) {
                Fn2 = Fn + Fn1;
                yield return Fn2;
                Fn = Fn1;
                Fn1 = Fn2;
            }
        }

        public static IEnumerable<int> Random(Random random) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next();
            }
        }
        public static IEnumerable<int> Random() {
            return Random(new Random());
        }

        public static IEnumerable<int> Random(Random random, int maxValue) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next(maxValue);
            }
        }
        public static IEnumerable<int> Random(int maxValue) {
            return Random(new Random(), maxValue);
        }

        public static IEnumerable<int> Random(Random random, int minValue, int maxValue) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next(minValue, maxValue);
            }
        }
        public static IEnumerable<int> Random(int minValue, int maxValue) {
            return Random(new Random(), minValue, maxValue);
        }

        public static IEnumerable<double> RandomDouble() {
            var random = new Random();
            while (true) {
                yield return random.NextDouble();
            }
        }
        public static IEnumerable<double> RandomDouble(Random random) {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true) {
                yield return random.Next();
            }
        }
    }
}
