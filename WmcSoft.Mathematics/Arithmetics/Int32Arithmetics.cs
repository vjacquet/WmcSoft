using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    public struct Int32Arithmetics : IArithmetics<int>
    {
        #region IArithmetics<int> Membres

        public int Add(int x, int y) {
            return x + y;
        }

        public int Subtract(int x, int y) {
            return x - y;
        }

        public int Multiply(int x, int y) {
            return x * y;
        }

        public int Divide(int x, int y) {
            return x / y;
        }

        public int Remainder(int x, int y) {
            return x % y;
        }

        public int DivideRemainder(int x, int y, out int remainder) {
            return Math.DivRem(x, y, out remainder);
        }

        public int Negate(int x) {
            return -x;
        }

        public int Reciprocal(int x) {
            throw new NotSupportedException();
        }

        public int Zero {
            get { return 0; }
        }

        public int One {
            get { return 1; }
        }

        public bool SupportReciprocal {
            get { return false; }
        }

        #endregion
    }
}
