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
using System.Text;
using System.Text.RegularExpressions;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents a number that can contact a telephone, mobile phone, fax,
    /// pager, or other telephonic device.
    /// </summary>
    public class TelecomAddress : AddressBase
    {
        #region Lifecycle

        public TelecomAddress()
        {
        }

        public TelecomAddress(string address, TelecomPhysicalType type = TelecomPhysicalType.Phone)
        {
            (CountryCode, NationalDirectDialingPrefix, AreaType, Number, Extension) = Parse(address);
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
                // see <https://en.wikipedia.org/wiki/E.164>
                var sb = new StringBuilder();
                if (string.IsNullOrEmpty(CountryCode)) {
                    sb.Append(NationalDirectDialingPrefix);
                    sb.Append(AreaType);
                } else {
                    sb.Append('+');
                    sb.Append(CountryCode);
                    if (!string.IsNullOrEmpty(NationalDirectDialingPrefix)) {
                        sb.Append(" (");
                        sb.Append(NationalDirectDialingPrefix);
                        sb.Append(')');
                    } else {
                        sb.Append(" ");
                    }
                }
                if (sb.Length > 0)
                    sb.Append(' ');
                sb.Append(Number);
                if (!string.IsNullOrEmpty(Extension)) {
                    sb.Append(" ext. ");
                    sb.Append(Extension);
                }
                return sb.ToString();
            }
        }

        #endregion

        #region Helpers

        static readonly Regex _parser = new Regex(@"^(\+(?<country>\d{1,3})?(\s+\((?<ndd>\d*)\)(?<area>\d*))?\s+)?(?<number>(\d|\s)*)( ext\. (?<ext>\d*))?$");

        public static (string country, string ndd, string area, string number, string ext) Parse(string address)
        {
            var m = _parser.Match(address);
            if (!m.Success)
                throw new FormatException();
            return (Val(m, "country"), Val(m, "ndd"), Val(m, "area"), Val(m, "number"), Val(m, "ext"));
        }

        static string Val(Match m, string name)
        {
            var g = m.Groups[name];
            return g.Success ? g.Value : null;
        }

        #endregion
    }
}
