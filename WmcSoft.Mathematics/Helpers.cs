using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    internal class Helpers
    {
        public static bool Even(int n)
        {
            return (n & 1) == 0;
        }

        public static bool Odd(int n)
        {
            return (n & 1) == 1;
        }

        public static int HalfNonNegative(int n)
        {
            return n / 2;
        }
    }
}
