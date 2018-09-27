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
using System.Globalization;

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents a name for an <see cref="Organization"/>.
    /// </summary>
    public class OrganizationName : DomainObject<TKey>, ITemporal, IFormattable
    {
        #region Lifecycle

        public OrganizationName()
        {
        }

        public OrganizationName(string name, OrganizationNameUse use = OrganizationNameUse.LegalName)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name == "") throw new ArgumentException(nameof(name));

            Name = name;
            Use = use;
        }

        #endregion

        #region Properties

        public virtual string Name { get; set; }
        public virtual OrganizationNameUse Use { get; set; }

        #endregion

        #region ITemporal Members

        public virtual DateTime? ValidSince { get; set; }
        public virtual DateTime? ValidUntil { get; set; }

        #endregion

        #region IFormattable members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var culture = formatProvider as CultureInfo;
            switch (format) {
            case "N":
                return Name.ToUpper(culture);
            case "n":
            default:
                return Name;
            }
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        public sealed override string ToString()
        {
            return ToString(null, null) ?? base.ToString();
        }

        #endregion
    }
}
