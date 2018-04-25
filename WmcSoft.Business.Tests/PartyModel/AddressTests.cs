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
using Xunit;
using WmcSoft.Business.PartyModel;

namespace WmcSoft.Business.Tests.PartyModel
{
    public class AddressTests
    {
        [Theory]
        [InlineData("+44 (0)208 123 4567 ext. 789", "44", "0", "208", "123 4567", "789")]
        [InlineData("+44 (0)208 123 4567", "44", "0", "208", "123 4567", null)]
        [InlineData("+44 0208 123 4567 ext. 789", "44", null, null, "0208 123 4567", "789")]
        [InlineData("0208 123 4567 ext. 789", null, null, null, "0208 123 4567", "789")]
        public void CanParseTelecomAddress(string address, string country, string ndd, string area, string number, string ext)
        {
            var (c, d, a, n, e) = TelecomAddress.Parse(address);
            Assert.Equal(country, c);
            Assert.Equal(ndd, d);
            Assert.Equal(area, a);
            Assert.Equal(number, n);
            Assert.Equal(ext, e);
        }

        [Theory]
        [InlineData("+44 0208 123 4567 ext 789")]
        [InlineData("(0)208 123 4567 ext. 789")]
        public void CannotParseIncorrectTelecomAddress(string address)
        {
            Assert.Throws<FormatException>(() => TelecomAddress.Parse(address));
        }
    }
}
