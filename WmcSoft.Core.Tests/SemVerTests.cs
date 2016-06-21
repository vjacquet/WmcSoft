using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft
{
    [TestClass]
    public class SemVerTests
    {
        [TestMethod]
        public void CanCreateSemVerFromVersion() {
            var version = new Version(1, 2, 3, 4);
            var semver = (SemVer)version;

            Assert.AreEqual(version.Major, semver.Major);
            Assert.AreEqual(version.Minor, semver.Minor);
            Assert.AreEqual(version.Build, semver.Patch);
        }

        [TestMethod]
        public void CheckToString() {
            var semver = new SemVer(1, 2, 3);
            Assert.AreEqual("1.2.3", semver.ToString());
        }

        [TestMethod]
        public void CanIncrement() {
            var semver = new SemVer(1, 2, 3);
            semver++;
            Assert.AreEqual("1.2.4", semver.ToString());
        }

        [TestMethod]
        public void CanCompare() {
            SemVer v0 = null;
            SemVer w0 = null;
            var v1 = new SemVer(1, 2, 3);
            var v2 = new SemVer(1, 2, 4);
            var v3 = new SemVer(1, 3, 3);
            var v4 = new SemVer(2, 2, 3);

            Assert.IsTrue(v1 < v2);
            Assert.IsTrue(v2 > v1);
            Assert.IsTrue(v1 <= v2);
            Assert.IsTrue(v2 >= v1);
            Assert.IsTrue(v2 < v3);
            Assert.IsTrue(v3 < v4);

            Assert.IsFalse(v1 < v0);
            Assert.IsFalse(v0 > v1);
            Assert.IsTrue(v1 <= v2);
            Assert.IsTrue(v2 >= v1);

            Assert.IsTrue(v0 == w0);
            Assert.IsFalse(v0 != w0);
        }
    }
}
