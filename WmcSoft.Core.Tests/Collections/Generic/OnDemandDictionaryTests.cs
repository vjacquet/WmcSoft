using System.Linq;
using Xunit;

namespace WmcSoft.Collections.Generic
{
    public class OnDemandDictionaryTests
    {
        [Fact]
        public void GetValueDoesGenerateValue()
        {
            var dictionary = new OnDemandDictionary<int, string>(x => x.ToString());

            Assert.Equal(0, dictionary.Count);

            var value = dictionary[4];
            Assert.Equal("4", value);
            Assert.Equal(1, dictionary.Count);
            Assert.Equal(4, dictionary.Keys.Single());
        }

        [Fact]
        public void TryGetValueDoesNotGenerateValue()
        {
            var dictionary = new OnDemandDictionary<int, string>(x => x.ToString());

            Assert.Equal(0, dictionary.Count);

            string value;
            var found = dictionary.TryGetValue(1, out value);
            Assert.False(found);
            Assert.Equal(0, dictionary.Count);
        }
    }
}
