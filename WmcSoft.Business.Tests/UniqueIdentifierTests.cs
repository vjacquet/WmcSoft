using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Business
{
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
