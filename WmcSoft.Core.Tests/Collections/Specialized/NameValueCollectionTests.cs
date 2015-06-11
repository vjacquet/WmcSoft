using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class NameValueCollectionTests
    {
        NameValueCollection _collection;

        [TestInitialize()]
        public void Initialize() {
            _collection = new NameValueCollection();
            _collection.Add("int", "1664");
            _collection.Add("date", "1973-05-02");
            _collection.Add("datetime", "1973-05-02T04:30");
            _collection.Add("timespan", "01:02:03.004");
            _collection.Add("ints", "1");
            _collection.Add("ints", "2");
            _collection.Add("ints", "3");
        }

        [TestMethod]
        public void CheckGetInt() {
            Assert.AreEqual(1664, _collection.GetValue<int>("int"));
        }

        [TestMethod]
        public void CheckGetDate() {
            var expected = new DateTime(1973, 5, 2);
            Assert.AreEqual(expected, _collection.GetValue<DateTime>("date"));
        }

        [TestMethod]
        public void CheckGetDateTime() {
            var expected = new DateTime(1973, 5, 2, 4, 30, 0);
            Assert.AreEqual(expected, _collection.GetValue<DateTime>("datetime"));
        }

        [TestMethod]
        public void CheckGetTimespan() {
            var expected = new TimeSpan(0, 1, 2, 3, 4);
            Assert.AreEqual(expected, _collection.GetValue<TimeSpan?>("timespan"));
        }

        [TestMethod]
        public void CheckGetInts() {
            var expected = new []{1, 2, 3};
            var actual = _collection.GetValues<int>("ints").ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckNameValueCollection() {
            var collection = new NameValueCollection();
            collection.Add("letter", "a");
            collection.Add("letter", "b");
            collection.Add("number", "1");
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(1, collection.PopValue<int>("number"));
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(0d, collection.PopValue<double>("number"));
            Assert.AreEqual(1, collection.Count);
            CollectionAssert.AreEqual(new[] { "a", "b" }, collection.PopValues<string>("letter").ToArray());
            Assert.AreEqual(0, collection.Count);
        }

    }


}
