using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue)) {
            if (source != null) {
                TValue value;
                if (source.TryGetValue(key, out value))
                    return value;
            }
            return defaultValue;
        }
    }
}
