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

 ****************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Net;
using WmcSoft.ComponentModel.Design.Serialization;

namespace WmcSoft.Net
{
    /// <summary>
    /// Converts to and from <see cref="IPAddress"/>.
    /// </summary>
    public class IPAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(byte[]))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            var sourceType = value.GetType();
            if (sourceType == typeof(byte[]))
                return new IPAddress((byte[])value);

            if (value is string) {
                var text = value.ToString();
                if (text == "")
                    return null;

                if (IPAddress.TryParse(text, out var address))
                    return address;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(byte[]))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return null;

            if (value is IPAddress address) {
                if (destinationType == typeof(string))
                    return address.ToString();

                if (destinationType == typeof(InstanceDescriptor))
                    //return typeof(IPAddress).DescribeMethod("Parse", address.ToString());
                    return typeof(IPAddress).DescribeConstructor(address.GetAddressBytes());

                if (destinationType == typeof(byte[]))
                    return address.GetAddressBytes();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
