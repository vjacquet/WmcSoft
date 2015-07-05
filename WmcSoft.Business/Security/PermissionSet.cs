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

 ****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WmcSoft.Security
{
    [DataContract]
    public class PermissionSet : ICollection<Permission>
    {
        #region Fields

        private readonly HashSet<Permission> _permissions;

        #endregion

        #region Lifecycle

        protected PermissionSet(PermissionSet permissions) {
            _permissions = new HashSet<Permission>(permissions);
        }

        public PermissionSet(params Permission[] permissions) {
            _permissions = new HashSet<Permission>(permissions);
        }

        #endregion

        #region Operators

        public static implicit operator PermissionSet(Permission permission) {
            return new PermissionSet(permission);
        }

        public static PermissionSet operator |(PermissionSet x, PermissionSet y) {
            var r = new PermissionSet(x);
            r._permissions.UnionWith(y);
            return r;
        }
        public static PermissionSet BitwiseOr(PermissionSet x, PermissionSet y) {
            return x | y;
        }

        public static PermissionSet operator &(PermissionSet x, PermissionSet y) {
            var r = new PermissionSet(x);
            r._permissions.IntersectWith(y);
            return r;
        }
        public static PermissionSet BitwiseAnd(PermissionSet x, PermissionSet y) {
            return x & y;
        }

        #endregion

        #region ICollection<Permission> Members

        public void Add(Permission permission) {
            _permissions.Add(permission);
        }

        public void Clear() {
            _permissions.Clear();
        }

        public bool Contains(Permission permission) {
            return _permissions.Contains(permission);
        }

        public void CopyTo(Permission[] array, int arrayIndex) {
            _permissions.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _permissions.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(Permission permission) {
            return _permissions.Remove(permission);
        }

        #endregion

        #region IEnumerable<Permission> Members

        public IEnumerator<Permission> GetEnumerator() {
            return _permissions.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _permissions.GetEnumerator();
        }

        #endregion
    }
}
