using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue ConvertOrDefault<TKey, TValue>(this IDictionary<TKey, object> source, TKey key, TValue defaultValue = default(TValue)) {
            if (source != null) {
                object value;
                if (source.TryGetValue(key, out value))
                    return (TValue)Convert.ChangeType(value, typeof(TValue));
            }
            return defaultValue;
        }

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
