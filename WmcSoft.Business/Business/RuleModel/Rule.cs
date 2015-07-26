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
using System.Collections;
using System.Xml.Serialization;

namespace WmcSoft.Business.RuleModel
{
    /// <remarks/>
    [XmlRoot("rule", Namespace = "", IsNullable = false)]
    public class Rule : IRuleEvaluator
    {
        /// <remarks/>
        [XmlElement("xor", typeof(XOr))]
        [XmlElement("rule", typeof(RuleRef))]
        [XmlElement("variable", typeof(Variable))]
        [XmlElement("lesserThan", typeof(LesserThan))]
        [XmlElement("lesserThanOrEqualTo", typeof(LesserThanOrEqualTo))]
        [XmlElement("notEqualTo", typeof(NotEqualTo))]
        [XmlElement("and", typeof(And))]
        [XmlElement("not", typeof(Not))]
        [XmlElement("or", typeof(Or))]
        [XmlElement("greaterThanOrEqualTo", typeof(GreaterThanOrEqualTo))]
        [XmlElement("proposition", typeof(Proposition))]
        [XmlElement("greaterThan", typeof(GreaterThan))]
        [XmlElement("equalTo", typeof(EqualTo))]
        public RuleElement[] Items;

        /// <remarks/>

        /// <remarks/>
        [XmlAttribute("name", DataType = "ID")]
        public string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }
        string name;

        #region Membres de IRuleEvaluator

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Evaluate(RuleContext context) {
            //System.Diagnostics.Debugger.Break();

            Stack stack = new Stack();
            IEnumerator enumerator = this.Items.GetEnumerator();
            while (enumerator.MoveNext()) {
                RuleElement element = (RuleElement)enumerator.Current;
                if (!(element is Operator)) {
                    if (element is Proposition) {
                        stack.Push(context[element.Name]);
                    } else if (element is Variable) {
                        stack.Push(context[element.Name]);
                    } else {
                        throw new UnexpectedElementException();
                    }
                } else {
                    switch (element.Name) {
                    case "NOT":
                        stack.Push(new Proposition(!PopProposition(stack).Value));
                        break;
                    case "AND":
                        stack.Push(new Proposition(PopProposition(stack).Value && PopProposition(stack).Value));
                        break;
                    case "OR":
                        stack.Push(new Proposition(PopProposition(stack).Value || PopProposition(stack).Value));
                        break;
                    case "XOR":
                        stack.Push(new Proposition(PopProposition(stack).Value != PopProposition(stack).Value));
                        break;
                    case "EQUALTO":
                        stack.Push(new Proposition(PopVariable(stack).Value.CompareTo(PopVariable(stack).Value) == 0));
                        break;
                    case "NOTEQUALTO":
                        stack.Push(new Proposition(PopVariable(stack).Value.CompareTo(PopVariable(stack).Value) != 0));
                        break;
                    case "GREATERTHAN":
                        stack.Push(new Proposition(PopVariable(stack).Value.CompareTo(PopVariable(stack).Value) > 0));
                        break;
                    case "LESSERTHAN":
                        stack.Push(new Proposition(PopVariable(stack).Value.CompareTo(PopVariable(stack).Value) < 0));
                        break;
                    case "GREATERTHANOREQUALTO":
                        stack.Push(new Proposition(PopVariable(stack).Value.CompareTo(PopVariable(stack).Value) >= 0));
                        break;
                    case "LESSERTHANOREQUALTO":
                        stack.Push(new Proposition(PopVariable(stack).Value.CompareTo(PopVariable(stack).Value) <= 0));
                        break;
                    default:
                        throw new UnexpectedElementException();
                    }
                }
            }

            if (stack.Count != 1)
                throw new InvalidOperationException();
            Proposition result = PopProposition(stack);
            return result.Value;
        }

        private static Proposition PopProposition(Stack stack) {
            RuleElement element = (RuleElement)stack.Pop();
            return (Proposition)element;
        }

        private static Variable PopVariable(Stack stack) {
            RuleElement element = (RuleElement)stack.Pop();
            return (Variable)element;
        }

        #endregion
    }
}
