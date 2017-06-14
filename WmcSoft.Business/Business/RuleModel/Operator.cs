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
    /// <summary>
    /// Represents a boolean operator or a quantifier operator.
    /// </summary>
    [XmlInclude(typeof(And))]
    [XmlInclude(typeof(Or))]
    [XmlInclude(typeof(XOr))]
    [XmlInclude(typeof(Not))]
    [XmlInclude(typeof(GreaterThanOrEqualTo))]
    [XmlInclude(typeof(LesserThanOrEqualTo))]
    [XmlInclude(typeof(LesserThan))]
    [XmlInclude(typeof(EqualTo))]
    [XmlInclude(typeof(NotEqualTo))]
    [XmlInclude(typeof(GreaterThan))]
    public class Operator : RuleElement
    {
        public sealed override string TypeName {
            get { return "Operator"; }
        }
    }

    public enum Operators
    {
        And,
        Or,
        Xor,
        Not,
        EqualTo,
        NotEqualTo,
        GreaterThan,
        LesserThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
    }

    public sealed class And : Operator
    {
        public And()
        {
            Name = "AND";
        }
    }

    public sealed class Or : Operator
    {
        public Or()
        {
            Name = "OR";
        }
    }

    public sealed class XOr : Operator
    {
        public XOr()
        {
            Name = "XOR";
        }
    }

    public sealed class Not : Operator
    {
        public Not()
        {
            Name = "NOT";
        }
    }

    public sealed class GreaterThanOrEqualTo : Operator
    {
        public GreaterThanOrEqualTo()
        {
            Name = "GREATERTHANOREQUALTO";
        }
    }

    public sealed class LesserThanOrEqualTo : Operator
    {
        public LesserThanOrEqualTo()
        {
            Name = "LESSERTHANOREQUALTO";
        }
    }

    public sealed class LesserThan : Operator
    {
        public LesserThan()
        {
            Name = "LESSERTHAN";
        }
    }

    public sealed class EqualTo : Operator
    {
        public EqualTo()
        {
            Name = "EQUALTO";
        }
    }

    public sealed class NotEqualTo : Operator
    {
        public NotEqualTo()
        {
            Name = "NOTEQUALTO";
        }
    }

    public sealed class GreaterThan : Operator
    {
        public GreaterThan()
        {
            Name = "GREATERTHAN";
        }
    }
}
