using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class ListDictionaryTests
    {
        [TestMethod]
        public void CanConstructListDictionary() {
            var dictionary = new ListDictionary<int, string>();
            Assert.IsNotNull(dictionary);
        }
    }
}
