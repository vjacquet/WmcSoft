using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmcSoft
{
    internal class SpecialFunctions<TElement>
    {
        public static Func<TElement, TElement> Identity {
            get { return x => x; }
        }

        public static Predicate<TElement> Always {
            get { return _ => true; }
        }

        public static Predicate<TElement> Never {
            get { return _ => false; }
        }
    }
}
