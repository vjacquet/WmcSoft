using System;
using Xunit;

namespace WmcSoft.Business
{
    public class UniqueIdentifierTests
    {
        [Fact]
        public void CanCreateGuidUniqueIdentifier()
        {
            IUniqueIdentifier<Guid> id = null;
            Assert.Null(id);
        }

        [Fact]
        public void CanCreateInt32UniqueIdentifier()
        {
            IUniqueIdentifier<int> id = null;
            Assert.Null(id);
        }

        [Fact]
        public void CanCreateStringUniqueIdentifier()
        {
            IUniqueIdentifier<string> id = null;
            Assert.Null(id);
        }
    }
}