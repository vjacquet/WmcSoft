// Stephen Toub
// stoub@microsoft.com
//
// XmlComments.cs
// Retrieve the xml comments stored in the assembly's comments file
// for specific types or members of types.
// ==========================================================================
// <mailto:vjacquet@club-internet.fr>
// * Added more paths in to look for the assembly(s documentation.
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

namespace WmcSoft.Reflection
{
    public sealed class DocumentationLoader
    {
        private Dictionary<Assembly, XmlDocument> assemblyDocumentations = new Dictionary<Assembly, XmlDocument>();

        public Dictionary<Assembly, XmlDocument> AssemblyDocumentations {
            get { return this.assemblyDocumentations; }
        }

        /// <summary>
        /// Hashtable, indexed by Type, of all the accessors for assembly type.  Each entry is assembly Hashtable, 
        /// indexed by MethodInfo, that returns the MemberInfo for assembly given MethodInfo accessor.
        /// </summary>
        private Hashtable _typeAccessors = new Hashtable();
        /// <summary>Binding flags to use for reflection operations.</summary>
        private static readonly BindingFlags _bindingFlags =
            BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>Return true if the assembly has documentation.</returns>
        public bool HasDocumentation(Assembly assembly) {
            return GetDocumentation(assembly) != null;
        }

        public XmlDocument GetDocumentation(Assembly assembly) {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            XmlDocument document = null;
            if (this.AssemblyDocumentations.TryGetValue(assembly, out document))
                return document;

            // Get xml dom for the assembly's documentation
            lock (((ICollection)AssemblyDocumentations).SyncRoot) {
                if (this.AssemblyDocumentations.TryGetValue(assembly, out document))
                    return document;

                string documentationLocation = DetermineXmlPath(assembly);
                if (documentationLocation == null) {
                    this.AssemblyDocumentations.Add(assembly, null);
                    return null;
                }

                // Load it and store it
                document = new XmlDocument();
                document.Load(documentationLocation);

                this.AssemblyDocumentations.Add(assembly, document);

                return document;
            }
        }

        public XmlDocumentation GetTypeDocumentation(Type type) {
            XmlDocument document = GetDocumentation(type.Assembly);
            if (document == null)
                return new XmlDocumentation(null);

            string xpath = string.Format("//member[@name=\"T:{0}\"]", type.FullName.Replace("+", "."));
            XmlNode node = document.SelectSingleNode(xpath);
            return new XmlDocumentation(node);
        }

        public XmlDocumentation GetMemberDocumentation(MemberInfo memberInfo) {
            // If this is an accessor, get the owner property/event of the accessor.
            if (memberInfo is MethodInfo) {
                MemberInfo owner = IsAccessor((MethodInfo)memberInfo);
                if (owner != null) memberInfo = owner;
            }

            return new XmlDocumentation(GetComments(memberInfo));
        }

        public Type GetEnumType(PropertyInfo property) {
            XmlNode el = this.GetMemberDocumentation(property).AllComments;
            if (el == null)
                return null;
            else {
                XmlElement enumElement = el.SelectSingleNode("enum") as XmlElement;
                if (enumElement == null)
                    return null;
                else {
                    // get cref attribute
                    string enumTypeName = enumElement.GetAttribute("cref");
                    if (String.IsNullOrEmpty(enumTypeName))
                        return null;
                    if (!enumTypeName.StartsWith("T"))
                        return null;

                    enumTypeName = enumTypeName.Substring(2).Trim();
                    // do we have an assembly name ?
                    string assemblyName = enumElement.GetAttribute("assembly-name");
                    if (!string.IsNullOrEmpty(assemblyName)) {
                        enumTypeName = Assembly.CreateQualifiedName(assemblyName, enumTypeName);
                    }

                    return Type.GetType(enumTypeName, false, false);
                }
            }
        }

        /// <summary>Gets the XML comments for the calling method.</summary>
        public XmlDocumentation Current {
            get {
                return GetMemberDocumentation(new StackTrace().GetFrame(1).GetMethod());
            }
        }

        #region Methods

