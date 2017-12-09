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
using System.Reflection;

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
