using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    public struct DoubleArithmetics : IArithmetics<double>
    {
        #region IArithmetics<double> Membres

        public double Add(double x, double y) {
            return x + y;
        }

        public double Subtract(double x, double y) {
            return x - y;
        }

        public double Multiply(double x, double y) {
            return x * y;
        }

        public double Divide(double x, double y) {
            return x / y;
        }

        public double Remainder(double x, double y) {
            return x % y;
        }

        public double DivideRemainder(double x, double y, out double remainder) {
            remainder = x % y;
            return x / y;
        }

        public double Negate(double x) {
            return -x;
        }

        public double Reciprocal(double x) {
            return 1d / x;
        }

        public double Zero {
            get { return 0d; }
        }

        public double One {
            get { return 1d; }
        }

        public bool SupportReciprocal {
            get { return true; }
        }

        #endregion
    }
}
