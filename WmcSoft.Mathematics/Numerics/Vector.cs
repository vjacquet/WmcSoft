using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Properties;

namespace WmcSoft.Numerics
{
    [Serializable]
    public struct Vector : IEquatable<Vector>
    {
        #region Fields

        private readonly double[] _data;

        #endregion

        #region Lifecycle

        public Vector(int n) {
            _data = new double[n];
        }

        public Vector(int n, Func<int, double> generator) {
            _data = new double[n];
            for (int i = 0; i < n; i++) {
                _data[i] = generator(i);
            }
        }

        public Vector(params double[] values) {
            _data = values;
        }

        #endregion

        #region Properties

        public int Cardinality { get { return _data == null ? 0 : _data.Length; } }
        public double this[int index] { get { return _data[index]; } }

        #endregion

        #region Operators

        public static Vector operator +(Vector x, Vector y) {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.BothVectorMustHaveSameLengthError);

            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = x._data[i] + y._data[i];
            }
            return result;
        }
        public static Vector Add(Vector x, Vector y) {
            return x + y;
        }

        public static Vector operator -(Vector x, Vector y) {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.BothVectorMustHaveSameLengthError);

            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = x._data[i] - y._data[i];
            }
            return result;
        }
        public static Vector Subtract(Vector x, Vector y) {
            return x - y;
        }

        public static Vector operator *(double alpha, Vector x) {
            var length = x.Cardinality;
            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = alpha * x._data[i];
            }
            return result;
        }
        public static Vector Multiply(double alpha, Vector x) {
            return alpha * x;
        }
        public static Vector operator *(Vector x, double alpha) {
            return alpha * x;
        }
        public static Vector Multiply(Vector x, double alpha) {
            return alpha * x;
        }

        public static Vector operator /(Vector x, double alpha) {
            var length = x.Cardinality;
            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = x._data[i] / alpha;
            }
            return result;
        }
        public static Vector Divide(Vector x, double alpha) {
            return x / alpha;
        }

        public static Vector operator -(Vector x) {
            var length = x.Cardinality;
            var result = new Vector(length);
            for (int i = 0; i < length; i++) {
                result._data[i] = -x._data[i];
            }
            return result;
        }
        public static Vector Negate(Vector x) {
            return -x;
        }

        public static Vector operator +(Vector x) {
            return x;
        }
        public static Vector Plus(Vector x) {
            return x;
        }

        public static double DotProduct(Vector x, Vector y) {
            var length = x.Cardinality;
            if (y.Cardinality != length)
                throw new ArgumentException(Resources.BothVectorMustHaveSameLengthError);

            var result = 0d;
            for (int i = 0; i < length; i++) {
                result += x._data[i] * y._data[i];
            }
            return result;

        }

        #endregion

        #region IEquatable<Vector> Membres

        public bool Equals(Vector other) {
            var length = Cardinality;
            if (other.Cardinality != length)
                return false;
            for (int i = 0; i < length; i++) {
                if (!_data[i].Equals(other._data[i]))
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Vector)this);
        }

        public override int GetHashCode() {
            if (_data == null)
                return 0;
            return _data.GetHashCode();
        }

        #endregion
    }
}
