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
    /// Represents an element of a <see cref="Rule"/> or <see cref="RuleContext"/>.
    /// </summary>
    [XmlInclude(typeof(Proposition))]
    [XmlInclude(typeof(Variable))]
    [XmlInclude(typeof(Operator))]
    [XmlInclude(typeof(XOr))]
    [XmlInclude(typeof(And))]
    [XmlInclude(typeof(Or))]
    [XmlInclude(typeof(GreaterThanOrEqualTo))]
    [XmlInclude(typeof(LesserThanOrEqualTo))]
    [XmlInclude(typeof(LesserThan))]
    [XmlInclude(typeof(Not))]
    [XmlInclude(typeof(EqualTo))]
    [XmlInclude(typeof(NotEqualTo))]
    [XmlInclude(typeof(GreaterThan))]
    public abstract class RuleElement : IEquatable<RuleElement>
    {
        /// <remarks/>
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlIgnore]
        public abstract string TypeName { get; }

        public override int GetHashCode() {
            return string.IsNullOrWhiteSpace(Name) ? 0 : Name.GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as RuleElement);
        }

        public bool Equals(RuleElement other) {
            if (other == null)
                return false;
            return Name == other.Name && TypeName == other.TypeName;
        }
    }
}
