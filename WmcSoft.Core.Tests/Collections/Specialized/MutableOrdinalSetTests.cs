using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WmcSoft.TestTools.UnitTesting;

namespace WmcSoft.Collections.Specialized
{
    [TestClass]
    public class MutableOrdinalSetTests
    {
        [TestMethod]
        public void CheckOrdinalSetIsSet() {
            ContractAssert.Set(new MutableOrdinalSet<int>(new Int32Ordinal(), ContractAssert.CollectionMinValue, ContractAssert.CollectionMaxValue));
        }
    }
}
