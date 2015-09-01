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
    /// Represents a collection of all <see cref="Communication"/>s about a specific topic
    /// related to a specific <see cref="Customer"/>.
    /// </summary>
    public class CustomerServiceCase
    {
        #region Fields

        readonly CustomerServiceCaseIdentifier _identifier;

        #endregion

        #region Lifecycle

        protected CustomerServiceCase()
            : this(new CustomerServiceCaseIdentifier()) {
        }

        protected CustomerServiceCase(CustomerServiceCaseIdentifier identifier) {
            _identifier = identifier;
        }

        #endregion

        public CustomerServiceCaseIdentifier Identifier {
            get { return _identifier; }
        }

        /// <summary>
        ///  The title of the case.
        /// </summary>
        /// <remarks>This should summarize the nature of the case, e.g. "Complaint about call out service".</remarks>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the case.
        /// </summary>
        public string BriefDescription { get; set; }

        /// <summary>
        /// The <see cref="PartyRoleIdentifier"/> of the <see cref="PartyRole"/> that raised the case.
        /// </summary>
        /// <remarks>This is usually the <see cref="PartyRoleIdentifier"/> of a <see cref="Customer"/>.</remarks>
        public PartyRoleIdentifier RaisedBy { get; set; }

        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsOpen { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
    }
}
