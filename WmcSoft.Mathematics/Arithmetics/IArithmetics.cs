using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    /// <summary>
    /// Interface to efficiently implement calculators
    /// </summary>
    /// <typeparam name="T">The type used in the operators</typeparam>
    /// <remarks>See <http://www.codeproject.com/Articles/8531/Using-generics-for-calculations>.</remarks>
    public interface IArithmetics<T>
    {
        // Binary operators
        T Add(T x, T y);
        T Subtract(T x, T y);
        T Multiply(T x, T y);
        T Divide(T x, T y);
        T Remainder(T x, T y);
        T DivideRemainder(T x, T y, out T remainder);

        // Unary operators
        T Negate(T x);
        T Reciprocal(T x);

        //// Traits
        T Zero { get; } // identity for addition
        T One { get; } // identity for multiplication

        bool SupportReciprocal { get; }

        //public T MinValue { get; }
        //public T MaxValue { get; }

        //public bool SupportInfinity { get; }
        //public bool IsPositiveInfinity(T x);
        //public bool IsNegativeInfinity(T x);
    }
}
