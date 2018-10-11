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
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace WmcSoft.Configuration
{
    [DebuggerDisplay("{Name,nq} => {Type,nq}")]
    public abstract class FactoryElement<T> : ConfigurationElement
    {
        private readonly NameValueCollection parameters = new NameValueCollection(StringComparer.Ordinal);

        protected override ConfigurationPropertyCollection Properties {
            get {
                var properties = base.Properties;
                if (!properties.Contains("name"))
                    properties.Add(new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey));
                if (!properties.Contains("type"))
                    properties.Add(new ConfigurationProperty("type", typeof(Type), typeof(T), new TypeNameConverter(), new SubclassTypeValidator(typeof(T)), ConfigurationPropertyOptions.IsRequired));
                return properties;
            }
        }

        public string Name {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        public Type Type {
            get { return (Type)this["type"]; }
            set { this["type"] = value; }
        }

        public NameValueCollection Parameters => parameters;

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            Parameters[name] = value;
            return true;
        }

        public virtual T CreateInstance(params object[] args)
        {
            var type = Type;
            return (T)Activator.CreateInstance(type, args);
        }
    }
}
