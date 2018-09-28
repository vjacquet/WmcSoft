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
    /// <summary>
    /// Base class when creating <see cref="XmlWriter"/> decorating another one.
    /// </summary>
    public class XmlWriterDecorator : XmlWriter
    {
        #region properties

        XmlWriter writer;

        protected XmlWriter Inner => writer;

        #endregion

        #region Lifecycle

        protected XmlWriterDecorator(XmlWriter writer)
        {
            this.writer = writer;
        }

        #endregion

        public override string ToString()
        {
            return writer.ToString();
        }

        public override void Close()
        {
            writer.Close();
        }

        protected override void Dispose(bool disposing)
        {
            writer.Close();
        }

        public override void Flush()
        {
            writer.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return writer.LookupPrefix(ns);
        }

        public override XmlWriterSettings Settings => writer.Settings;

        public override void WriteAttributes(XmlReader reader, bool defattr)
        {
            writer.WriteAttributes(reader, defattr);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            writer.WriteBase64(buffer, index, count);
        }

        public override void WriteBinHex(byte[] buffer, int index, int count)
        {
            writer.WriteBinHex(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            writer.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            writer.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            writer.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            writer.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            writer.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            writer.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            writer.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            writer.WriteEndDocument();
        }

        public override void WriteEntityRef(string name)
        {
            writer.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            writer.WriteFullEndElement();
        }

        public override void WriteName(string name)
        {
            writer.WriteName(name);
        }

        public override void WriteNmToken(string name)
        {
            writer.WriteNmToken(name);
        }

        public override void WriteNode(System.Xml.XPath.XPathNavigator navigator, bool defattr)
        {
            writer.WriteNode(navigator, defattr);
        }

        public override void WriteNode(XmlReader reader, bool defattr)
        {
            writer.WriteNode(reader, defattr);
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            writer.WriteProcessingInstruction(name, text);
        }

        public override void WriteQualifiedName(string localName, string ns)
        {
            writer.WriteQualifiedName(localName, ns);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            writer.WriteRaw(buffer, index, count);
        }

        public override void WriteRaw(string data)
        {
            writer.WriteRaw(data);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            writer.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument()
        {
            writer.WriteStartDocument();
        }

        public override void WriteStartDocument(bool standalone)
        {
            writer.WriteStartDocument(standalone);
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            writer.WriteStartElement(prefix, localName, ns);
        }

        public override WriteState WriteState => writer.WriteState;

        public override void WriteString(string text)
        {
            writer.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            writer.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteValue(bool value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(DateTime value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(decimal value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(double value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(float value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(int value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(long value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(object value)
        {
            writer.WriteValue(value);
        }

        public override void WriteValue(string value)
        {
            writer.WriteValue(value);
        }

        public override void WriteWhitespace(string ws)
        {
            writer.WriteWhitespace(ws);
        }

        public override string XmlLang => writer.XmlLang;

        public override XmlSpace XmlSpace => writer.XmlSpace;
    }

}
