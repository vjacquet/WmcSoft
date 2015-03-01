using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    public static class IntExtensions
    {
        public static int Clamp(this long x) {
            if (x < Int32.MinValue)
                return Int32.MinValue;
            if (x > Int32.MaxValue)
                return Int32.MaxValue;
            return (int)x;
        }
    }
}
