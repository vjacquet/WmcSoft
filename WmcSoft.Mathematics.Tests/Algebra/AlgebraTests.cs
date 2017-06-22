using System;
using System.Linq.Expressions;
using Xunit;

namespace WmcSoft.Algebra.Tests
{
    public class AlgebraTests
    {
        [Fact]
        public void AssumesFunctions()
        {
            Func<double, double> sin = Math.Sin;
            Func<double, double> sin2 = Math.Sin;
            Func<double, double> cos = Math.Cos;

            Expression<Func<double, double>> f = (x) => sin(x);

            Assert.Equal(sin, sin2);
            Assert.NotEqual(sin, cos);
        }

        [Fact]
        public void CheckGroup()
        {
            var m = Addition.Operation;
            var x = 5;
            var y = 2;
            var z = m.Eval(x, y);

            Assert.Equal(7, z);
            Assert.True(m.IsAbelianGroup());
        }
    }

    public struct Addition : IGroupLike<int>
    {
        public static Addition Operation;

        public int Negate(int x)
        {
            return -x;
        }
        public int Zero { get { return 0; } }

        #region IGroupLike<int> Members

        int IGroupLike<int>.Inverse(int x)
        {
            return -x;
        }
        int IGroupLike<int>.Identity {
            get { return 0; }
        }

        public int Eval(int x, int y)
        {
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
