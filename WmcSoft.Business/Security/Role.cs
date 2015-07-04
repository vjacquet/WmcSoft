using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    [DataContract]
    public class Role : IEnumerable<Permission>
    {
        #region Fields

        private readonly HashSet<Permission> _permissions;

        #endregion

        #region Lifecycle

        public Role(string name, params Permission[] permissions) {
            Name = name;
            _permissions = new HashSet<Permission>(permissions);
        }

        public Role(string name, IEnumerable<Permission> permissions) {
            Name = name;
            _permissions = new HashSet<Permission>(permissions);
        }

        #endregion

        #region Properties

        [DataMember]
        public string Name { get; private set; }

        #endregion

        #region Methods

        public bool Add(Permission permission) {
            return _permissions.Add(permission);
        }

        public bool Remove(Permission permission) {
            return _permissions.Remove(permission);
        }

        public bool Contains(Permission permission) {
            return _permissions.Contains(permission);
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
