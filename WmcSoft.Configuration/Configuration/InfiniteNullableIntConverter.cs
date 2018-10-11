#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Configuration;
using System.Globalization;

namespace WmcSoft.Configuration
{
    /// <summary>
    /// Converts between a string and the standard infinite or integer value.
    /// <c>null</c> or empty strings converts to <c>null</c>.
    /// </summary>
    public sealed class InfiniteNullableIntConverter : ConfigurationConverterBase
    {
        private readonly InfiniteIntConverter converter;

        public InfiniteNullableIntConverter()
        {
            converter = new InfiniteIntConverter();
        }

        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
        {
            if (string.IsNullOrEmpty((string)data))
                return null;
            return converter.ConvertFrom(ctx, ci, data);
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
        {
            if (value == null)
                return "";
            return converter.ConvertTo(ctx, ci, value, type);
        }
    }
}
