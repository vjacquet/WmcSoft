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
using System.Collections.Generic;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents an administrative and functional structure.
    /// </summary>
    [RM.DisplayName(nameof(Organization))]
    [RM.Description(nameof(Organization))]
    public abstract class Organization : Party
    {
        #region Lifecycle

        public Organization()
        {
            OtherOrganizationNames = new List<OrganizationName>();
        }

        public Organization(OrganizationName organizationName)
            : this()
        {
            OrganizationName = organizationName;
        }

        public Organization(string organizationName)
            : this(new OrganizationName(organizationName))
        {
        }

        #endregion

        #region Properties

        public virtual OrganizationName OrganizationName { get; set; }
        public virtual ICollection<OrganizationName> OtherOrganizationNames { get; private set; }

        #endregion

        #region IFormattable overrides

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (OrganizationName == null)
                return Id.ToString();
            return OrganizationName.ToString(format, formatProvider);
        }

        #endregion
    }
}
