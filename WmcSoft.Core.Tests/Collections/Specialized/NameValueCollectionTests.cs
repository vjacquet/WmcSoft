using System;
using System.Collections.Specialized;
using Xunit;

namespace WmcSoft.Collections.Specialized
{
    public class NameValueCollectionTests : IClassFixture<NameValueCollectionTests.Fixture>
    {
        public class Fixture : NameValueCollection
        {
            public Fixture()
            {
                Add("int", "1664");
                Add("date", "1973-05-02");
                Add("datetime", "1973-05-02T04:30");
                Add("timespan", "01:02:03.004");
                Add("ints", "1");
                Add("ints", "2");
                Add("ints", "3");
            }
        }

        readonly NameValueCollection _collection;

        public NameValueCollectionTests(Fixture fixture)
        {
            _collection = fixture;
        }

        [Fact]
        public void CheckGetInt()
        {
            Assert.Equal(1664, _collection.GetValue<int>("int"));
        }

        [Fact]
        public void CheckGetDate()
        {
            var expected = new DateTime(1973, 5, 2);
            Assert.Equal(expected, _collection.GetValue<DateTime>("date"));
        }

        [Fact]
        public void CheckGetDateTime()
        {
            var expected = new DateTime(1973, 5, 2, 4, 30, 0);
            Assert.Equal(expected, _collection.GetValue<DateTime>("datetime"));
        }

        [Fact]
        public void CheckGetTimespan()
        {
            var expected = new TimeSpan(0, 1, 2, 3, 4);
            Assert.Equal(expected, _collection.GetValue<TimeSpan?>("timespan"));
        }

        [Fact]
        public void CheckGetInts()
        {
            var expected = new[] { 1, 2, 3 };
            var actual = _collection.GetValues<int>("ints");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckNameValueCollection()
        {
            var collection = new NameValueCollection();
            collection.Add("letter", "a");
            collection.Add("letter", "b");
            collection.Add("number", "1");
            Assert.Equal(2, collection.Count);
            Assert.Equal(1, collection.PopValue<int>("number"));
            Assert.Single(collection);
            Assert.Equal(0d, collection.PopValue<double>("number"));
            Assert.Single(collection);
            Assert.Equal(new[] { "a", "b" }, collection.PopValues<string>("letter").ToArray());
            Assert.Empty(collection);
        }

    }


}
