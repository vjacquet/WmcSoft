using Xunit;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    public class MutableOrdinalSetTests
    {
        [Fact]
        public void CheckOrdinalSetIsSet()
        {
            ContractAssert.Set(new MutableOrdinalSet<int>(new Int32Ordinal(), ContractAssert.CollectionMinValue, ContractAssert.CollectionMaxValue));
        }
    }
}
