using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    [DataContract]
    [KnownType(typeof(Group))]
    [KnownType(typeof(User))]
    [DebuggerDisplay("User: {Name, nq}")]
    public abstract class Principal: IEquatable<Principal>
    {
        protected Principal(string name) {
            Name = name;
        }

        [DataMember]
        public string Name { get; private set; }

        public virtual bool Match(Principal other) {
            return Equals(other);
        }

        public override string ToString() {
            return Name;
        }
        public override int GetHashCode() {
            return Name.GetHashCode();
        }

        #region IEquatable<Principal> Membres

        public bool Equals(Principal other) {
            if(other == null)
                return false;
            return GetType() == other.GetType()
                && Name == other.Name;
        }

        #endregion
    }
}