        /// <summary>Gets the path to a valid xml comments file for the assembly.</summary>
        /// <param name="asm">The assembly whose documentation is to be found.</param>
        /// <returns>The path to documentation for an assembly; null if none found.</returns>
        private static string DetermineXmlPath(Assembly asm) {
            // Get a list of locations to examine for the xml
            // 1. The location of the assembly.
            // 2. The runtime directory of the framework.
            // 3. Patch to the runtime directory of the framework
            string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            string[] locations = new string[] {
                asm.Location,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0", Path.GetFileName(asm.CodeBase)),
                runtimeDirectory+ @"en\" + Path.GetFileName(asm.CodeBase),
                Directory.GetParent(runtimeDirectory).Parent.FullName + @"\v2.0.50727\en\" + Path.GetFileName(asm.CodeBase),
            };

            // Checks each path to see if the xml file exists there; if it does, return it.
            foreach (string location in locations) {
                string newPath = Path.ChangeExtension(location, ".xml");
                if (File.Exists(newPath))
                    return newPath;
            }

            // No xml found; return null.
            return null;
        }

        /// <summary>Retrieve the XML comments for a type or a member of a type.</summary>
        /// <param name="mi">The type or member for which comments should be retrieved.</param>
        /// <returns>A string of xml containing the xml comments of the selected type or member.</returns>
        private XmlNode GetComments(MemberInfo mi) {
            Type declType = (mi is Type) ? ((Type)mi) : mi.DeclaringType;
            XmlDocument doc = GetDocumentation(declType.Assembly);
            if (doc == null) return null;
            string xpath;

            // The fullname uses plus signs to separate nested types from their declaring
            // types.  The xml documentation uses dotted-notation.  We need to change
            // from one to the other.
            string typeName = declType.FullName.Replace("+", ".");

            // Based on the member type, get the correct xpath query to lookup the 
            // member's comments in the assembly's documentation.
            switch (mi.MemberType) {
            case MemberTypes.NestedType:
            case MemberTypes.TypeInfo:
                xpath = "//member[@name='T:" + typeName + "']";
                break;

            case MemberTypes.Constructor:
                xpath = "//member[@name='M:" + typeName + "." +
                    "#ctor" + CreateParamsDescription(((ConstructorInfo)mi).GetParameters()) + "']";
                break;

            case MemberTypes.Method:
                xpath = "//member[@name='M:" + typeName + "." +
                    mi.Name + CreateParamsDescription(((MethodInfo)mi).GetParameters());
                if (mi.Name == "op_Implicit" || mi.Name == "op_Explicit") {
                    xpath += "~{" + ((MethodInfo)mi).ReturnType.FullName + "}";
                }
                xpath += "']";
                break;

            case MemberTypes.Property:
                xpath = "//member[@name='P:" + typeName + "." +
                    mi.Name + CreateParamsDescription(((PropertyInfo)mi).GetIndexParameters()) + "']"; // have args when indexers
                break;

            case MemberTypes.Field:
                xpath = "//member[@name='F:" + typeName + "." + mi.Name + "']";
                break;

            case MemberTypes.Event:
                xpath = "//member[@name='E:" + typeName + "." + mi.Name + "']";
                break;

            // Unknown type, nothing to do
            default:
                return null;
            }

            // Get the appropriate node from the document
            return doc.SelectSingleNode(xpath);
        }

        /// <summary>Determines if a MethodInfo represents an accessor.</summary>
        /// <param name="mi">The MethodInfo to check.</param>
        /// <returns>The MemberInfo that represents the property or event if this is an accessor; null, otherwise.</returns>
        private MemberInfo IsAccessor(MethodInfo mi) {
            // Must be a special name in order to be an accessor
            if (!mi.IsSpecialName) return null;

            Hashtable accessors;
            lock (_typeAccessors.SyncRoot) {
                // We cache accessor information to speed things up, so check to see if the array
                // of accessors for this type has already been computed.
                accessors = (Hashtable)_typeAccessors[mi.DeclaringType];
                if (accessors == null) {
                    // Retrieve the accessors for the declaring type
                    _typeAccessors[mi.DeclaringType] = accessors = RetrieveAccessors(mi.DeclaringType);
                }
            }

            // Return the owning property or event for the accessor
            return (MemberInfo)accessors[mi];
        }

        /// <summary>Retrieve all property and event accessors on a given type.</summary>
        /// <param name="t">The type from which the accessors should be retrieved.</param>
        /// <returns>A dictionary of all accessors.</returns>
        private static Hashtable RetrieveAccessors(Type t) {
            // Build up list of accessors to exclude from method list
            Hashtable ht = new Hashtable();

            // Get all property accessors
            foreach (PropertyInfo pi in t.GetProperties(_bindingFlags)) {
                foreach (MethodInfo mi in pi.GetAccessors(true)) ht[mi] = pi;
            }

            // Get all event accessors
            foreach (EventInfo ei in t.GetEvents(_bindingFlags)) {
                MethodInfo addMethod = ei.GetAddMethod(true);
                MethodInfo removeMethod = ei.GetRemoveMethod(true);
                MethodInfo raiseMethod = ei.GetRaiseMethod(true);

                if (addMethod != null) ht[addMethod] = ei;
                if (removeMethod != null) ht[removeMethod] = ei;
                if (raiseMethod != null) ht[raiseMethod] = ei;
            }

            // Return the whole list
            return ht;
        }

