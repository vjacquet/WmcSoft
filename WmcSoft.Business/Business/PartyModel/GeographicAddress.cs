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
using System.Collections.Specialized;
using System.Globalization;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents a geographic location at which a Party may be contacted.
    /// It is a postal address for the Party.
    /// </summary>
    public class GeographicAddress : AddressBase
    {
        #region Fields

        readonly StringCollection _addressLines;

        #endregion

        #region Lifecycle

        public GeographicAddress() {
            _addressLines = new StringCollection();
        }

        #endregion

        #region Properties

        public override string Address {
            get {
                // TODO: decide how to render the address considering the locale.
                return String.Empty;
            }
        }

        public StringCollection AddressLines {
            get { return _addressLines; }
        }

        public string City { get; set; }

        public string RegionOrState { get; set; }

        public string ZipOrPostCode { get; set; }

        public RegionInfo Country { get; set; }

        #endregion
    }
}
