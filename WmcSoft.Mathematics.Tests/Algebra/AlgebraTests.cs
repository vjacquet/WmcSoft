using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WmcSoft.Algebra.Tests
{
    [TestClass]
    public class AlgebraTests
    {
        [TestMethod]
        public void AssumesFunctions() {
            Func<double, double> sin = Math.Sin;
            Func<double, double> sin2 = Math.Sin;
            Func<double, double> cos = Math.Cos;

            Expression<Func<double, double>> f = (x) => sin(x);

            Assert.AreEqual(sin, sin2);
            Assert.AreNotEqual(sin, cos);
        }

        [TestMethod]
        public void CheckGroup() {
            var m = Addition.Operation;
            var x = 5;
            var y = 2;
            var z = m.Eval(x, y);

            Assert.AreEqual(7, z);
            Assert.IsTrue(m.IsAbelianGroup());
        }
    }

    public struct Addition : IGroupLike<int>
    {
        public static Addition Operation;

        public int Negate(int x) {
            return -x;
        }
        public int Zero { get { return 0; } }

        #region IGroupLike<int> Members

        int IGroupLike<int>.Inverse(int x) {
            return -x;
        }
        int IGroupLike<int>.Identity {
            get { return 0; }
        }

        public int Eval(int x, int y) {
            return x + y;
        }

        #endregion

        #region IGroupLikeTraits Members

        public bool SupportIdentity { get { return true; } }
        public bool SupportInverse { get { return true; } }

        public bool IsAssociative { get { return true; } }
        public bool IsCommutative { get { return true; } }
        public bool IsIdempotent { get { return false; } }

        #endregion
    }
}
