using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class ComparerTests
    {
        [Fact]
        public void CheckConformingReverseComparerWithNullValues()
        {
            var comparer = new ConformingReverseComparer<string>(StringComparer.Ordinal);

            Assert.True(comparer.Compare(default(string), "x") < 0);
            Assert.True(comparer.Compare("y", default(string)) > 0);
            Assert.True(comparer.Compare(default(string), default(string)) == 0);
            Assert.True(comparer.Compare("x", "y") > 0);
        }

        [Fact]
        public void CheckReverseComparerWithNullValues()
        {
            var comparer = new ReverseComparer<string>(StringComparer.Ordinal);

            Assert.True(comparer.Compare(default(string), "x") > 0);
            Assert.True(comparer.Compare("y", default(string)) < 0);
            Assert.True(comparer.Compare(default(string), default(string)) == 0);
            Assert.True(comparer.Compare("x", "y") > 0);
        }

        [Fact]
        public void CheckSelectCompare()
        {
            var expected = new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var comparer = new SelectComparer<int, int>(x => -x);
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Array.Sort(data, comparer);
            Assert.Equal(expected, data);
        }

        [Fact]
        public void CheckCustomSortCompare()
        {
            var comparer = new CustomSortComparer<int>(2, 4, 8);
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 4 };
            Array.Sort(data, comparer);
            Assert.Equal(2, data[0]);
            Assert.Equal(4, data[1]);
            Assert.Equal(4, data[2]);
            Assert.Equal(8, data[3]);
        }

        [Fact]
        public void CheckCompareBuilder()
        {
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var annabelle = new Person { Name = "Annabelle", Age = 10 };
            var comparer = Compare.OrderOf<Person>().By(x => x.Age);
            Assert.True(comparer.Compare(alicia, annabelle) > 0);
        }

        [Fact]
        public void CanAdaptComparerForEquality()
        {
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var annabelle = new Person { Name = "Annabelle", Age = 10 };
            var jessica = new Person { Name = "Jessica", Age = 11 };
            var comparer = new EqualityComparerAdapter<Person>(Compare.OrderOf<Person>().By(x => x.Age));
            Assert.True(comparer.Equals(alicia, jessica));
            Assert.False(comparer.Equals(alicia, annabelle));
        }

        [Fact]
        public void CanUseEqualityComparerAdapterInHashSet()
        {
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var annabelle = new Person { Name = "Annabelle", Age = 10 };
            var jessica = new Person { Name = "Jessica", Age = 11 };
            var comparer = new EqualityComparerAdapter<Person>(Compare.OrderOf<Person>().By(x => x.Age));
            var set = new HashSet<Person>(comparer);
            Assert.True(set.Add(alicia));
            Assert.False(set.Add(jessica));
        }

        [Fact]
        public void CanUseEqualityComparerToCompareDefaultValue()
        {
            var nobody = default(Person);
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var jessica = new Person { Name = "Jessica", Age = 11 };
            var comparer = new EqualityComparerAdapter<Person>(Compare.OrderOf<Person>().By(x => x.Age));
            Assert.True(comparer.Equals(alicia, jessica));
            Assert.True(comparer.Equals(nobody, nobody));
            Assert.False(comparer.Equals(alicia, nobody));
            Assert.False(comparer.Equals(nobody, jessica));
        }

        [Fact]
        public void CanCompareLexicographically()
        {
            int[] a = { 1, 2, 3, 4 };
            int[] b = { 1, 2, 3, 4, 5 };
            int[] c = { 4, 3, 2, 1 };

            var comparer = Comparer<int>.Default.Lexicographical();

            Assert.True(comparer.Compare(a, a) == 0);
            Assert.True(comparer.Compare(a, b) < 0);
            Assert.True(comparer.Compare(b, a) > 0);
            Assert.True(comparer.Compare(b, c) < 0);
            Assert.True(comparer.Compare(c, b) > 0);
            Assert.True(comparer.Compare(a, c) < 0);
            Assert.True(comparer.Compare(c, a) > 0);
        }
    }

    [DebuggerDisplay("{Name} ({Age} years old)")]
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
