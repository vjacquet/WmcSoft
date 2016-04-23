using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WmcSoft.Collections.Generic;
using WmcSoft.ComponentModel;

namespace WmcSoft.Reflection
{
    public static class ReflectionExtensions
    {
        #region EnumerateDefinitions

        public static IEnumerable<MethodInfo> EnumerateDefinitions(this MethodInfo method) {
            var baseDefinition = method.GetBaseDefinition();
            var parameters = method.GetParameters().ToArray(p => p.ParameterType);
            while (method != baseDefinition) {
                yield return method;
                method = method.DeclaringType.BaseType.GetMethod(method.Name, parameters);
            }
            yield return baseDefinition;
        }

        #endregion

        #region GetCustomAttributes / GetMetadataAttribute

        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = true) where TAttribute : Attribute {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).OfType<TAttribute>().FirstOrDefault();
        }
        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute {
            return type.GetCustomAttributes(typeof(TAttribute), inherit).OfType<TAttribute>().FirstOrDefault();
        }

        public static TAttribute GetMetadataAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute {
            return TypeDescriptor.GetProperties(memberInfo.DeclaringType)[memberInfo.Name].GetMetadataAttribute<TAttribute>();
        }

        public static TAttribute[] GetCustomAttributes<TAttribute>(this MemberInfo element, bool inherit = true) where TAttribute : Attribute {
            return (TAttribute[])Attribute.GetCustomAttributes(element, typeof(TAttribute), inherit);
        }
        public static TAttribute[] GetCustomAttributes<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute {
            return (TAttribute[])Attribute.GetCustomAttributes(type, typeof(TAttribute), inherit);
        }

        #endregion

        #region GetValue

        public static T GetValue<T>(this PropertyInfo property, object obj) {
            var value = property.GetValue(obj);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static T GetValue<T>(this PropertyInfo property, object obj, IFormatProvider provider) {
            var value = property.GetValue(obj);
            return (T)Convert.ChangeType(value, typeof(T), provider);
        }

        public static T GetPropertyValue<T>(this Type type, object obj, string propertyName) {
            var property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentOutOfRangeException("propertyName");
            return property.GetValue<T>(obj);
        }

        public static T GetPropertyValue<T>(this Type type, object obj, string propertyName, IFormatProvider provider) {
            var property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentOutOfRangeException("propertyName");
            return property.GetValue<T>(obj, provider);
        }

        #endregion
    }
}
