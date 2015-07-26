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
using System.ComponentModel;
using System.Xml.Serialization;

namespace WmcSoft.Business.RuleModel
{
    /// <remarks/>
    public class Proposition : RuleElement
    {
        public Proposition() {
        }

        public Proposition(bool value) {
            this.value = value;
        }

        /// <remarks/>
        [XmlAttribute("value")]
        public bool Value {
            get {
                return this.value;
            }
            set {
                this.value = value;
            }
        }
        bool value;

        /// <remarks/>
        [XmlIgnore()]
        public bool ValueSpecified {
            get {
                return valueSpecified;
            }
            set {
                valueSpecified = value;
            }
        }
        bool valueSpecified;

        /// <remarks/>
        [XmlAttribute("type")]
        [DefaultValue("Proposition")]
        public override string TypeName {
            get {
                return type;
            }
            set {
                //type = value;
            }
        }
        string type = "Proposition";
    }
}
