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
    }
}
