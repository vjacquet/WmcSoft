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
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using WmcSoft.Properties;

namespace WmcSoft.Reflection
{
    public static class ReflectionHelper
    {
        public static string GetAssemblyName(Assembly assembly) {
            if (assembly == null) {
                throw new ArgumentNullException("assembly");
            }
            return assembly.FullName.Substring(0, assembly.FullName.IndexOf(",")).Trim();
        }

        public static string GetAssemblyName(string qualifiedTypeName) {
            if (qualifiedTypeName == null) {
                throw new ArgumentNullException("qualifiedTypeName");
            }
            string[] strArray = qualifiedTypeName.Split(new char[] { ',' });
            if (strArray.Length < 2) {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidQualifiedName, new object[] { qualifiedTypeName }));
            }
            return strArray[1].Trim();
        }

        public static string GetAssemblyName(Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            return GetAssemblyName(type.Assembly);
        }

        public static string CreateShortQualifiedName(Type value) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }
            return Assembly.CreateQualifiedName(GetAssemblyName(value), value.FullName);
        }

        public static bool IsAssemblyQualifiedTypeName(string typeName) {
            return !String.IsNullOrEmpty(typeName)
                && (typeName.IndexOf(',') >= 0);
        }

        public static string GetAssemblyString(string qualifiedTypeName) {
            if (qualifiedTypeName == null) {
                throw new ArgumentNullException("qualifiedTypeName");
            }
            if (qualifiedTypeName.IndexOf(',') == -1) {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.InvalidQualifiedName, new object[] { qualifiedTypeName }));
            }
            return qualifiedTypeName.Substring(qualifiedTypeName.IndexOf(',') + 1).Trim();
        }

        public static string GetSimpleTypeName(object target) {
            if (target == null) {
                throw new ArgumentNullException("target");
            }
            return GetSimpleTypeName(target.GetType());
        }

        public static string GetSimpleTypeName(Type type) {
            if (type == null) {
                throw new ArgumentNullException("type");
            }
            return (type.FullName + ", " + GetAssemblyName(type));
        }

        public static bool IsAssignableTo(Type targetType, object value) {
            bool flag = false;
            if (value.GetType().IsCOMObject) {
                if (targetType.IsInterface) {
                    IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(value);
                    Guid gUID = targetType.GUID;
                    IntPtr zero = IntPtr.Zero;
                    Marshal.QueryInterface(iUnknownForObject, ref gUID, out zero);
                    flag = zero == IntPtr.Zero;
                    if (!flag) {
                        Marshal.Release(zero);
                    }
                    if (iUnknownForObject != IntPtr.Zero) {
                        Marshal.Release(iUnknownForObject);
                    }
                } else {
                    flag = true;
                }
            } else if ((value.GetType() != targetType) && !targetType.IsAssignableFrom(value.GetType())) {
                flag = true;
            }
            return !flag;
        }

        public static bool IsAssignableTo<T>(object value) {
            return IsAssignableTo(typeof(T), value);
        }

        public static Type LoadType(string className) {
            if (className == null) {
                throw new ArgumentNullException("className");
            }
            return LoadType(className, false);
        }

        public static Type LoadType(string className, bool callingAssembly) {
            if (className == null) {
                throw new ArgumentNullException("className");
            }
            Type type = null;
            if ((className == null) || (className.Length <= 0)) {
                return type;
            }
            if (callingAssembly) {
                type = Assembly.GetCallingAssembly().GetType(className.Trim(), false, true);
            } else {
                type = Type.GetType(className.Trim(), false, true);
            }
            if (type != null) {
                return type;
            }
            string[] strArray = className.Split(new char[] { ',' });
            Assembly assembly = Assembly.Load(GetAssemblyName(className));
            if (assembly == null) {
                throw new TypeLoadException(string.Format(CultureInfo.CurrentCulture, Resources.CantLoadAssembly, new object[] { strArray[1], className }));
            }
            return assembly.GetType(strArray[0].Trim(), true, true);
        }
    }
}
