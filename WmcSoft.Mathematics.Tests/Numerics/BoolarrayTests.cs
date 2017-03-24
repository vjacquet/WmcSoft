using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Numerics.Tests
{
    [TestClass]
    public class BoolarrayTests
    {
        [TestMethod]
        public void CheckBoolarray()
        {
            var x = new Boolarray(true, true, true, true);
            var y = new Boolarray(true, true, false, true);
            var z = new Boolarray(true, true, true, true);

            if (x) { }
            else { Assert.Fail(); }

            if (!x) { Assert.Fail(); }
            else { }

            if (y) { Assert.Fail(); }
            else { }

            if (x && z) { }
            else { Assert.Fail(); }

            if (x || z) { }
            else { Assert.Fail(); }

            if (x || y) { }
            else { Assert.Fail(); }

            if (x && !y) { }
            else { Assert.Fail(); }
        }
    }
}