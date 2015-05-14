using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WmcSoft.Reflection
{
    public abstract class ReflectionVisitor
    {

        public virtual void Visit(Assembly assembly) {
            foreach (Module module in assembly.GetModules()) {
                Visit(module);
            }
        }

        public virtual void Visit(Module module) {
            foreach (Type type in module.GetTypes()) {
                Visit(type);
            }
        }

        public virtual void Visit(Type type) {
            if (type != typeof(Object))
                Visit(type.BaseType);

            foreach (Type interfaceType in type.GetInterfaces()) {
                Visit(interfaceType);
            }

            foreach (MemberInfo info in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
                if (info is FieldInfo)
                    Visit((FieldInfo)info);
                else if (info is ConstructorInfo)
                    Visit((ConstructorInfo)info);
                else if (info is MethodInfo) {
                    MethodInfo methodInfo = (MethodInfo)info;
                    if (!methodInfo.IsConstructor
                        && !methodInfo.IsSpecialName) {
                        Visit((MethodInfo)info);
                    }
                } else if (info is PropertyInfo)
                    Visit((PropertyInfo)info);
                else if (info is EventInfo)
                    Visit((EventInfo)info);
            }

            foreach (Type nestedType in type.GetNestedTypes()) {
                Visit(nestedType);
            }
        }

        public virtual void Visit(ConstructorInfo constructorInfo) {
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters()) {
                Visit(parameterInfo);
            }
        }

        public virtual void Visit(MethodInfo methodInfo) {
            if ((methodInfo.Attributes & MethodAttributes.SpecialName) == 0) {
                foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                    Visit(parameterInfo);
                }
                if (methodInfo.ReturnParameter != null) {
                    Visit(methodInfo.ReturnParameter);
                }
            }
        }

        public virtual void Visit(ParameterInfo parameterInfo) {
        }

        public virtual void Visit(FieldInfo fieldInfo) {
        }

        public virtual void Visit(PropertyInfo propertyInfo) {
        }

        public virtual void Visit(EventInfo eventInfo) {
        }
    }

}
