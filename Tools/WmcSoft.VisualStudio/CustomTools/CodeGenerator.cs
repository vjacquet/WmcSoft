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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;

using WmcSoft.CustomTools.Design;
using WmcSoft.Xml.XPath;
using System.Globalization;
using System.Xml.XPath;
using System.ComponentModel;

using WmcSoft.CodeBuilders;
using System.Resources;
using Microsoft.VisualStudio.Shell;

namespace WmcSoft.CustomTools
{
    [Guid("ddd7d995-7b4f-4ff6-9173-bb66ddd4ab77")]
    [CustomTool("WmcSoft.CodeGenerator", true)]
    [SupportedCategory(SupportedCategoryAttribute.CSharpCategory)]
    [SupportedCategory(SupportedCategoryAttribute.VBCategory)]
    [SupportedVersion("8.0")]
    [SupportedVersion("9.0")]
    [SupportedVersion("10.0")]
    [SupportedVersion("11.0")]
    [SupportedVersion("12.0")]
    [ComVisible(true)]
    public class CodeGenerator : CustomToolBase
    {
        #region Lifecycle

        public CodeGenerator() {
        }

        public CodeGenerator(CodeDomProvider codeProvider) {
            this.CodeProvider = codeProvider;
        }

        #endregion

        #region Generation method
#if OLD
        private CodeCommentStatement Generate(XmlSchemaAnnotation annotation) {
            CodeCommentStatement comment = new CodeCommentStatement();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<summary>");
            foreach (XmlSchemaDocumentation documentation in annotation.Items) {
                foreach (XmlNode node in documentation.Markup) {
                    node.Normalize();
                    TextReader reader = new StringReader(node.InnerText);
                    while (reader.Peek() != -1) {
                        string line = reader.ReadLine().Trim();
                        if (!String.IsNullOrEmpty(line)) {
                            builder.Append(' ');
                            builder.AppendLine(line);
                        }
                    }

                }
            }
            builder.Append(" </summary>");
            comment.Comment = new CodeComment(builder.ToString(), true);
            return comment;
        }

        private CodeTypeReference Generate(XmlSchemaSimpleType type) {
            CodeTypeReference codeTypeReference = GetReference(type.Name);

            if (type.Content is XmlSchemaSimpleTypeRestriction) {
                XmlSchemaSimpleTypeRestriction restriction = (XmlSchemaSimpleTypeRestriction)type.Content;
                if (restriction.BaseTypeName.Name == "token" || restriction.BaseTypeName.Name == "NMTOKEN") {
                    bool isEnumeration = false;
                    int minLength = -1;
                    int maxLength = -1;
                    foreach (XmlSchemaObject item in restriction.Facets) {
                        if (item is XmlSchemaMinLengthFacet) {
                            minLength = Int16.Parse(((XmlSchemaMinLengthFacet)item).Value);
                        } else if (item is XmlSchemaMaxLengthFacet) {
                            maxLength = Int16.Parse(((XmlSchemaMaxLengthFacet)item).Value);
                        } else if (item is XmlSchemaEnumerationFacet) {
                            isEnumeration = true;
                            break;
                        }
                    }

                    if (isEnumeration) {
                        CodeTypeDeclaration codeType = new CodeTypeDeclaration(type.Name);
                        codeType.IsEnum = true;
                        codeType.IsPartial = true;
                        codeType.Comments.Add(Generate(type.Annotation));
                        codeNamespace.Types.Add(codeType);

                        foreach (XmlSchemaEnumerationFacet item in restriction.Facets) {
                            CodeTypeMember member = new CodeMemberField();
                            member.Name = String.Join("", item.Value.Split(' ', '-', '/'));

                            codeType.Members.Add(member);
                        }

                        codeTypeReference.BaseType = codeType.Name;
                    } else if (minLength == 1 && maxLength == 1) {
                        codeTypeReference.BaseType = "System.Char";
                    } else {
                        codeTypeReference.BaseType = "System.String";
                    }
                } else if (restriction.BaseTypeName.Name == "int") {
                    codeTypeReference.BaseType = "System.Int32";
                }
                restriction = null;
            }

            return codeTypeReference;
        }