        /// <summary>Generates a parameter string used when searching xml comment files.</summary>
        /// <param name="parameters">List of parameters to a member.</param>
        /// <returns>A parameter string used when searching xml comment files.</returns>
        private static string CreateParamsDescription(ParameterInfo[] parameters) {
            StringBuilder paramDesc = new StringBuilder();

            // If there are parameters then we need to construct a list
            if (parameters.Length > 0) {
                // Start the list
                paramDesc.Append("(");

                // For each parameter, append the type of the parameter.
                // Separate all items with commas.
                for (int i = 0; i < parameters.Length; i++) {
                    Type paramType = parameters[i].ParameterType;
                    string paramName = paramType.FullName ?? paramType.Name;

                    // Handle special case where ref parameter ends in & but xml docs use @.
                    // Pointer parameters end in * in both type representation and xml comments representation.
                    if (paramName.EndsWith("&")) paramName = paramName.Substring(0, paramName.Length - 1) + "@";

                    // Handle multidimensional arrays
                    if (paramType.IsArray && paramType.GetArrayRank() > 1) {
                        paramName = paramName.Replace(",", "0:,").Replace("]", "0:]");
                    }

                    // Append the fixed up parameter name
                    paramDesc.Append(paramName);
                    if (i != parameters.Length - 1) paramDesc.Append(",");
                }

                // End the list
                paramDesc.Append(")");
            }

            // Return the parameter list description
            return paramDesc.ToString();
        }

