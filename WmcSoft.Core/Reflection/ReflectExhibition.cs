using System;
using System.Reflection;

namespace WmcSoft.Reflection
{
    [Obsolete("Use ReflectionVisitor instead.", true)]
    public class ReflectExhibition
    {
        public void Show(Assembly assembly, IReflectVisitor visitor) {
            visitor.Visit(assembly);
            foreach (Module module in assembly.GetModules()) {
                Show(module, visitor);
            }
        }

        public void Show(Module module, IReflectVisitor visitor) {
            visitor.Visit(module);
            foreach (Type type in module.GetTypes()) {
                Show(type, visitor);
            }
        }

        public void Show(Type type, IReflectVisitor visitor) {
            visitor.Visit(type);

            Show(type.BaseType, visitor);

            foreach (Type interfaceType in type.GetInterfaces()) {
                Show(interfaceType, visitor);
            }

            foreach (MemberInfo info in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
                if (info is FieldInfo)
                    Show((FieldInfo)info, visitor);
                else if (info is ConstructorInfo)
                    Show((ConstructorInfo)info, visitor);
                else if (info is MethodInfo) {
                    MethodInfo methodInfo = (MethodInfo)info;
                    if (!methodInfo.IsConstructor
                        && !methodInfo.IsSpecialName) {
                        Show((MethodInfo)info, visitor);
                    }
                }
                else if (info is PropertyInfo)
                    Show((PropertyInfo)info, visitor);
                else if (info is EventInfo)
                    Show((EventInfo)info, visitor);
            }

            foreach (Type nestedType in type.GetNestedTypes()) {
                Show(nestedType, visitor);
            }
        }

        public void Show(FieldInfo fieldInfo, IReflectVisitor visitor) {
            visitor.Visit(fieldInfo);
        }

        public void Show(MethodInfo methodInfo, IReflectVisitor visitor) {
            if ((methodInfo.Attributes & MethodAttributes.SpecialName) == 0) {
                visitor.Visit(methodInfo);
                foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                    visitor.Visit(parameterInfo);
                }
                if (methodInfo.ReturnParameter != null) {
                    visitor.Visit(methodInfo.ReturnParameter);
                }
            }
        }

        public void Show(ConstructorInfo constructorInfo, IReflectVisitor visitor) {
            visitor.Visit(constructorInfo);
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters()) {
                visitor.Visit(parameterInfo);
            }
        }

        public void Show(ParameterInfo parameterInfo, IReflectVisitor visitor) {
            visitor.Visit(parameterInfo);
        }

        public void Show(EventInfo eventInfo, IReflectVisitor visitor) {
            visitor.Visit(eventInfo);
        }

        public void Show(PropertyInfo propertyInfo, IReflectVisitor visitor) {
            visitor.Visit(propertyInfo);
        }
    }
}
