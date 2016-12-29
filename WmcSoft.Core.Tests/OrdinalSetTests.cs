using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace WmcSoft
{
    using Letters = OrdinalSet<char, OrdinalSetTests.Letter>;

    [TestClass]
    public class OrdinalSetTests
    {
        internal struct Letter : IOrdinal<char>, IReadOnlyList<char>
        {
            const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

            public char this[int index] {
                get { return Alphabet[index]; }
            }

            public int Count {
                get { return Alphabet.Length; }
            }

            public char Advance(char x, int n) {
                var i = Alphabet.IndexOf(x);
                return Alphabet[i + n];
            }

            public int Compare(char x, char y) {
                return Alphabet.IndexOf(x) - Alphabet.IndexOf(y);
            }

            public IEnumerator<char> GetEnumerator() {
                return Alphabet.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }


        [TestMethod]
        public void CheckUnion() {
            var vowels = new Letters("aeiouy");
            var consonants = new Letters("bcdfghjklmnpqrstvwxz");
            var all = vowels | consonants;

            Assert.AreEqual(26, all.Count);
        }

        [TestMethod]
        public void CheckIntersect() {
            var vowels = new Letters("aeiouy");
            var consonants = new Letters("bcdfghjklmnpqrstvwxz");
            var none = vowels & consonants;

            Assert.AreEqual(0, none.Count);
        }

        [TestMethod]
        public void CheckComplement() {
            var vowels = new Letters("aeiouy");
            var consonants = new Letters("bcdfghjklmnpqrstvwxz");

            Assert.AreEqual(vowels, ~consonants);
        }

        [TestMethod]
        public void CheckOrderDoesNotMatter() {
            var spiderman = new Letters("spiderman");
            var manspider = new Letters("manspider");

            Assert.AreEqual(spiderman, manspider);
        }

        [TestMethod]
        public void CheckDifference() {
            var x = new Letters("abcdef");
            var y = new Letters("begh");

            Assert.AreEqual(new Letters("acdf"), x - y);
            Assert.AreEqual(new Letters("gh"), y - x);
        }

        [TestMethod]
        public void CheckToString() {
            var x = new Letters("abcdef");

            Assert.AreEqual("{a,b,c,d,e,f}", x.ToString());
        }

        [TestMethod]
        public void CheckEnumeration() {
            var x = new Letters("abcdef");

            Assert.AreEqual("abcdef", new string(x.ToArray()));
        }

        [TestMethod]
        public void CheckSymmetricDifference() {
            var x = new Letters("abcdef");
            var y = new Letters("begh");

            Assert.AreEqual(new Letters("acdfgh"), x ^ y);
        }
    }
}
