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
    public static class XmlReaderExtensions
    {
        public static string ReadAttributeValue(this XmlReader reader, string name) {
            reader.MoveToAttribute(name);
            reader.ReadAttributeValue();
            var value = reader.Value;
            reader.MoveToElement();
            return value;
        }

        public static T ReadAttributeValue<T>(this XmlReader reader, string name)
            where T : IConvertible {
            var value = reader.ReadAttributeValue(name);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
