using Xunit;

namespace WmcSoft.TestTools.UnitTesting
{
    public static partial class ContractAssert
    {
        public static void Ordinal<TOrdinal, T>(TOrdinal ordinal, T startValue, T endValue, int distance)
           where TOrdinal : IOrdinal<T>
        {
            var actual = ordinal.Advance(startValue, distance);
            Assert.Equal(endValue, actual);
            Assert.Equal(distance, ordinal.Distance(startValue, endValue));
        }
    }
}
