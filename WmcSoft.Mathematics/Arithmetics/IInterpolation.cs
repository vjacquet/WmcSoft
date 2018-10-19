using System;

namespace WmcSoft.Arithmetics
{
    /// <summary>
    /// Represents an algorithm to interpolate a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="U">The type of the result.</typeparam>
    public interface IInterpolation<T, U>
    {
        U Interpolate(T value);
    }

    public static class InterpolationExtensions
    {
        public static Func<T, U> AsFunc<T, U>(this IInterpolation<T, U> interpolation)
        {
            return interpolation.Interpolate;
        }

        public static InterpolationAdapter<T> AsFunction<T>(this IInterpolation<T, T> interpolation)
        {
            return new InterpolationAdapter<T>(interpolation);
        }
    }

    public struct InterpolationAdapter<T> : IFunction<T>
    {
        private readonly IInterpolation<T, T> underlying;

        public InterpolationAdapter(IInterpolation<T, T> interpolation)
        {
            underlying = interpolation;
        }

        public T Eval(T x)
        {
            return underlying.Interpolate(x);
        }

        public bool Equals(IFunction<T> other)
        {
            if (other is InterpolationAdapter<T> that) {
                return underlying.Equals(that.underlying);
            }
            return false;
        }


    }
}