        #endregion
    }



}
/*        public class MemberNamer
        {

            public override string GetMemberName(Member member) {
                using (TextWriter writer = new StringWriter()) {
                    switch (member.NodeType) {
                    case NodeType.Field:
                        writer.Write("F:");
                        WriteField((Field)member, writer);
                        break;
                    case NodeType.Property:
                        writer.Write("P:");
                        WriteProperty((Property)member, writer);
                        break;
                    case NodeType.Method:
                        writer.Write("M:");
                        WriteMethod((Method)member, writer);
                        break;
                    case NodeType.InstanceInitializer:
                        writer.Write("M:");
                        WriteConstructor((InstanceInitializer)member, writer);
                        break;
                    case NodeType.StaticInitializer:
                        writer.Write("M:");
                        WriteStaticConstructor((StaticInitializer)member, writer);
                        break;
                    case NodeType.Event:
                        writer.Write("E:");
                        WriteEvent((Event)member, writer);
                        break;
                    }
                    return (writer.ToString());
                }
            }

            public override string GetNamespaceName(Namespace space) {
                using (TextWriter writer = new StringWriter()) {
                    writer.Write("N:");
                    WriteNamespace(space, writer);
                    return (writer.ToString());
                }
            }

            public override string GetTypeName(TypeNode type) {
                using (TextWriter writer = new StringWriter()) {
                    writer.Write("T:");
                    WriteType(type, writer);
                    return (writer.ToString());
                }
            }

            private static string GetName(Member entity) {
                using (TextWriter writer = new StringWriter()) {
                    TypeNode type = entity as TypeNode;
                    if (type != null) {
                        writer.Write("T:");
                        WriteType(type, writer);
                        return (writer.ToString());
                    }

                    switch (entity.NodeType) {
                    case NodeType.Namespace:
                        writer.Write("N:");
                        WriteNamespace(entity as Namespace, writer);
                        break;
                    case NodeType.Field:
                        writer.Write("F:");
                        WriteField(entity as Field, writer);
                        break;
                    case NodeType.Property:
                        writer.Write("P:");
                        WriteProperty(entity as Property, writer);
                        break;
                    case NodeType.Method:
                        writer.Write("M:");
                        WriteMethod(entity as Method, writer);
                        break;
                    case NodeType.InstanceInitializer:
                        writer.Write("M:");
                        WriteConstructor(entity as InstanceInitializer, writer);
                        break;
                    case NodeType.StaticInitializer:
                        writer.Write("M:");
                        WriteStaticConstructor(entity as StaticInitializer, writer);
                        break;
                    case NodeType.Event:
                        writer.Write("E:");
                        WriteEvent(entity as Event, writer);
                        break;
                    }
                    return (writer.ToString());
                }

            }

            private static void WriteConstructor(InstanceInitializer constructor, TextWriter writer) {
                WriteType(constructor.DeclaringType, writer);
                writer.Write(".#ctor");
                WriteParameters(constructor.Parameters, writer);
            }

            private static void WriteEvent(Event trigger, TextWriter writer) {
                WriteType(trigger.DeclaringType, writer);

                Event eiiTrigger = null;
                if (trigger.IsPrivate && trigger.IsVirtual) {
                    Event[] eiiTriggers = ReflectionUtilities.GetImplementedEvents(trigger);
                    if (eiiTriggers.Length > 0) eiiTrigger = eiiTriggers[0];
                }

                if (eiiTrigger != null) {
                    TypeNode eiiType = eiiTrigger.DeclaringType;
                    TextWriter eiiWriter = new StringWriter();

                    if (eiiType != null && eiiType.Template != null) {
                        writer.Write(".");
                        WriteTemplate(eiiType, writer);
                    } else {
                        WriteType(eiiType, eiiWriter);
                        writer.Write(".");
                        writer.Write(eiiWriter.ToString().Replace('.', '#'));
                    }

                    writer.Write("#");
                    writer.Write(eiiTrigger.Name.Name);
                } else {
                    writer.Write(".{0}", trigger.Name.Name);
                }
            }

            private static void WriteField(Field member, TextWriter writer) {
                WriteType(member.DeclaringType, writer);
                writer.Write(".{0}", member.Name.Name);
            }

            private static void WriteMethod(Method method, TextWriter writer) {
                string name = method.Name.Name;
                WriteType(method.DeclaringType, writer);

                Method eiiMethod = null;
                if (method.IsPrivate && method.IsVirtual) {
                    MethodList eiiMethods = method.ImplementedInterfaceMethods;
                    if (eiiMethods.Length > 0) eiiMethod = eiiMethods[0];
                }
                if (eiiMethod != null) { //explicitly implemented interface
                    TypeNode eiiType = eiiMethod.DeclaringType;
                    TextWriter eiiWriter = new StringWriter();


                    //we need to keep the param names instead of turning them into numbers
                    //get the template to the right format
                    if (eiiType != null && eiiType.Template != null) {
                        writer.Write(".");
                        WriteTemplate(eiiType, writer);
                    } else //revert back to writing the type the old way if there is no template
                {
                        WriteType(eiiType, eiiWriter);
                        writer.Write(".");
                        writer.Write(eiiWriter.ToString().Replace('.', '#'));
                    }

                    writer.Write("#");
                    writer.Write(eiiMethod.Name.Name);
                } else {
                    writer.Write(".{0}", name);
                }
                if (method.IsGeneric) {
                    TypeNodeList genericParameters = method.TemplateParameters;
                    if (genericParameters != null) {
                        writer.Write("``{0}", genericParameters.Length);
                    }
                }
                WriteParameters(method.Parameters, writer);
                // add ~ for conversion operators
                if ((name == "op_Implicit") || (name == "op_Explicit")) {
                    writer.Write("~");
                    WriteType(method.ReturnType, writer);
                }

            }

            // The actual logic to construct names
            private static void WriteNamespace(Namespace space, TextWriter writer) {
                writer.Write(space.Name);
            }

            private static void WriteParameters(ParameterList parameters, TextWriter writer) {
                if ((parameters == null) || (parameters.Length == 0)) return;
                writer.Write("(");
                for (int i = 0; i < parameters.Length; i++) {
                    if (i > 0) writer.Write(",");
                    WriteType(parameters[i].Type, writer);
                }
                writer.Write(")");
            }

            private static void WriteProperty(Property property, TextWriter writer) {
                WriteType(property.DeclaringType, writer);
                //Console.WriteLine( "{0}::{1}", property.DeclaringType.FullName, property.Name );

                Property eiiProperty = null;
                if (property.IsPrivate && property.IsVirtual) {
                    Property[] eiiProperties = ReflectionUtilities.GetImplementedProperties(property);
                    if (eiiProperties.Length > 0) eiiProperty = eiiProperties[0];
                }

                if (eiiProperty != null) {
                    TypeNode eiiType = eiiProperty.DeclaringType;
                    TextWriter eiiWriter = new StringWriter();

                    if (eiiType != null && eiiType.Template != null) {
                        writer.Write(".");
                        WriteTemplate(eiiType, writer);
                    } else {
                        WriteType(eiiType, eiiWriter);
                        writer.Write(".");
                        writer.Write(eiiWriter.ToString().Replace('.', '#'));
                    }

                    writer.Write("#");
                    writer.Write(eiiProperty.Name.Name);
                } else {
                    writer.Write(".{0}", property.Name.Name);
                }
                ParameterList parameters = property.Parameters;
                WriteParameters(parameters, writer);
            }

            private static void WriteStaticConstructor(StaticInitializer constructor, TextWriter writer) {
                WriteType(constructor.DeclaringType, writer);
                writer.Write(".#cctor");
                WriteParameters(constructor.Parameters, writer);
            }

            /// <summary>
            /// Used for explicitly implemented interfaces to convert the template to the
            /// format used in the comments file.
            /// </summary>
            /// <param name="type">EII Type</param>
            /// <param name="writer"></param>
            private static void WriteTemplate(TypeNode eiiType, TextWriter writer) {
                string eiiClean = eiiType.Template.ToString();
                eiiClean = eiiClean.Replace('.', '#');
                eiiClean = eiiClean.Replace(',', '@'); //change the seperator between params
                eiiClean = eiiClean.Replace('<', '{'); //change the parameter brackets
                eiiClean = eiiClean.Replace('>', '}');
                writer.Write(eiiClean);
            }

            private static void WriteType(TypeNode type, TextWriter writer) {
                switch (type.NodeType) {
                case NodeType.ArrayType:
                    ArrayType array = type as ArrayType;
                    WriteType(array.ElementType, writer);
                    writer.Write("[");
                    if (array.Rank > 1) {
                        for (int i = 0; i < array.Rank; i++) {
                            if (i > 0) writer.Write(",");
                            writer.Write("0:");
                        }
                    }
                    writer.Write("]");
                    break;
                case NodeType.Reference:
                    Reference reference = type as Reference;
                    TypeNode referencedType = reference.ElementType;
                    WriteType(referencedType, writer);
                    writer.Write("@");
                    break;
                case NodeType.Pointer:
                    Pointer pointer = type as Pointer;
                    WriteType(pointer.ElementType, writer);
                    writer.Write("*");
                    break;
                case NodeType.OptionalModifier:
                    TypeModifier optionalModifierClause = type as TypeModifier;
                    WriteType(optionalModifierClause.ModifiedType, writer);
                    writer.Write("!");
                    WriteType(optionalModifierClause.Modifier, writer);
                    break;
                case NodeType.RequiredModifier:
                    TypeModifier requiredModifierClause = type as TypeModifier;
                    WriteType(requiredModifierClause.ModifiedType, writer);
                    writer.Write("|");
                    WriteType(requiredModifierClause.Modifier, writer);
                    break;
                default:
                    if (type.IsTemplateParameter) {
                        ITypeParameter gtp = (ITypeParameter)type;
                        if (gtp.DeclaringMember is TypeNode) {
                            writer.Write("`");
                        } else if (gtp.DeclaringMember is Method) {
                            writer.Write("``");
                        } else {
                            throw new InvalidOperationException("Generic parameter not on type or method.");
                        }
                        writer.Write(gtp.ParameterListIndex);
                    } else {
                        // namespace
                        TypeNode declaringType = type.DeclaringType;
                        if (declaringType != null) {
                            // names of nested types begin with outer type name
                            WriteType(declaringType, writer);
                            writer.Write(".");
                        } else {
                            // otherwise just prepend the namespace
                            Identifier space = type.Namespace;
                            if ((space != null) && !String.IsNullOrEmpty(space.Name)) {
                                //string space = type.Namespace.Name;
                                //if (space != null && space.Length > 0) {
                                writer.Write(space.Name);
                                writer.Write(".");
                            }
                        }
                        // name
                        writer.Write(type.GetUnmangledNameWithoutTypeParameters());
                        // generic parameters
                        if (type.IsGeneric) {
                            // number of parameters
                            TypeNodeList parameters = type.TemplateParameters;
                            if (parameters != null) {
                                writer.Write("`{0}", parameters.Length);
                            }
                            // arguments
                            TypeNodeList arguments = type.TemplateArguments;
                            if ((arguments != null) && (arguments.Length > 0)) {
                                writer.Write("{");
                                for (int i = 0; i < arguments.Length; i++) {
                                    TypeNode argument = arguments[i];
                                    if (i > 0) writer.Write(",");
                                    WriteType(arguments[i], writer);
                                }
                                writer.Write("}");
                            }
                        }
                    }
                    break;
                }
            }
        }
*/
