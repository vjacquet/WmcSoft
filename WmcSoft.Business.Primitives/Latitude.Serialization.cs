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
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using WmcSoft.Serialization.Json;

namespace WmcSoft
{
    [Serializable]
    [XmlSchemaProvider("GetSchema")]
    [JsonConverter(typeof(GeoCoordinateConverter))]
    public partial struct Latitude : IXmlSerializable
    {
        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var d = reader.NodeType == XmlNodeType.Element
                ? reader.ReadElementContentAsDecimal()
                : reader.ReadContentAsDecimal();

            this = new Latitude(d);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteValue(this);
        }
    }
}
