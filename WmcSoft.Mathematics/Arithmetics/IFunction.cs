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

using System;

namespace WmcSoft.Arithmetics
{
    /// <summary>
    /// Represents a function .
    /// </summary>
    /// <typeparam name="T">The domain and the codomain.</typeparam>
    public interface IFunction<T> : IEquatable<IFunction<T>>
    {
        T Eval(T x);
    }

    public static class FunctionExtensions
    {
        public static Func<T, T> AsFunc<T>(this IFunction<T> function)
        {
            return function.Eval;
        }

        public static GenericFunction<T> AsFunction<T>(this Func<T, T> func)
        {
            return new GenericFunction<T>(func);
        }
    }

    public struct GenericFunction<T> : IFunction<T>
    {
        private readonly Func<T, T> _func;

        public GenericFunction(Func<T, T> func)
        {
            if (func == null) throw new ArgumentNullException("func");
            _func = func;
        }

        public T Eval(T x)
        {
            return _func(x);
        }

        public bool Equals(IFunction<T> other)
        {
            if (other is GenericFunction<T> that) {
                return _func.Equals(that._func);
            }
            return false;
        }
    }
}
