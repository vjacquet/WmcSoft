#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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

 ****************************************************************************
 * Based on <https://html.spec.whatwg.org/multipage/scripting.html#2dcontext>
 ****************************************************************************/

#endregion

using System;

namespace WmcSoft.Canvas
{
    public sealed class Variant<T1, T2, T3>
    {
        public class Visitor
        {
            public virtual void Visit(T1 instance) {
            }
            public virtual void Visit(T2 instance) {
            }
            public virtual void Visit(T3 instance) {
            }
        }

        object _storage;

        public Variant(T1 instance) {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            _storage = instance;
        }
        public Variant(T2 instance) {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            _storage = instance;
        }
        public Variant(T3 instance) {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            _storage = instance;
        }

        public static implicit operator Variant<T1, T2, T3>(T1 instance) {
            return new Variant<T1, T2, T3>(instance);
        }
        public static implicit operator Variant<T1, T2, T3>(T2 instance) {
            return new Variant<T1, T2, T3>(instance);
        }
        public static implicit operator Variant<T1, T2, T3>(T3 instance) {
            return new Variant<T1, T2, T3>(instance);
        }

        public void Visit(Visitor visitor) {
            var type = GetInnerType();
            if (type == typeof(T1))
                visitor.Visit((T1)_storage);
            else if (type == typeof(T2))
                visitor.Visit((T2)_storage);
            else if (type == typeof(T3))
                visitor.Visit((T3)_storage);
            else
                throw new InvalidOperationException();
        }

        public T Cast<T>() {
            if (typeof(T) == typeof(T1) || typeof(T) == typeof(T2) || typeof(T) == typeof(T3))
                throw new InvalidCastException();
            return (T)_storage;
        }

        public Type GetInnerType() {
            return _storage.GetType();
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof(Variant<T1, T2, T3>))
                return false;
            return Equals((Variant<T1, T2, T3>)obj);
        }
        public override int GetHashCode() {
            var h = _storage.GetType().GetHashCode();
            return CombineHashCodes(_storage.GetType().GetHashCode(), _storage.GetHashCode());
        }

        public bool Equals(Variant<T1, T2, T3> other) {
            if (other == null)
                return false;
            return _storage.GetType().Equals(other.GetHashCode()) && _storage.Equals(other._storage);
        }

        internal static int CombineHashCodes(int h1, int h2) {
            return (((h1 << 5) + h1) ^ h2);
        }

        public override string ToString() {
            return _storage.ToString();
        }
    }
}
