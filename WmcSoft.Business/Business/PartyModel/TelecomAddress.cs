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

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents a number that can contact a telephone, mobile phone, fax,
    /// pager, or other telephonic device.
    /// </summary>
    public class TelecomAddress : AddressBase
    {
        #region Lifecycle

        public TelecomAddress() {
        }

        #endregion

        #region Properties

        public string CountryCode { get; set; }

        public string NationalDirectDialingPrefix { get; set; }

        public string AreaType { get; set; }

        public string Number { get; set; }

        public string Extension { get; set; }

        public TelecomPhysicalType PhysicalType { get; set; }

        public override string Address {
            get {
                // TODO: decide how to render the address considering the locale.
                return String.Empty;
            }
        }

        #endregion
    }
}
