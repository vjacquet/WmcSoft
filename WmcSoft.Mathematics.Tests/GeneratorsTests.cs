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
        public void CheckFactorial() {
            var actual = Generators.Factorial().ElementAt(5);
            Assert.AreEqual(120, actual);
        }

        [TestMethod]
        public void CheckUnarySequence() {
            using (var sequence = Generators.Sequence(x => x + 2, 0).GetEnumerator()) {
                Assert.AreEqual(0, sequence.Read());
                Assert.AreEqual(2, sequence.Read());
                Assert.AreEqual(4, sequence.Read());
                Assert.AreEqual(6, sequence.Read());
            }
        }

        [TestMethod]
        public void CheckBinarySequence() {
            using (var sequence = Generators.Sequence((x, y) => x + y, 0, 1).GetEnumerator()) {
                Assert.AreEqual(0, sequence.Read());
                Assert.AreEqual(1, sequence.Read());
                Assert.AreEqual(1, sequence.Read());
                Assert.AreEqual(2, sequence.Read());
                Assert.AreEqual(3, sequence.Read());
                Assert.AreEqual(5, sequence.Read());
                Assert.AreEqual(8, sequence.Read());
                Assert.AreEqual(13, sequence.Read());
                Assert.AreEqual(21, sequence.Read());
            }
        }

        [TestMethod]
        public void CheckPermutations() {
            var permutations = Generators.Permutations('a', 'b', 'c', 'd', 'e', 'f');
            var expected = new HashSet<string>();
            var sequence = new List<string>();
            foreach (var p in permutations) {
                var s = new String(p.ToArray());
                Assert.IsTrue(expected.Add(s));
                sequence.Add(s);
            }

            Func<int, int> factorial = n => Enumerable.Range(1, n).Aggregate((x, y) => x * y);

            var f = factorial(sequence.First().Length);
            Assert.AreEqual(f, expected.Count);
        }

        [TestMethod]
        public void SolveSendMoreMoney() {
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
            Assert.IsTrue(solutions.Count == 1);
            Assert.AreEqual("9567+1085=10652", solutions.Single());
        }
    }
}
