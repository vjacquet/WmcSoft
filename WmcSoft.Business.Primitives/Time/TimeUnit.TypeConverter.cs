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
 * Adapted from TimeUnit.java, TimeUnitConversionFactors.java
 * ----------------------------------------------------------
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

namespace WmcSoft.Time
{
    [TypeConverter(typeof(TimeUnitConverter))]
    public partial struct TimeUnit
    {
#if NET461
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.InheritanceDemand, Name = "FullTrust")]
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
#endif
        class TimeUnitConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
            {
                return sourceType == typeof(string);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
            {
                return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null) throw GetConvertFromException(value);

                string text = value as string;
                if (text == null) throw new ArgumentException(nameof(value));

                return FromTimeUnitName(text);
            }

            public static TimeUnit FromTimeUnitName(string name)
            {
                switch (name) {
                case "Millisecond": return Millisecond;
                case "Second": return Second;
                case "Minute": return Minute;
                case "Hour": return Hour;
                case "Day": return Day;
                case "Week": return Week;
                case "Month": return Month;
                case "Quarter": return Quarter;
                case "Year": return Year;
                default:
                    throw new FormatException();
                }
            }

            [SecurityCritical]
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
            {
                if (destinationType != null && value is TimeUnit) {
                    var timeUnit = (TimeUnit)value;
                    var name = timeUnit._type.ToString();
                    if (destinationType == typeof(InstanceDescriptor)) {
                        var method = GetType().GetMethod("FromTimeUnitName", new System.Type[] { typeof(string) });
                        return new InstanceDescriptor(method, new object[] { name });
                    }
                    if (destinationType == typeof(string)) {
                        return timeUnit._type.ToString();
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(new[] {
                    Millisecond,
                    Second,
                    Minute,
                    Hour,
                    Day,
                    Week,

                    Month,
                    Quarter,
                    Year,
                });
            }
        }
    }
}
