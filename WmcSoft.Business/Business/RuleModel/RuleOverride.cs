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
using System.Xml.Serialization;

namespace WmcSoft.Business.RuleModel
{
    /// <summary>
    /// Overrides a <see cref="Rule"/> in a <see cref="RuleSet"/>.
    /// </summary>
    public class RuleOverride : Proposition, IEquatable<RuleOverride>
    {
        /// <remarks/>
        [XmlAttribute("ref")]
        public string Reference { get; set; }

        /// <remarks/>
        [XmlAttribute("why")]
        public string Why { get; set; }

        /// <remarks/>
        [XmlAttribute("when")]
        public DateTime When { get; set; }

        /// <remarks/>
        [XmlIgnore]
        public sealed override string TypeName {
            get { return "RuleOverride"; }
        }

        #region IEquatable<Proposition> Membres

        public bool Equals(RuleOverride other) {
            if (other == null)
                return false;
            return base.Equals(other)
                && Value == other.Value
                && Reference == other.Reference
                && Why == other.Why
                && When == other.When;
        }

        public override bool Equals(object obj) {
            return Equals(obj as RuleOverride);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        #endregion
    }
}
