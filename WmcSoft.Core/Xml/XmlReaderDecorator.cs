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

using System.Xml;

namespace WmcSoft.Xml
{
    public class XmlReaderDecorator : XmlReader
    {
        #region properties

        XmlReader _reader;

        protected XmlReader Inner {
            get { return _reader; }
        }

        #endregion

        #region Lifecycle

        protected XmlReaderDecorator(XmlReader reader) {
            _reader = reader;
        }

        #endregion

        public override int AttributeCount {
            get { return _reader.AttributeCount; }
        }

        public override string BaseURI {
            get { return _reader.BaseURI; }
        }

        public override void Close() {
            _reader.Close();
        }

        public override int Depth {
            get { return _reader.Depth; }
        }

        public override bool EOF {
            get { return _reader.EOF; }
        }

        public override string GetAttribute(int i) {
            return _reader.GetAttribute(i);
        }

        public override string GetAttribute(string name, string namespaceURI) {
            return _reader.GetAttribute(name, namespaceURI);
        }

        public override string GetAttribute(string name) {
            return _reader.GetAttribute(name);
        }

        public override bool HasValue {
            get { return _reader.HasAttributes; }
        }

        public override bool IsEmptyElement {
            get { return _reader.IsEmptyElement; }
        }

        public override string LocalName {
            get { return _reader.LocalName; }
        }

        public override string LookupNamespace(string prefix) {
            return _reader.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name, string ns) {
            return _reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToAttribute(string name) {
            return _reader.MoveToAttribute(name);
        }

        public override bool MoveToElement() {
            return _reader.MoveToElement();
        }

        public override bool MoveToFirstAttribute() {
            return _reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute() {
            return _reader.MoveToNextAttribute();
        }

        public override XmlNameTable NameTable {
            get { return _reader.NameTable; }
        }

        public override string NamespaceURI {
            get { return _reader.NamespaceURI; ; }
        }

        public override XmlNodeType NodeType {
            get { return _reader.NodeType; }
        }

        public override string Prefix {
            get { return _reader.Prefix; }
        }

        public override bool Read() {
            return _reader.Read();
        }

        public override bool ReadAttributeValue() {
            return _reader.ReadAttributeValue();
        }

        public override ReadState ReadState {
            get { return _reader.ReadState; ; }
        }

        public override void ResolveEntity() {
            _reader.ResolveEntity();
        }

        public override string Value {
            get { return _reader.Value; }
        }
    }

}
