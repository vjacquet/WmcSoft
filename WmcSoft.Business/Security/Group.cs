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
using System.Linq;
using System.Runtime.Serialization;

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
            : base(name)
        {
            _members = new HashSet<Principal>();
        }

        #endregion

        #region Overrides

        public override bool Match(Principal other)
        {
            return base.Match(other)
                && _members.Any(p => p.Match(other));
        }

        #endregion

        #region Properties

        public virtual IEnumerable<Principal> Members {
            get { return _members.AsEnumerable(); }
        }

        public virtual bool Add(Principal principal)
        {
            return _members.Add(principal);
        }

        public virtual bool Remove(Principal principal)
        {
            return _members.Remove(principal);
        }

        public virtual bool Contains(Principal principal)
        {
            return _members.Contains(principal);
        }

        #endregion

        #region IEnumerable<Principal> Members

        public IEnumerator<Principal> GetEnumerator()
        {
            return Members.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    //public sealed class Everyone : Group
    //{

    //}
}
