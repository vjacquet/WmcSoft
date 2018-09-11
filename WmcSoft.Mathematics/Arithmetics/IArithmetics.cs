#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using WmcSoft.Algebra;

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

        // Traits
        T Zero { get; } // identity for addition
        T One { get; } // identity for multiplication

        bool SupportReciprocal { get; }

        //public T MinValue { get; }
        //public T MaxValue { get; }

        //public bool SupportInfinity { get; }
        //public bool IsPositiveInfinity(T x);
        //public bool IsNegativeInfinity(T x);
    }

    public static class ArithmeticsExtensions
    {
        static IRingLike<T> ToAdditiveMultiplicativeRing<T, A>(this A arithmetics)
            where A : IArithmetics<T>
        {
            return new AdditiveMultiplicativeRingAdapter<T, A>(arithmetics);
        }
    }
}
