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

using System.Globalization;
using Xunit;

namespace WmcSoft.Business.PartyModel
{
    public class FormatterTests
    {
        [Theory]
        [InlineData("l", "55 Rue du Faubourg Saint-Honoré")]
        [InlineData("t", "Paris")]
        [InlineData("T", "PARIS")]
        [InlineData("z", "75008")]
        [InlineData("c", "France")]
        [InlineData("C", "FRANCE")]
        [InlineData("r", "")]
        [InlineData("R", "")]
        [InlineData("O2", "FR")]
        [InlineData("O3", "FRA")]
        [InlineData("g", "55 Rue du Faubourg Saint-Honoré\r\n75008 Paris")]
        [InlineData("G", "55 Rue du Faubourg Saint-Honoré\r\n75008 PARIS")]
        [InlineData("i", "55 Rue du Faubourg Saint-Honoré\r\n75008 Paris\r\nFRANCE")]
        [InlineData("I", "55 Rue du Faubourg Saint-Honoré\r\n75008 PARIS\r\nFRANCE")]
        public void CanFormatFrenchGeographicAddress(string format, string expected)
        {
            var address = new GeographicAddress {
                AddressLines = new string[] { "55 Rue du Faubourg Saint-Honoré" },
                ZipOrPostCode = "75008",
                Town = "Paris",
                Country = new RegionInfo("FR"),
            };
            var actual = address.ToString(format);
            Assert.Equal(expected, actual);
        }
    }
}
