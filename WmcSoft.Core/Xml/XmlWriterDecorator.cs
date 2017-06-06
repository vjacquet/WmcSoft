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
    public class XmlWriterDecorator : XmlWriter
    {
        #region properties

        XmlWriter _writer;

        protected XmlWriter Inner {
            get { return _writer; }
        }

        #endregion

        #region Lifecycle

        protected XmlWriterDecorator(XmlWriter writer)
        {
            _writer = writer;
        }

        #endregion

        public override string ToString()
        {
            return _writer.ToString();
        }

        public override void Close()
        {
            _writer.Close();
        }

        protected override void Dispose(bool disposing)
        {
            _writer.Close();
        }

        public override void Flush()
        {
            _writer.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return _writer.LookupPrefix(ns);
        }

        public override XmlWriterSettings Settings {
            get { return _writer.Settings; }
        }

        public override void WriteAttributes(XmlReader reader, bool defattr)
        {
            _writer.WriteAttributes(reader, defattr);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            _writer.WriteBase64(buffer, index, count);
        }

        public override void WriteBinHex(byte[] buffer, int index, int count)
        {
            _writer.WriteBinHex(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            _writer.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            _writer.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            _writer.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            _writer.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            _writer.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            _writer.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            _writer.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            _writer.WriteEndDocument();
        }

        public override void WriteEntityRef(string name)
        {
            _writer.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            _writer.WriteFullEndElement();
        }

        public override void WriteName(string name)
        {
            _writer.WriteName(name);
        }

        public override void WriteNmToken(string name)
        {
            _writer.WriteNmToken(name);
        }

        public override void WriteNode(System.Xml.XPath.XPathNavigator navigator, bool defattr)
        {
            _writer.WriteNode(navigator, defattr);
        }

        public override void WriteNode(XmlReader reader, bool defattr)
        {
            _writer.WriteNode(reader, defattr);
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            _writer.WriteProcessingInstruction(name, text);
        }

        public override void WriteQualifiedName(string localName, string ns)
        {
            _writer.WriteQualifiedName(localName, ns);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            _writer.WriteRaw(buffer, index, count);
        }

        public override void WriteRaw(string data)
        {
            _writer.WriteRaw(data);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            _writer.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument()
        {
            _writer.WriteStartDocument();
        }

        public override void WriteStartDocument(bool standalone)
        {
            _writer.WriteStartDocument(standalone);
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            _writer.WriteStartElement(prefix, localName, ns);
        }

        public override WriteState WriteState {
            get { return _writer.WriteState; }
        }

        public override void WriteString(string text)
        {
            _writer.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            _writer.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteValue(bool value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(DateTime value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(decimal value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(double value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(float value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(int value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(long value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(object value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(string value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteWhitespace(string ws)
        {
            _writer.WriteWhitespace(ws);
        }

        public override string XmlLang {
            get { return _writer.XmlLang; }
        }

        public override XmlSpace XmlSpace {
            get { return _writer.XmlSpace; }
        }
    }

}
