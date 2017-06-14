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
    /// Represents a set of rules, with no duplicates allowed.
    /// </summary>
    [XmlRoot("ruleSet", Namespace = @"http://www.wmcsoft.fr/schemas/2015/business/RuleModel.xsd", IsNullable = false)]
    public class RuleSet : IRuleEvaluator
    {
        public static readonly RuleSet Empty = new RuleSet {
            Version = "1.0",
            Rules = new Rule[0],
        };

        /// <remarks/>
        [XmlElement("rule")]
        public Rule[] Rules { get; set; }

        /// <remarks/>
        [XmlAttribute("version")]
        public string Version { get; set; }

        /// <remarks/>
        [XmlAttribute("name", DataType = "ID")]
        public string Name { get; set; }

        #region Membres de IRuleEvaluator

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Evaluate(RuleContext context)
        {
            foreach (Rule rule in Rules) {
                var ruleElement = context[rule.Name];
                var ruleOverride = ruleElement as RuleOverride;
                if (ruleOverride != null) {
                    if (!ruleOverride.Value)
                        return false;
                } else if (!rule.Evaluate(context))
                    return false;
            }
            return true;
        }

        #endregion
    }
}
