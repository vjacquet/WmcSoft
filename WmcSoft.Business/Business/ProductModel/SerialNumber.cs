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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Represents a unique identifier for a specific instance of a product.
    /// </summary>
    public class SerialNumber : IUniqueIdentifier<string>, IComparable<SerialNumber>
    {
        #region Fields

        readonly string _serialNumber;

        #endregion

        #region Lifecycle

        public SerialNumber(string serialNumber) {
            _serialNumber = serialNumber;
        }

        public static implicit operator SerialNumber(string value) {
            return new SerialNumber(value);
        }

        #endregion

        #region IUniqueIdentifier<string> Members

        public string Identifier {
            get { return _serialNumber; }
        }

        #endregion

        #region IComparable<SerialNumber> Members

        public virtual int CompareTo(SerialNumber other) {
            return StringComparer.InvariantCultureIgnoreCase.Compare(_serialNumber, other._serialNumber);
        }

        #endregion

        #region IEquatable<string> Members

        public bool Equals(string other) {
            return StringComparer.InvariantCultureIgnoreCase.Equals(_serialNumber, other);
        }

        #endregion
    }
}
