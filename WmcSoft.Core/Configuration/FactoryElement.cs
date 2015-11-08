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
using System.Configuration;
using System.Diagnostics;

namespace WmcSoft.Configuration
{
    [DebuggerDisplay("{Name,nq} => {TypeName,nq}")]
    public abstract class FactoryElement<T> : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string TypeName {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        Type ResolveType() {
            return Type.GetType(TypeName, true, true);
        }

        public T CreateInstance() {
            var type = ResolveType();
            return (T)Activator.CreateInstance(type);
        }

        public T CreateInstance(params object[] args) {
            var type = ResolveType();
            return (T)Activator.CreateInstance(type, args);
        }
    }
}
