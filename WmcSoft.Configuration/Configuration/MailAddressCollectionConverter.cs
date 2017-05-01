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
using System.Configuration;
using System.Globalization;
using System.Net.Mail;

namespace WmcSoft.Configuration
{
    /// <summary>
    /// Configuration converter to <see cref="MailAddressCollection"/>
    /// </summary>
    /// <remarks>Multiple e-mail addresses must be separated with a comma character (see <https://tools.ietf.org/html/rfc2822>).</remarks>
    public sealed class MailAddressCollectionConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
        {
            var collection = new MailAddressCollection();
            if (data != null) {
                var addresses = data.ToString();
                collection.Add(addresses);
            }
            return collection;
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type)
        {
            var collection = value as MailAddressCollection;
            if (collection == null || collection.Count == 0)
                return "";
            return collection.ToString();
        }
    }
}