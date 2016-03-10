using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ComparerTests
    {
        [TestMethod]
        public void CheckSelectCompare() {
            var expected = new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var comparer = new SelectComparer<int, int>(x => -x);
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Array.Sort(data, comparer);
            CollectionAssert.AreEquivalent(expected, data);
        }

        [TestMethod]
        public void CheckCustomSortCompare() {
            var comparer = new CustomSortComparer<int>(2, 4, 8);
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 4 };
            Array.Sort(data, comparer);
            Assert.AreEqual(2, data[0]);
            Assert.AreEqual(4, data[1]);
            Assert.AreEqual(4, data[2]);
            Assert.AreEqual(8, data[3]);
        }

        [TestMethod]
        public void CheckCompareBuilder() {
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var annabelle = new Person { Name = "Annabelle", Age = 10 };
            var comparer = Compare.OrderOf<Person>().By(x => x.Age);
            Assert.IsTrue(comparer.Compare(alicia, annabelle) > 0);
        }

        [TestMethod]
        public void CanAdaptComparerForEquality() {
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var annabelle = new Person { Name = "Annabelle", Age = 10 };
            var jessica = new Person { Name = "Jessica", Age = 11 };
            var comparer = new EqualityComparerAdapter<Person>(Compare.OrderOf<Person>().By(x => x.Age));
            Assert.IsTrue(comparer.Equals(alicia, jessica));
            Assert.IsFalse(comparer.Equals(alicia, annabelle));
        }

        [TestMethod]
        public void CanUseEqualityComparerAdapterInHashSet() {
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var annabelle = new Person { Name = "Annabelle", Age = 10 };
            var jessica = new Person { Name = "Jessica", Age = 11 };
            var comparer = new EqualityComparerAdapter<Person>(Compare.OrderOf<Person>().By(x => x.Age));
            var set = new HashSet<Person>(comparer);
            Assert.IsTrue(set.Add(alicia));
            Assert.IsFalse(set.Add(jessica));
        }

        [TestMethod]
        public void CanUseEqualityComparerToCompareDefaultValue() {
            var nobody = default(Person);
            var alicia = new Person { Name = "Alicia", Age = 11 };
            var jessica = new Person { Name = "Jessica", Age = 11 };
            var comparer = new EqualityComparerAdapter<Person>(Compare.OrderOf<Person>().By(x => x.Age));
            Assert.IsTrue(comparer.Equals(alicia, jessica));
            Assert.IsTrue(comparer.Equals(nobody, nobody));
            Assert.IsFalse(comparer.Equals(alicia, nobody));
            Assert.IsFalse(comparer.Equals(nobody, jessica));
        }
    }

    [DebuggerDisplay("{Name} ({Age} years old)")]
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
