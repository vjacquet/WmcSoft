using System.Collections.Generic;

namespace WmcSoft.Collections.Generic
{
    public class OrderedDictionaryTests : AssertDictionaryContractTests<string, int>
    {
        protected override IDictionary<string, int> CreateDictionary()
        {
            return new OrderedDictionary<string, int>();
        }

        protected override IEnumerable<KeyValuePair<string, int>> GetSamples()
        {
            yield return new KeyValuePair<string, int>("one", 1);
            yield return new KeyValuePair<string, int>("two", 2);
            yield return new KeyValuePair<string, int>("three", 3);
        }
    }
}
