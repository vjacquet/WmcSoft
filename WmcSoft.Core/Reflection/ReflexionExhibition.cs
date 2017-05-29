using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WmcSoft.Reflection
{
    public class ReflectionExhibition
    {
        public interface IVisitor
        {
            void Visit(Type type);
            void Visit(ConstructorInfo constructorInfo);
            void Visit(MethodInfo methodInfo);
            void Visit(ParameterInfo parameterInfo);
            void Visit(FieldInfo fieldInfo);
            void Visit(PropertyInfo propertyInfo);
            void Visit(EventInfo eventInfo);
        }

        public void Exhibit(Type type, IVisitor visitor)
        {
            var members = type.GetMembers();

            visitor.Visit(type);

            Exhibit(type.BaseType, visitor);

            foreach (var interfaceType in type.GetInterfaces()) {
                Exhibit(interfaceType, visitor);
            }

            foreach (var member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
                switch (member) {
                case FieldInfo info:
                    Exhibit(info, visitor);
                    break;
                case ConstructorInfo info:
                    Exhibit(info, visitor);
                    break;
                case MethodInfo info:
                    Exhibit(info, visitor);
                    break;
                case PropertyInfo info:
                    Exhibit(info, visitor);
                    break;
                case EventInfo info:
                    Exhibit(info, visitor);
                    break;
                }
            }

            foreach (var nestedType in type.GetNestedTypes()) {
                Exhibit(nestedType, visitor);
            }
        }

        public void Exhibit(FieldInfo fieldInfo, IVisitor visitor)
        {
            visitor.Visit(fieldInfo);
        }

        public void Exhibit(MethodInfo methodInfo, IVisitor visitor)
        {
            if ((methodInfo.Attributes & MethodAttributes.SpecialName) == 0) {
                visitor.Visit(methodInfo);
                foreach (var parameterInfo in methodInfo.GetParameters()) {
                    visitor.Visit(parameterInfo);
                }
                if (methodInfo.ReturnParameter != null) {
                    visitor.Visit(methodInfo.ReturnParameter);
                }
            }
        }

        public void Exhibit(ConstructorInfo constructorInfo, IVisitor visitor)
        {
            visitor.Visit(constructorInfo);
            foreach (var parameterInfo in constructorInfo.GetParameters()) {
                visitor.Visit(parameterInfo);
            }
        }

        public void Exhibit(ParameterInfo parameterInfo, IVisitor visitor)
        {
            visitor.Visit(parameterInfo);
        }

        public void Exhibit(EventInfo eventInfo, IVisitor visitor)
        {
            visitor.Visit(eventInfo);
        }

        public void Exhibit(PropertyInfo propertyInfo, IVisitor visitor)
        {
            visitor.Visit(propertyInfo);
        }
    }
}
