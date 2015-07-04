using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
