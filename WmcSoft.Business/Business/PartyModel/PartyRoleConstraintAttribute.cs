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

namespace WmcSoft.Business.PartyModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class PartyRoleConstraintAttribute : Attribute
    {
        public static readonly PartyRoleConstraintAttribute Default = new PartyRoleConstraintAttribute();

        private readonly string _typeName;

        [NonSerialized]
        private Type _type;

        protected PartyRoleConstraintAttribute() {
            _typeName = String.Empty;
        }

        public PartyRoleConstraintAttribute(string typeName) {
        }

        public PartyRoleConstraintAttribute(Type type) {
            _type = type;
            _typeName = type.AssemblyQualifiedName;
        }

        public virtual bool CanPlayRole(Party party) {
            if (_type == null) {
                _type = Type.GetType(_typeName, true);
            }
            return _type.IsAssignableFrom(party.GetType());
        }
    }
}
