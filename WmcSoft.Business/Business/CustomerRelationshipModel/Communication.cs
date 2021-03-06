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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Business.PartyModel;

namespace WmcSoft.Business.CustomerRelationshipModel
{
    /// <summary>
    /// Represents a type of <see cref="PartyRelationship"/> that capture the details of a communication
    /// between two <see cref="PartyRole"/>s
    /// </summary>
    public abstract class Communication : PartyRelationship
    {
        #region Lifecycle

        protected Communication(PartyRole client, PartyRole supplier)
            : base(client, supplier) {
            NextCommunications = new List<Communication>();
        }

        #endregion

        #region Properties

        public DateTime? Sent { get; set; }
        public DateTime? Received { get; set; }
        public string Content { get; set; }
        public string Outcome { get; set; }

        public AddressBase FromAddress { get; set; }
        public AddressBase ToAddress { get; set; }

        public Communication PreviousCommunication { get; set; }
        public IList<Communication> NextCommunications { get; set; }

        #endregion
    }
}