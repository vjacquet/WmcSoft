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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WmcSoft
{
    [Serializable]
    [XmlSchemaProvider("GetSchema")]
    [TypeConverter(typeof(LatitudeConverter))]
    public partial struct Latitude : IXmlSerializable
    {
        class LatitudeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            static Latitude ParseInvariant(string input)
            {
                var d = decimal.Parse(input, CultureInfo.InvariantCulture);
                return new Latitude(d);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                    throw GetConvertFromException(value);

                if (value is string text)
                    return ParseInvariant(text);

                throw new ArgumentException(nameof(value));
            }

            [SecurityCritical]
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is Latitude l) {
                    if (destinationType == typeof(InstanceDescriptor)) {
                        var method = GetType().GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
                        var (y, m, d, s) = l;
                        return new InstanceDescriptor(method, new object[] { y, m, d, s });
                    }
                    if (destinationType == typeof(string)) {
                        decimal degrees = l;
                        return degrees >= 0
                            ? string.Format(CultureInfo.InvariantCulture, "+{0:00.######}", degrees)
                            : string.Format(CultureInfo.InvariantCulture, "-{0:00.######}", -degrees);
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet _)
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
