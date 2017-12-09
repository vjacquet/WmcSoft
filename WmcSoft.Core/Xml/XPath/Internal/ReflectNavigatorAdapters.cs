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
using System.Xml;
using System.Reflection;

namespace WmcSoft.Xml.XPath.Internal
{
    internal abstract class TypeAdapter : ElementNavigationAdapter
    {
        protected Type type;

        public TypeAdapter(Type type, NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
            this.type = type;

            int index = 0;

            // add base class
            string inherits;
            if (!type.IsInterface) {
                if (type.BaseType != typeof(object))
                    this.childNodes.Add(new InheritedTypeAdapter("inherits", type.BaseType, this, index++));
                inherits = "implements";
            } else {
                inherits = "inherits";
            }

            // add interfaces
            foreach (Type interfaceType in type.GetInterfaces()) {
                this.childNodes.Add(new InheritedTypeAdapter(inherits, interfaceType, this, index++));
            }

            foreach (MemberInfo info in type.GetMembers(BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Static
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly)) {
                NavigationAdapter adapter = ReflectNavigator.CreateAdapter(info, this, index);
                if (adapter != null) {
                    this.childNodes.Add(adapter);
                    ++index;
                }
            }

            /*foreach (Type nestedType in type.GetNestedTypes()) {
                Show(nestedType, visitor);
            }*/
        }

        public override int GetAttributeCount()
        {
            return 3;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("name");
            case 1:
                return nameTable.Get("fullName");
            case 2:
                return nameTable.Get("access");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return type.Name;
            case 1:
                return type.FullName;
            case 2:
                return GetModifiersAttribute(nameTable);
            }
            return string.Empty;
        }

