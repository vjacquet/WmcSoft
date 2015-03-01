using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    public struct DecimalArithmetics : IArithmetics<decimal>
    {
        #region IArithmetics<decimal> Membres

        public decimal Add(decimal x, decimal y) {
            return x + y;
        }

        public decimal Subtract(decimal x, decimal y) {
            return x - y;
        }

        public decimal Multiply(decimal x, decimal y) {
            return x * y;
        }

        public decimal Divide(decimal x, decimal y) {
            return x / y;
        }

        public decimal Remainder(decimal x, decimal y) {
            return x % y;
        }

        public decimal DivideRemainder(decimal x, decimal y, out decimal remainder) {
            remainder = x % y;
            return x / y;
        }

        public decimal Negate(decimal x) {
            return -x;
        }

        public decimal Reciprocal(decimal x) {
            return 1m / x;
        }

        public decimal Zero {
            get { return 0m; }
        }

        public decimal One {
            get { return 1m; }
        }

        public bool SupportReciprocal {
            get { return true; }
        }

        #endregion
    }
}
