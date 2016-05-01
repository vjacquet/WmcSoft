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
    public abstract class ReflectionVisitor
    {
        #region Accept

        public void Accept(Assembly assembly) {
            Visit(assembly);
            foreach (Module module in assembly.GetModules()) {
                Accept(module);
            }
        }

        public void Accept(Module module) {
            Visit(module);
            foreach (Type type in module.GetTypes()) {
                Accept(type);
            }
        }

        public void Accept(Type type) {
            Visit(type);
            if (type != typeof(object))
                Accept(type.BaseType);

            foreach (Type interfaceType in type.GetInterfaces()) {
                Accept(interfaceType);
            }

            foreach (MemberInfo info in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)) {
                if (info is FieldInfo)
                    Accept((FieldInfo)info);
                else if (info is ConstructorInfo)
                    Accept((ConstructorInfo)info);
                else if (info is MethodInfo) {
                    MethodInfo methodInfo = (MethodInfo)info;
                    if (!methodInfo.IsConstructor
                        && !methodInfo.IsSpecialName) {
                        Accept((MethodInfo)info);
                    }
                } else if (info is PropertyInfo)
                    Accept((PropertyInfo)info);
                else if (info is EventInfo)
                    Accept((EventInfo)info);
            }

            foreach (Type nestedType in type.GetNestedTypes()) {
                Accept(nestedType);
            }
        }

        public void Accept(ConstructorInfo constructorInfo) {
            Visit(constructorInfo);
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters()) {
                Accept(parameterInfo);
            }
        }

        public void Accept(MethodInfo methodInfo) {
            Visit(methodInfo);
            if ((methodInfo.Attributes & MethodAttributes.SpecialName) == 0) {
                foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                    Accept(parameterInfo);
                }
                if (methodInfo.ReturnParameter != null) {
                    Accept(methodInfo.ReturnParameter);
                }
            }
        }

        public void Accept(ParameterInfo parameterInfo) {
            Visit(parameterInfo);
        }

        public void Accept(FieldInfo fieldInfo) {
            Visit(fieldInfo);
        }

        public void Accept(PropertyInfo propertyInfo) {
            Visit(propertyInfo);
        }

        public void Accept(EventInfo eventInfo) {
            Visit(eventInfo);
        }

        #endregion

        #region Visit methods

        protected virtual void Visit(Assembly assembly) {
        }

        protected virtual void Visit(Module module) {
        }

        protected virtual void Visit(Type type) {
        }

        protected virtual void Visit(ConstructorInfo constructorInfo) {
        }

        protected virtual void Visit(MethodInfo methodInfo) {
        }

        protected virtual void Visit(ParameterInfo parameterInfo) {
        }

        protected virtual void Visit(FieldInfo fieldInfo) {
        }

        protected virtual void Visit(PropertyInfo propertyInfo) {
        }

        protected virtual void Visit(EventInfo eventInfo) {
        }

        #endregion
    }

}
