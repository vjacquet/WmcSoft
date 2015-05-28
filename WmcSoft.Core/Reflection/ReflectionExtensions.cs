using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Collections.Generic;

namespace WmcSoft.Reflection
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<MethodInfo> EnumerateDefinitions(this MethodInfo method) {
            var baseDefinition = method.GetBaseDefinition();
            var parameters = method.GetParameters().ToArray(p => p.ParameterType);
            while (method != baseDefinition) {
                yield return method;
                method = method.DeclaringType.BaseType.GetMethod(method.Name, parameters);
            }
            yield return baseDefinition;
        }

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
    }
}
