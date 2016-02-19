using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Asserts
{
    [TestClass]
    public class GetHashCodeAsserts
    {
        [TestMethod]
        public void CanGetHashCodeOfNullableType() {
            DateTime? x = default(DateTime?);

            Assert.IsTrue(x == null);
            Assert.AreEqual(0, x.GetHashCode());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotGetHashCodeOfNullStringFromOrdinalIgnoreCase() {
            Assert.AreEqual(0, StringComparer.OrdinalIgnoreCase.GetHashCode(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotGetHashCodeOfNullStringFromInvariantCulture() {
            Assert.AreEqual(0, StringComparer.InvariantCulture.GetHashCode(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotGetHashCodeOfNullStringFromInvariantCultureIgnoreCase() {
            Assert.AreEqual(0, StringComparer.InvariantCultureIgnoreCase.GetHashCode(null));
        }
    }
}
