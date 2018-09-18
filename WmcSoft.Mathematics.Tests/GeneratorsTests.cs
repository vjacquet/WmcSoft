using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using WmcSoft.Collections.Generic;
using System.Numerics;

namespace WmcSoft.Tests
{
    public class GeneratorsTests
    {
        [Theory]
        [InlineData(0, "1")]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(3, "6")]
        [InlineData(4, "24")]
        [InlineData(5, "120")]
        [InlineData(10, "3628800")]
        [InlineData(15, "1307674368000")]
        [InlineData(20, "2432902008176640000")]
        public void CheckFactorial(int n, string factorial)
        {
            var actual = Generators.Factorial().ElementAt(n);
            var expected = BigInteger.Parse(factorial);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckUnarySequence()
        {
            var sequence = Generators.Sequence(x => x + 2, 0);
            var expected = new[] { 0, 2, 4, 6 };
            var actual = sequence.Take(expected.Length);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckBinarySequence()
        {
            var sequence = Generators.Sequence((x, y) => x + y, 0, 1);
            var expected = new[] { 0, 1, 1, 2, 3, 5, 8, 13, 21 };
            var actual = sequence.Take(expected.Length);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckPermutations()
        {
            var permutations = Generators.Permutations('a', 'b', 'c', 'd', 'e', 'f');
            var expected = new HashSet<string>();
            var sequence = new List<string>();
            foreach (var p in permutations) {
                var s = new string(p.ToArray());
                Assert.True(expected.Add(s));
                Assert.Equal(s.Length, s.Distinct().Count());
                sequence.Add(s);
            }

            Func<int, int> factorial = n => Enumerable.Range(1, n).Aggregate((x, y) => x * y);

            var f = factorial(sequence.First().Length);
            Assert.Equal(f, expected.Count);
        }

        [Fact]
        public void SolveSendMoreMoney()
        {
            // solve SEND + MORE = MONEY
            int S = 0, E = 1, N = 2, D = 3, M = 4, O = 5, R = 6, Y = 7;
            var permutations = Generators.Permutations(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            var solutions = new HashSet<string>();
            foreach (var p in permutations) {
                if (p[S] == 0 || p[M] == 0)
                    continue;

                var send = p.ElementsAt(S, E, N, D).ToInt32();
                var more = p.ElementsAt(M, O, R, E).ToInt32();
                var money = p.ElementsAt(M, O, N, E, Y).ToInt32();
                if (send + more == money) {
                    solutions.Add(send + "+" + more + "=" + money);
                }
            }
            Assert.True(solutions.Count == 1);
            Assert.Equal("9567+1085=10652", solutions.Single());
        }
    }
}
