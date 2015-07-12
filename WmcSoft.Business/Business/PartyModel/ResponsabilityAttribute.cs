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
    /// <summary>
    /// Registers the mandatory or optinal responsabilities on the PartyRole type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    class ResponsabilityAttribute : Attribute
    {
        public static readonly ResponsabilityAttribute Default = new ResponsabilityAttribute();

        private readonly string _typeName;
        private readonly RegistrationPolicy _policy;

        [NonSerialized]
        private Type _type;
        [NonSerialized]
        private Responsability _responsability;

        protected ResponsabilityAttribute() {
            _typeName = String.Empty;
        }

        public ResponsabilityAttribute(string typeName, RegistrationPolicy policy = RegistrationPolicy.Optional) {
            _typeName = typeName;
            _policy = policy;
        }

        public ResponsabilityAttribute(Type type, RegistrationPolicy policy = RegistrationPolicy.Optional) {
            _type = type;
            _typeName = type.AssemblyQualifiedName;
            _policy = policy;
        }

        public Responsability Responsability {
            get {
                if (_responsability == null) {
                    if (_type == null) {
                        _type = Type.GetType(_typeName, true);
                    }
                    _responsability = (Responsability)Activator.CreateInstance(_type);
                }
                return _responsability;
            }
        }

        public RegistrationPolicy Policy {
            get { return _policy; }
        }
    }
}
