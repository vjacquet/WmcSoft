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
using System.Text;

using TKey = System.Guid;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Specifies information about an Address assigned to a specific Party.
    /// </summary>
    public class AddressProperties : DomainObject<TKey>, ITemporal
    {
        #region Properties

        public Party Party { get; set; }
        public AddressBase Address { get; set; }
        public string Use { get; set; }

        #endregion

        #region ITemporal Members

        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }

        #endregion
    }
}
