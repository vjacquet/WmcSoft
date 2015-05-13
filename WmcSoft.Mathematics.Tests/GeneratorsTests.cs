using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Tests
{
    [TestClass]
    public class GeneratorsTests
    {
        [TestMethod]
        public void CheckUnarySeries() {
            var series = Generators.Series(x => x + 2, 0).GetEnumerator();
            Assert.AreEqual(0, series.Read());
            Assert.AreEqual(2, series.Read());
            Assert.AreEqual(4, series.Read());
            Assert.AreEqual(6, series.Read());
        }

        [TestMethod]
        public void CheckBinarySeries() {
            var series = Generators.Series((x, y) => x + y, 0, 1).GetEnumerator();
            Assert.AreEqual(0, series.Read());
            Assert.AreEqual(1, series.Read());
            Assert.AreEqual(1, series.Read());
            Assert.AreEqual(2, series.Read());
            Assert.AreEqual(3, series.Read());
            Assert.AreEqual(5, series.Read());
            Assert.AreEqual(8, series.Read());
            Assert.AreEqual(13, series.Read());
            Assert.AreEqual(21, series.Read());
        }

        [TestMethod]
        public void CheckPermutations() {
            var permutations = Generators.Permutations("a", "b", "c", "d", "e", "f");
            var expected = new HashSet<string>();
            var sequence = new List<string>();
            foreach (var p in permutations) {
                var s = p.Join("");
                Assert.IsTrue(expected.Add(s));
                sequence.Add(s);
            }

            Func<int, int> factorial = n => Enumerable.Range(1, n).Aggregate((x, y) => x * y);

            var f = factorial(sequence.First().Length);
            Assert.AreEqual(f, expected.Count);
        }

        int ToInt32(IReadOnlyList<int> code, params int[] indices) {
            return indices.Select(i => code[i]).ToInt32();
        }

        [TestMethod]
        public void SolveSendMoreMoney() {
            int S = 0, E = 1, N = 2, D = 3, M = 4, O = 5, R = 6, Y = 7;
            var permutations = Generators.Permutations(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            var solutions = new HashSet<string>();
            foreach (var p in permutations) {
                if (p[S] == 0 || p[M] == 0)
                    continue;

                var send = ToInt32(p, S, E, N, D);
                var more = ToInt32(p, M, O, R, E);
                var money = ToInt32(p, M, O, N, E, Y);
                if (send + more == money) {
                    solutions.Add(send + "+" + more + "=" + money);
                }
            }
            Assert.IsTrue(solutions.Count == 1);
            Assert.AreEqual("9567+1085=10652", solutions.Single());
        }
    }
}
