using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business
{
    /// <summary>
    /// Summary description for UniqueIdentifierTests
    /// </summary>
    [TestClass]
    public class UniqueIdentifierTests
    {
        [TestMethod]
        public void CanCreateGuidUniqueIdentifier() {
            IUniqueIdentifier<Guid> id = null;
            Assert.IsNull(id);
        }

        [TestMethod]
        public void CanCreateInt32UniqueIdentifier() {
            IUniqueIdentifier<int> id = null;
            Assert.IsNull(id);
        }

        [TestMethod]
        public void CanCreateStringUniqueIdentifier() {
            IUniqueIdentifier<string> id = null;
            Assert.IsNull(id);
        }
    }
}
