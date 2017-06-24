using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft
{
    using Letters = OrdinalSet<char, OrdinalSetTests.Letter>;

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

            public char Advance(char x, int n)
            {
                var i = Alphabet.IndexOf(x);
                return Alphabet[i + n];
            }

            public int Compare(char x, char y)
            {
                return Alphabet.IndexOf(x) - Alphabet.IndexOf(y);
            }

            public IEnumerator<char> GetEnumerator()
            {
                return Alphabet.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        [Fact]
        public void CheckUnion()
        {
            var vowels = new Letters("aeiouy");
            var consonants = new Letters("bcdfghjklmnpqrstvwxz");
            var all = vowels | consonants;

            Assert.Equal(26, all.Count);
        }

        [Fact]
        public void CheckIntersect()
        {
            var vowels = new Letters("aeiouy");
            var consonants = new Letters("bcdfghjklmnpqrstvwxz");
            var none = vowels & consonants;

            Assert.Equal(0, none.Count);
        }

        [Fact]
        public void CheckComplement()
        {
            var vowels = new Letters("aeiouy");
            var consonants = new Letters("bcdfghjklmnpqrstvwxz");

            Assert.Equal(vowels, ~consonants);
        }

        [Fact]
        public void CheckOrderDoesNotMatter()
        {
            var spiderman = new Letters("spiderman");
            var manspider = new Letters("manspider");

            Assert.Equal(spiderman, manspider);
        }

        [Fact]
        public void CheckDifference()
        {
            var x = new Letters("abcdef");
            var y = new Letters("begh");

            Assert.Equal(new Letters("acdf"), x - y);
            Assert.Equal(new Letters("gh"), y - x);
        }

        [Fact]
        public void CheckToString()
        {
            var x = new Letters("abcdef");

            Assert.Equal("{a,b,c,d,e,f}", x.ToString());
        }

        [Fact]
        public void CheckEnumeration()
        {
            var x = new Letters("abcdef");

            Assert.Equal("abcdef", new string(x.ToArray()));
        }

        [Fact]
        public void CheckSymmetricDifference()
        {
            var x = new Letters("abcdef");
            var y = new Letters("begh");

            Assert.Equal(new Letters("acdfgh"), x ^ y);
        }
    }
}
