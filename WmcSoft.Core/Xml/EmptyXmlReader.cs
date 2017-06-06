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

namespace WmcSoft.Xml
{
    public class EmptyXmlReader : XmlReader
    {
        #region Lifecycle

        public EmptyXmlReader()
        {
        }

        #endregion

        #region Methods

        public override void Close()
        {
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override string GetAttribute(int i)
        {
            return string.Empty;
        }

        public override string GetAttribute(string name)
        {
            return string.Empty;
        }

        public override string GetAttribute(string localName, string namespaceURI)
        {
            return string.Empty;
        }

        public bool HasLineInfo()
        {
            return false;
        }

        public override string LookupNamespace(string prefix)
        {
            return string.Empty;
        }

        public override void MoveToAttribute(int i)
        {
        }

        public override bool MoveToAttribute(string name)
        {
            return false;
        }

        public override bool MoveToAttribute(string localName, string namespaceURI)
        {
            return false;
        }

        public override bool MoveToElement()
        {
            return false;
        }

        public override bool MoveToFirstAttribute()
        {
            return false;
        }

        public override bool MoveToNextAttribute()
        {
            return false;
        }

        public override bool Read()
        {
            return false;
        }

        public override bool ReadAttributeValue()
        {
            return false;
        }

        public override int ReadValueChunk(char[] buffer, int index, int count)
        {
            return 0;
        }

        public override void ResolveEntity()
        {
        }

        #endregion

        #region Properties

        public override int AttributeCount {
            get { return 0; }
        }

        public override string BaseURI {
            get { return string.Empty; }
        }

        public override bool CanReadBinaryContent {
            get { return false; }
        }

        public override bool CanReadValueChunk {
            get { return false; }
        }

        public override bool CanResolveEntity {
            get { return false; }
        }

        public override int Depth {
            get { return 0; }
        }

        public override bool EOF {
            get { return true; }
        }

        public override bool HasValue {
            get { return false; }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsEmptyElement {
            get { return true; }
        }

        public override string this[string name, string namespaceURI] {
            get { return string.Empty; }
        }

        public override string this[string name] {
            get { return string.Empty; }
        }

        public override string this[int i] {
            get { return string.Empty; }
        }

        public override string LocalName {
            get { return string.Empty; }
        }

        public override string Name {
            get { return string.Empty; }
        }

        public override string NamespaceURI {
            get { return string.Empty; }
        }

        public override XmlNameTable NameTable {
            get { return null; }
        }

        public override XmlNodeType NodeType {
            get { return XmlNodeType.None; }
        }

        public override string Prefix {
            get { return string.Empty; }
        }

        public override ReadState ReadState {
            get { return ReadState.EndOfFile; }
        }

        public override string Value {
            get { return string.Empty; }
        }

        public override string XmlLang {
            get { return string.Empty; }
        }

        #endregion
    }
}
