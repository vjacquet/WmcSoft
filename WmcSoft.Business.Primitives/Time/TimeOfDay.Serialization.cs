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

 ****************************************************************************
 * Adapted from TimeOfDay.java
 * ---------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WmcSoft.Time
{
    [Serializable]
    [XmlSchemaProvider("GetSchema")]
    [TypeConverter(typeof(TimeOfDayConverter))]
    public partial struct TimeOfDay : IXmlSerializable
    {
        class TimeOfDayConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                    throw GetConvertFromException(value);

                if (value is string text) {
                    var d = DateTime.ParseExact(text, GetFormat(culture), culture);
                    return new TimeOfDay(d.Hour, d.Minute);
                }

                throw new ArgumentException(nameof(value));
            }

            [SecurityCritical]
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType != null && value is TimeOfDay time) {
                    if (destinationType == typeof(InstanceDescriptor)) {
                        var method = typeof(TimeOfDay).GetConstructor(new Type[] { typeof(int), typeof(int) });
                        var (h, m) = time;
                        return new InstanceDescriptor(method, new object[] { h, m });
                    }
                    if (destinationType == typeof(string)) {
                        return time.ToString(GetFormat(culture), culture);
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            static string GetFormat(CultureInfo culture)
            {
                return culture.TwoLetterISOLanguageName == "iv" ? @"HH\:mm" : "t";
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet _)
        {
            return new XmlQualifiedName("time", "http://www.w3.org/2001/XMLSchema");
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
            if (!DateTime.TryParseExact(s, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) {
                throw new FormatException();
            }

            this = new TimeOfDay(d.Hour, d.Minute);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(_storage.ToString(@"hh\:mm", CultureInfo.InvariantCulture));
        }
    }
}
