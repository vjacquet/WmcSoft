﻿#region Licence

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

using Xunit;
using WmcSoft.Business.PartyModel;

namespace FlowGroup.Business.Tests.PartyModel
{
    public class PartyTests
    {
        [Fact]
        public void CanCreateCompany()
        {
            var company = new Company("MyLittleCompany");

            Assert.True(company is Party);
            Assert.NotNull(company.OrganizationName);
            Assert.Equal(OrganizationalNameUse.LegalName, company.OrganizationName.Use);
            Assert.Equal("MyLittleCompany", company.OrganizationName.ToString());
        }
    }
}