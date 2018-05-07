using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic.Algorithms
{
    public class FinderTests
    {
        [Fact]
        public void CanFindUsingNaiveFinder()
        {
            const string p = "10100111";
            const string t = "100111010010100010100111000111";

            var a = new NaiveFinder<char>(p.AsReadOnlyList(), EqualityComparer<char>.Default);
            var actual = a.FindNextOccurence(t.AsReadOnlyList(), 5);
            var expected = t.IndexOf(p);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFindUsingKnuthMorrisPratt()
        {
            const string p = "10100111";
            const string t = "100111010010100010100111000111";

            var a = new KnuthMorrisPrattFinder<char>(p.AsReadOnlyList(), EqualityComparer<char>.Default);
            var actual = a.FindNextOccurence(t.AsReadOnlyList(), 5);
            var expected = t.IndexOf(p);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFindAllUsingKnuthMorrisPratt()
        {
            const string p = "10100111";
            const string t = "1001110100101000101001110001010011111";

            var a = new KnuthMorrisPrattFinder<char>(p.AsReadOnlyList(), EqualityComparer<char>.Default);
            var actual = a.FindAllOccurences(t.AsReadOnlyList()).ToArray();
            var expected = new[] {
                t.IndexOf(p),
                t.LastIndexOf(p)
            };
            Assert.Equal(expected, actual);
        }

        class Basic : IEqualityComparer<Basic>
        {
            public bool Equals(Basic x, Basic y)
            {
                return true;
            }

            public int GetHashCode(Basic obj)
            {
                return 0;
            }
        }

        class Derived : Basic
        {
        }

        [Fact]
        public void CanFindWithContravariance()
        {
            var list = new List<Derived>();
            var pattern = new List<Basic>();
            var finder = new NaiveFinder<Basic>(pattern);
            finder.FindNextOccurence(list, 0);
        }
    }
}
