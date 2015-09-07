using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    /// <summary>
    /// Provides a base for implementing <see cref="Random"/> generators.
    /// </summary>
    /// <remarks>Unlike Random, all methods call Sample</remarks>
    public abstract class RandomBase : Random
    {
        public sealed override double NextDouble() {
            return base.NextDouble();
        }

        public sealed override int Next() {
            return Next(0, Int32.MaxValue);
        }

        public sealed override int Next(int maxValue) {
            return Next(0, maxValue);
        }

        public sealed override int Next(int minValue, int maxValue) {
            double value = Sample();
            return minValue + (int)System.Math.Floor(0.5 + value * ((long)maxValue - (long)minValue));
        }
    }
}
