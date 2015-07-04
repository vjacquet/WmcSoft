using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WmcSoft.Security
{
    [DataContract]
    public class Group : Principal, IEnumerable<Principal>
    {
        #region Fields

        private HashSet<Principal> _members;

        #endregion

        #region Lifecycle

        public Group(string name)
            : base(name) {
            _members = new HashSet<Principal>();
        }

        #endregion

        #region Overrides

        public override bool Match(Principal other) {
            return base.Match(other)
                && _members.Any(p => p.Match(other));
        }

        #endregion

        #region Properties

        public virtual IEnumerable<Principal> Members {
            get { return _members.AsEnumerable(); }
        }

        public virtual bool Add(Principal principal) {
            return _members.Add(principal);
        }

        public virtual bool Remove(Principal principal) {
            return _members.Remove(principal);
        }

        public virtual bool Contains(Principal principal) {
            return _members.Contains(principal);
        }

        #endregion

        #region IEnumerable<Principal> Members

        public IEnumerator<Principal> GetEnumerator() {
            return Members.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }

    //public sealed class Everyone : Group
    //{

    //}
}
