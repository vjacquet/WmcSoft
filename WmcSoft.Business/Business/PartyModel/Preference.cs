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

using System.Collections.Generic;

namespace WmcSoft.Business.PartyModel
{
    /// <summary>
    /// Represents a Party's expressed choice of (or liking for) something,
    /// often from a set of possible or offered options.
    /// </summary>
    public abstract class Preference
    {
        #region Fields

        private readonly string _name;
        private readonly string _description;

        #endregion

        #region Lifecycle

        protected Preference(string name, string description) {
            _name = name;
            _description = description;
        }

        #endregion

        #region Properties

        public string OptionName {
            get { return _name; }
        }

        public string OptionDescription {
            get { return _description; }
        }

        #endregion

        #region Overridables

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract IEnumerable<Preference> Options { get; }

        #endregion
    }

}