        private CodeTypeReference GetReference(string name) {
            CodeTypeReference codeTypeReference = null;
            if (references.ContainsKey(name)) {
                codeTypeReference = references[name];
            } else {
                codeTypeReference = new CodeTypeReference("System.Object");
                references[name] = codeTypeReference;
            }
            return codeTypeReference;
        }

        private CodeTypeDeclaration Generate(XmlSchemaComplexType type) {
            CodeTypeDeclaration codeType = new CodeTypeDeclaration(type.Name);
            codeType.IsClass = true;
            codeType.IsPartial = true;
            codeType.Comments.Add(Generate(type.Annotation));

            if (type.IsAbstract) {
                codeType.TypeAttributes |= TypeAttributes.Abstract;
            }
            if (type.Particle is XmlSchemaSequence) {
                XmlSchemaSequence sequence = (XmlSchemaSequence)type.Particle;
                foreach (XmlSchemaObject item in sequence.Items) {
                    XmlSchemaElement element = item as XmlSchemaElement;
                    if (element != null) {
                        CodeMemberField field = new CodeMemberField();
                        field.Name = element.Name;
                        field.Attributes |= MemberAttributes.Public;
                        if (element.SchemaType != null) {
                            field.Type = new CodeTypeReference("System.String");
                        } else {
                            field.Type = GetReference(element.SchemaTypeName.Name);
                        }
                        codeType.Members.Add(field);
                    }
                }
            }

            return codeType;
        }

        private CodeCompileUnit Generate(XmlSchema schema) {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            this.compileUnit = compileUnit;

            CodeNamespace codeNamespace = new CodeNamespace("Broadsoft");
            compileUnit.Namespaces.Add(codeNamespace);
            this.codeNamespace = codeNamespace;

            // Add the new namespace import for the System namespace.
            //
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));

            // Generate the complex types
            //
            foreach (XmlSchemaObject item in schema.Items) {
                if (item is XmlSchemaSimpleType) {
                    Generate((XmlSchemaSimpleType)item);
                } else if (item is XmlSchemaComplexType) {
                    CodeTypeDeclaration codeType = Generate((XmlSchemaComplexType)item);
                    codeNamespace.Types.Add(codeType);
                } else {
                    base.GeneratorErrorCallback(true, 1000, "Unhandled schema object", item.LineNumber, item.LinePosition);
                }
            }

            return compileUnit;
        }
#endif
        #endregion

        #region Custom Tool

        public override string GetDefaultExtension() {
            return ".CodeGen" + base.GetDefaultExtension();
        }

        protected override string OnGenerateCode(string inputFileName, string inputFileContent) {
            var writer = new StringWriter();
            var options = new CodeGeneratorOptions {
                BlankLinesBetweenMembers = false,
                ElseOnClosing = true,
            };

            // read the input...
            var settings = new XmlReaderSettings();
            var resourceManager = new ResourceManager(typeof(CodeBuilder));
            using (var stream = typeof(CodeBuilder).Assembly.GetManifestResourceStream("WmcSoft.CodeBuilders.CodeGenerator.xsd")) {
                var schema = XmlSchema.Read(stream, new ValidationEventHandler(SchemaValidationCallback));
                settings.Schemas.Add(schema);
                settings.ValidationType = ValidationType.Schema;
            }

            CodeBuilderContext context = new CodeBuilderContext(this);

            using (XmlReader reader = XmlReader.Create(new StringReader(inputFileContent), settings)) {
                if (reader.Read()) {
                    reader.MoveToContent();
                    Debug.Assert(reader.Name == "codeGenerator");
                    CodeGeneratorBuilder builder = new CodeGeneratorBuilder();
                    builder.Parse(reader, context);
                }
            }

            // then generate the code
            CodeProvider.GenerateCodeFromCompileUnit(context.CurrentCompileUnit, writer, options);

            return writer.ToString();
        }

        public static void SchemaValidationCallback(object sender, ValidationEventArgs args) {
            Console.WriteLine(args.Message);
        }


        #endregion
    }
}
