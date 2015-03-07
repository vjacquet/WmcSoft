using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, int length)
            where TSource : new() {
            var array = new TSource[length];
            var i = 0;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext() && i < length) {
                array[i++] = enumerator.Current;
            }
            return array;
        }
    }
}

