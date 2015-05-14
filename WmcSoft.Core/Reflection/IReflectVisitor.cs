using System;
using System.Reflection;

namespace WmcSoft.Reflection
{
    [Obsolete("Derive from ReflectionVisitor instead.", true)]
    public interface IReflectVisitor
    {
        void Visit(Assembly assembly);
        void Visit(Module module);
        void Visit(Type type);
        void Visit(ConstructorInfo constructorInfo);
        void Visit(MethodInfo methodInfo);
        void Visit(ParameterInfo parameterInfo);
        void Visit(FieldInfo fieldInfo);
        void Visit(PropertyInfo propertyInfo);
        void Visit(EventInfo eventInfo);
    }
}
