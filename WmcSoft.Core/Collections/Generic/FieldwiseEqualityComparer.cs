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

 ----------------------------------------------------------------------------

    Adapted from <http://grabbagoft.blogspot.fr/2007/06/generic-value-object-equality.html>

 ****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Reflection;

using static WmcSoft.Collections.Generic.EqualityComparer;

namespace WmcSoft.Collections.Generic
{
    /// <summary>
    /// Utility to evaluate equality of two instances based on the value of their fields.
    /// </summary>
    /// <typeparam name="T">The type of the instances.</typeparam>
    public sealed class FieldwiseEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly IList<FieldInfo> _fields;

        public FieldwiseEqualityComparer(IList<FieldInfo> fields)
        {
            _fields = fields;
        }

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            if (x.GetType() != y.GetType()) return false;

            foreach (var field in _fields) {
                if (!object.Equals(field.GetValue(x), field.GetValue(y)))
                    return false;
            }
            return true;
        }

        public int GetHashCode(T obj)
        {
            if (obj == null) return 0;

            var h = 0;
            foreach (var field in _fields) {
                object value = field.GetValue(obj);
                h = CombineHashCodes(h, value != null ? value.GetHashCode() : 0);
            }
            return h;
        }

        #region Factory methods

        public static FieldwiseEqualityComparer<T> FromFields()
        {
            return FromFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        public static FieldwiseEqualityComparer<T> FromFields(BindingFlags bindingFlags)
        {
            var fields = typeof(T).GetFields(bindingFlags);
            if (typeof(T).BaseType == typeof(object))
                return new FieldwiseEqualityComparer<T>(fields);

            var results = new List<FieldInfo>(fields);
            var baseType = typeof(T).BaseType;
            do {
                results.AddRange(baseType.GetFields(bindingFlags));
                baseType = baseType.BaseType;
            } while (baseType != typeof(object));

            return new FieldwiseEqualityComparer<T>(results);
        }

        #endregion
    }
}
