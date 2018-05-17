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
    /// Represents an automated agent.
    /// </summary>
    public class Bot : Party
    {
        #region Lifecycle

        public Bot()
        {
            OtherBotNames = new List<BotName>();
        }

        public Bot(BotName organizationName)
            : this()
        {
            BotName = organizationName;
        }

        public Bot(string organizationName)
            : this(new BotName(organizationName))
        {
        }

        #endregion

        #region Properties

        public virtual BotName BotName { get; set; }
        public virtual ICollection<BotName> OtherBotNames { get; private set; }

        #endregion

        #region IFormattable overrides

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            if (BotName == null)
                return Id.ToString();
            return BotName.ToString(format, formatProvider);
        }

        #endregion
    }
}
