using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Collections.Generic
{
    [TestClass]
    public class IndexTests
    {
        [TestMethod]
        public void CanAddAndRemoveToIndex() {
            var index = new Index<int, string>();
            index.Add(1, "one");
            index.Add(1, "un");
            Assert.AreEqual(2, index.Count);

            index.Add(2, "two");

            Assert.AreEqual(3, index.Count);
            Assert.AreEqual("one un", index.GetValues(1).JoinWith(' '));

            index.Remove(1, "one");
            Assert.AreEqual(2, index.Count);
            Assert.AreEqual("un", index.GetValues(1).JoinWith(' '));

            index.Remove(1, "un");
            Assert.AreEqual("", index.GetValues(1).JoinWith(' '));
        }
    }
}
