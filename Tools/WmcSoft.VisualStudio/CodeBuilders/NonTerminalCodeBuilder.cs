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
using System.Collections.Generic;
using System.Xml;

namespace WmcSoft.CodeBuilders
{
    public abstract class NonTerminalCodeBuilder : CodeBuilder
    {
        protected abstract CodeBuilder Create(string name);

        public sealed override void Parse(XmlReader reader, CodeBuilderContext context)
        {
            var attributes = CodeBuilder.ReadAttributes(reader);
            BeforeParsingChildren(attributes, context);

            int depth = (reader.NodeType == XmlNodeType.None) ? -1 : reader.Depth;
            while (reader.Read() && (depth < reader.Depth)) {
                switch (reader.NodeType) {
                case XmlNodeType.Element:
                    CodeBuilder codeBuilder = Create(reader.LocalName);
                    if (codeBuilder == null)
                        throw new CodeBuilderException(String.Format("Unrecognize element {0}.", reader.LocalName));
                    codeBuilder.Parse(reader, context);
                    break;
                case XmlNodeType.EndElement:
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.EntityReference:
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                case XmlNodeType.Comment:
                case XmlNodeType.DocumentType:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                    break;
                }
            }
            if ((depth == reader.Depth) && (reader.NodeType == XmlNodeType.EndElement)) {
                reader.Read();
            }

            AfterParsingChildren(attributes, context);

            if (attributes.Count > 0)
                throw new CodeBuilderException("Unrecognize attribute.");
        }

        protected virtual void BeforeParsingChildren(IDictionary<string, string> attributes, CodeBuilderContext context)
        {
        }
        protected virtual void AfterParsingChildren(IDictionary<string, string> attributes, CodeBuilderContext context)
        {
        }
    }
}