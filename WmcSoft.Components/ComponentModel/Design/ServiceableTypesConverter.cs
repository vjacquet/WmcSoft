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
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
    internal class ServiceableTypesConverter : TypeConverter
    {
        #region TypeComparer class

        class TypeComparer : IComparer<Type>
        {
            #region IComparer<Type> Membres

            public int Compare(Type x, Type y) {
                return String.Compare(x.AssemblyQualifiedName, y.AssemblyQualifiedName, true);
            }

            #endregion
        }

        #endregion

        #region Private fields

        static IComparer<Type> comparer = new TypeComparer();

        static Type[] excludedTypes;
        static Type[] excludedInterfaceTypes;

        static ServiceableTypesConverter() {
            excludedTypes = new Type[] { 
                typeof(Object), 
                typeof(Component), 
                typeof(MarshalByRefObject), 
                typeof(MarshalByValueComponent), 
            };
            Array.Sort(excludedTypes, comparer);
            excludedInterfaceTypes = new Type[] { 
                typeof(IComponent), 
                typeof(IContainer), 
                typeof(IServiceProvider), 
                typeof(IDisposable), 
            };
            Array.Sort(excludedInterfaceTypes, comparer);
        }

        #endregion

        #region Methods

        IEnumerable<Type> GetTypes(ITypeDescriptorContext context) {
            if (context != null && context.Instance != null) {
                var type = context.Instance.GetType();
                var baseType = type;
                while (Array.BinarySearch(excludedTypes, baseType, comparer) < 0) {
                    yield return baseType;
                    baseType = baseType.BaseType;
                }
                foreach (Type interfaceType in type.GetInterfaces()) {
                    if (Array.BinarySearch(excludedInterfaceTypes, interfaceType, comparer) < 0)
                        yield return interfaceType;
                }
            }
        }

        #endregion

        #region Overridables

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (value is string) {
                if (String.IsNullOrEmpty((string)value))
                    return null;
                foreach (Type type in GetTypes(context)) {
                    if (value.Equals(type.FullName)) {
                        return type;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType == null) {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string)) {
                if (value == null) {
                    return "";
                }
                return ((Type)value).FullName;
            }
            if ((destinationType == typeof(InstanceDescriptor)) && (value is Type)) {
                MethodInfo method = typeof(Type).GetMethod("GetType", new Type[] { typeof(string) });
                if (method != null) {
                    return new InstanceDescriptor(method, new object[] { ((Type)value).AssemblyQualifiedName });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            List<Type> types = new List<Type>(GetTypes(context));
            return new TypeConverter.StandardValuesCollection(types);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }

        #endregion
    }
}
