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

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Determines the ProductTypes of the ProductInstances that may be included
    /// in a PackageInstance based on selections from a ProductSet. The inclusion
    /// can occur only if the condition PropositionOfInclusion is true.
    /// </summary>
    public class ConditionalPropositionOfInclusion : PropositionOfInclusion
    {
        #region Fields

        readonly List<PropositionOfInclusion> _body;

        #endregion

        #region Lifecyle

        ConditionalPropositionOfInclusion() {
            _body = new List<PropositionOfInclusion>();
        }

        #endregion

        #region Properties

        public PropositionOfInclusion Condition { get; set; }
        public ICollection<PropositionOfInclusion> Body { get { return _body; } }

        #endregion

        #region Overridables

        public override bool IsSubsetOf() {
            throw new NotImplementedException();
        }

        #endregion
    }
}
