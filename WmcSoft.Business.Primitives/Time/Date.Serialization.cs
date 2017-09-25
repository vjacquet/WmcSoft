﻿#region Licence

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

 ****************************************************************************
 * Adapted from CalendarDate.java
 * ------------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using WmcSoft.Serialization.Json;

namespace WmcSoft.Time
{
    [Serializable]
    [XmlSchemaProvider("GetSchema")]
    [JsonConverter(typeof(DateConverter))]
    public partial struct Date : IXmlSerializable
    {
        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return new XmlQualifiedName("date", "http://www.w3.org/2001/XMLSchema");
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var s = reader.NodeType == XmlNodeType.Element
                ? reader.ReadElementContentAsString()
                : reader.ReadContentAsString();
            if (!DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) {
                throw new FormatException();
            }

            this = new Date(d);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(AsDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
}
