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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WmcSoft.Security
{
    [DataContract]
    [KnownType(typeof(RequiredLevelPermission))]
    [KnownType(typeof(AutorizeLevelPermission))]
    public class Permission : IEquatable<Permission>, IEnumerable<Permission>
    {
        public Permission(string name) {
            Name = name;
        }

        [DataMember]
        public string Name { get; protected set; }

        #region Operators

        public static PermissionSet operator |(Permission x, Permission y) {
            var r = new PermissionSet(x);
            return r | y;
        }
        public static PermissionSet BitwiseOr(Permission x, Permission y) {
            return x | y;
        }

        public static PermissionSet operator &(Permission x, Permission y) {
            var r = new PermissionSet(x);
            return r & y;
        }
        public static PermissionSet BitwiseAnd(Permission x, Permission y) {
            return x & y;
        }

        #endregion

        #region Overrides

        public override string ToString() {
            return Name;
        }

        public override int GetHashCode() {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name);
        }

        public override bool Equals(object obj) {
            return Equals(obj as Permission);
        }

        #endregion

        #region IEquatable<Permission> Members

        public bool Equals(Permission other) {
            if (other == null)
                return false;
            return String.Compare(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        #endregion

        #region IEnumerable<Permission> Membres

        IEnumerator<Permission> IEnumerable<Permission>.GetEnumerator() {
            yield return this;
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((IEnumerable<Permission>)this).GetEnumerator();
        }

        #endregion
    }

    [DataContract]
    public class RequiredLevelPermission : Permission
    {
        public RequiredLevelPermission(string name, int level = 1)
            : base(name) {
            Level = level;
        }

        [DataMember]
        public int Level { get; private set; }

        public RequiredLevelPermission Merge(RequiredLevelPermission other) {
            return new RequiredLevelPermission(Name, Math.Max(Level, other.Level));
        }
    }

    [DataContract]
    public class AutorizeLevelPermission : Permission
    {
        public AutorizeLevelPermission(string name, int level = 1)
            : base(name) {
            Level = level;
        }

        [DataMember]
        public int Level { get; private set; }

        public AutorizeLevelPermission Merge(AutorizeLevelPermission other) {
            return new AutorizeLevelPermission(Name, Math.Min(Level, other.Level));
        }
    }
}
