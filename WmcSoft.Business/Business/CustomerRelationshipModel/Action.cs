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
using WmcSoft.Business.PartyModel;

namespace WmcSoft.Business.CustomerRelationshipModel
{
    /// <summary>
    /// Represent a description of something that can happen.
    /// </summary>
    public class Action
    {
        public Action() {
            PossibleOutcomes = new List<Outcome>();
            ActualOutcomes = new List<Outcome>();
        }

        /// <summary>
        /// A short description of the <see cref="Action"/>.
        /// </summary>
        /// <remarks>This should focus on what the <see cref="Action"/> is trying to achieve.</remarks>
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public IList<Outcome> PossibleOutcomes { get; private set; }
        public IList<Outcome> ActualOutcomes { get; private set; }

        public PartySignature ActionInitiator {
            get {
                throw new NotImplementedException();
            }
        }
        public IList<PartySignature> ActionApprovers {
            get {
                throw new NotImplementedException();
            }
        }
    }

    public enum ActionStatus
    {
        Pending,
        Open,
        Closed,
    }
}