        protected string GetModifiersAttribute(XmlNameTable nameTable)
        {
            if (type.IsPublic)
                return nameTable.Get("public");
            return nameTable.Get("private");
        }
    }

    internal class InheritedTypeAdapter : TypeAdapter
    {
        string elementName;
        public InheritedTypeAdapter(string elementName, Type type, NavigationAdapter parent, int indexInParent)
            : base(type, parent, indexInParent)
        {
            this.elementName = elementName;
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get(elementName);
        }
    }

    internal class EnumAdapter : TypeAdapter
    {
        internal string[] names;

        public EnumAdapter(Type type, NavigationAdapter parent, int indexInParent)
            : base(type, parent, indexInParent)
        {
            names = Enum.GetNames(type);
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("enum");
        }

        public override int GetChildCount()
        {
            return names.Length;
        }

        public override NavigationAdapter GetChild(int index)
        {
            if (index < 0 || index > names.Length)
                return null;
            return new EnumNameAdapter(this, index);
        }
    }

    internal class EnumNameAdapter : NavigationAdapter
    {
        public EnumNameAdapter(NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
        }

        public override int GetAttributeCount()
        {
            return 1;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("name");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Add(((EnumAdapter)parent).names[indexInParent]);
            }
            return string.Empty;
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("entry");
        }

        public override int GetChildCount()
        {
            return 0;
        }

        public override bool IsEmptyElement()
        {
            return true;
        }
    }

    internal class InterfaceAdapter : TypeAdapter
    {
        public InterfaceAdapter(Type type, NavigationAdapter parent, int indexInParent)
            : base(type, parent, indexInParent)
        {
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("interface");
        }
    }

    internal class StructAdapter : InterfaceAdapter
    {
        public StructAdapter(Type type, NavigationAdapter parent, int indexInParent)
            : base(type, parent, indexInParent)
        {
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("struct");
        }
    }

    internal class ClassAdapter : InterfaceAdapter
    {
        public ClassAdapter(Type type, NavigationAdapter parent, int indexInParent)
            : base(type, parent, indexInParent)
        {
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("class");
        }

        public override int GetAttributeCount()
        {
            return 5;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 3:
                return nameTable.Get("isAbstract");
            case 4:
                return nameTable.Get("isSealed");
            }
            return base.GetAttributeName(nameTable);
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 3:
                return nameTable.Get(type.IsAbstract ? "true" : "false");
            case 4:
                return nameTable.Get(type.IsSealed ? "true" : "false");
            }
            return base.GetValue(nameTable);
        }
    }

    internal class FieldAdapter : NavigationAdapter
    {
        FieldInfo fieldInfo;

        public FieldAdapter(FieldInfo fieldInfo, NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
            this.fieldInfo = fieldInfo;
        }

        public override int GetAttributeCount()
        {
            return 6;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("name");
            case 1:
                return nameTable.Get("type");
            case 2:
                return nameTable.Get("access");
            case 3:
                return nameTable.Get("isStatic");
            case 4:
                return nameTable.Get("isReadOnly");
            case 5:
                return nameTable.Get("metadataToken");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Add(fieldInfo.Name);
            case 1:
                return nameTable.Add(fieldInfo.FieldType.FullName);
            case 2:
                if (fieldInfo.IsFamilyOrAssembly)
                    return nameTable.Get("protected internal");
                else if (fieldInfo.IsFamilyAndAssembly)
                    return nameTable.Get("internal");
                else if (fieldInfo.IsFamily)
                    return nameTable.Get("protected");
                else if (fieldInfo.IsPublic)
                    return nameTable.Get("public");
                return nameTable.Get("private");
            case 3:
                return nameTable.Get(fieldInfo.IsStatic ? "true" : "false");
            case 4:
                return nameTable.Get(fieldInfo.IsInitOnly ? "true" : "false");
            case 5:
                return fieldInfo.MetadataToken.ToString("x");
            }
            return string.Empty;
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("field");
        }

        public override int GetChildCount()
        {
            return 0;
        }

        public override bool IsEmptyElement()
        {
            return true;
        }
    }

    internal class PropertyAdapter : ElementNavigationAdapter
    {
        PropertyInfo propertyInfo;

        public PropertyAdapter(PropertyInfo propertyInfo, NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
            this.propertyInfo = propertyInfo;

            int index = 0;
            if (propertyInfo.CanRead) {
                this.childNodes.Add(new SpecialMethodAdapter("get", propertyInfo.GetGetMethod(true), this, index++));
            }
            if (propertyInfo.CanWrite) {
                this.childNodes.Add(new SpecialMethodAdapter("set", propertyInfo.GetSetMethod(true), this, index++));
            }
        }

        public override int GetAttributeCount()
        {
            return 3;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("name");
            case 1:
                return nameTable.Get("type");
            case 2:
                return nameTable.Get("metadataToken");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Add(propertyInfo.Name);
            case 1:
                return nameTable.Add(propertyInfo.PropertyType.FullName);
            case 2:
                return propertyInfo.MetadataToken.ToString("x");
            }
            return string.Empty;
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("property");
        }

    }

    internal class ParameterAdapter : NavigationAdapter
    {
        ParameterInfo parameterInfo;

        public ParameterAdapter(ParameterInfo parameterInfo, NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
            this.parameterInfo = parameterInfo;
        }

        public override int GetAttributeCount()
        {
            return 2;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("name");
            case 1:
                return nameTable.Get("type");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Add(parameterInfo.Name);
            case 1:
                return nameTable.Add(parameterInfo.ParameterType.FullName);
            }
            return string.Empty;
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("parameter");
        }

        public override int GetChildCount()
        {
            return 0;
        }

        public override bool IsEmptyElement()
        {
            return true;
        }
    }

    internal class ReturnAdapter : ParameterAdapter
    {
        public ReturnAdapter(ParameterInfo parameterInfo, NavigationAdapter parent, int indexInParent)
            : base(parameterInfo, parent, indexInParent)
        {
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("return");
        }
    }

    internal class ConstructorAdapter : ElementNavigationAdapter
    {
        protected MethodBase method;

        public ConstructorAdapter(MethodBase method, NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
            this.method = method;

            int index = 0;
            foreach (ParameterInfo parameterInfo in method.GetParameters()) {
                this.childNodes.Add(new ParameterAdapter(parameterInfo, this, index++));
            }
        }

        public override int GetAttributeCount()
        {
            return 2;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("access");
            case 1:
                return nameTable.Get("metadataToken");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                if (method.IsFamilyOrAssembly)
                    return nameTable.Get("protected internal");
                else if (method.IsFamilyAndAssembly)
                    return nameTable.Get("internal");
                else if (method.IsFamily)
                    return nameTable.Get("protected");
                else if (method.IsPublic)
                    return nameTable.Get("public");
                return nameTable.Get("private");
            case 1:
                return method.MetadataToken.ToString("x");
            }
            throw new InvalidOperationException();
        }
    }

    internal class MethodAdapter : ConstructorAdapter
    {
        public MethodAdapter(MethodBase method, NavigationAdapter parent, int indexInParent)
            : base(method, parent, indexInParent)
        {
            var methodInfo = (MethodInfo)method;
            if (methodInfo.ReturnType != typeof(void)) {
                this.childNodes.Add(new ReturnAdapter(methodInfo.ReturnParameter, this, this.childNodes.Count));
            }
        }

        public override int GetAttributeCount()
        {
            return 8;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 2:
                return nameTable.Get("name");
            case 3:
                return nameTable.Get("access");
            case 4:
                return nameTable.Get("isAbstract");
            case 5:
                return nameTable.Get("isSealed");
            case 6:
                return nameTable.Get("isStatic");
            case 7:
                return nameTable.Get("isVirtual");
            }
            return base.GetAttributeName(nameTable);
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 2:
                return method.Name;
            case 3:
                if (method.IsFamilyOrAssembly)
                    return nameTable.Get("protected internal");
                else if (method.IsFamilyAndAssembly)
                    return nameTable.Get("internal");
                else if (method.IsFamily)
                    return nameTable.Get("protected");
                else if (method.IsPublic)
                    return nameTable.Get("public");
                return nameTable.Get("private");
            case 4:
                return nameTable.Get(method.IsAbstract ? "true" : "false");
            case 5:
                return nameTable.Get(method.IsFinal ? "true" : "false");
            case 6:
                return nameTable.Get(method.IsStatic ? "true" : "false");
            case 7:
                return nameTable.Get(method.IsVirtual ? "true" : "false");
            }
            return base.GetValue(nameTable);
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("method");
        }
    }

    internal class SpecialMethodAdapter : MethodAdapter
    {
        string specialName;

        public SpecialMethodAdapter(string specialName, MethodInfo methodInfo, NavigationAdapter parent, int indexInParent)
            : base(methodInfo, parent, indexInParent)
        {
            this.specialName = specialName;
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get(specialName);
        }
    }

    internal class EventAdapter : ElementNavigationAdapter
    {
        EventInfo eventInfo;

        public EventAdapter(EventInfo eventInfo, NavigationAdapter parent, int indexInParent)
            : base(parent, indexInParent)
        {
            this.eventInfo = eventInfo;

            int index = 0;
            MethodInfo methodInfo;
            if ((methodInfo = eventInfo.GetAddMethod(true)) != null) {
                this.childNodes.Add(new SpecialMethodAdapter("add", methodInfo, this, index++));
            }
            if ((methodInfo = eventInfo.GetRemoveMethod(true)) != null) {
                this.childNodes.Add(new SpecialMethodAdapter("remove", methodInfo, this, index++));
            }
        }

        public override string GetElementName(XmlNameTable nameTable)
        {
            return nameTable.Get("event");
        }

        public override int GetAttributeCount()
        {
            return 1;
        }

        public override string GetAttributeName(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return nameTable.Get("name");
            }
            throw new InvalidOperationException();
        }

        public override string GetValue(XmlNameTable nameTable)
        {
            switch (attributeIndex) {
            case 0:
                return eventInfo.Name;
            }
            throw new InvalidOperationException();
        }
    }
}
