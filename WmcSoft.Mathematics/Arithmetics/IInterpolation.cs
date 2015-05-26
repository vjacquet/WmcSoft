using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Arithmetics
{
    public interface IInterpolation<T, U>
    {
        U Interpolate(T value);
    }

    public static class InterpolationExtensions
    {
        public static Func<T, U> AsFunction<T, U>(this IInterpolation<T, U> interpolation) {
            return (t) => interpolation.Interpolate(t);
        }
    }
}
