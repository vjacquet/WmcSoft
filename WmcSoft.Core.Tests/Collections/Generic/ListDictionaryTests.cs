using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ListDictionaryTests : AssertDictionaryContractTests<string, int>
    {
        protected override IDictionary<string, int> CreateDictionary() {
            return new ListDictionary<string, int>();
        }

        protected override IEnumerable<KeyValuePair<string, int>> GetSamples() {
            yield return new KeyValuePair<string, int>("one", 1);
            yield return new KeyValuePair<string, int>("two", 2);
            yield return new KeyValuePair<string, int>("three", 3);
        }
    }
}
