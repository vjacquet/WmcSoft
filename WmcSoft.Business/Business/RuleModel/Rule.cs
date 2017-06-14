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
using System.Xml.Serialization;

namespace WmcSoft.Business.RuleModel
{
    /// <summary>
    /// Represents a constraint on the operation of the software systems of the business.
    /// Its semantics are defined by a sequence of <see cref="RuleElement"/>.
    /// </summary>
    [XmlRoot("rule", Namespace = @"http://www.wmcsoft.fr/schemas/2015/business/RuleModel.xsd", IsNullable = false)]
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
        public string Name { get; set; }

        #region Membres de IRuleEvaluator

        class Evaluator
        {
            readonly Stack<RuleElement> _stack;
            readonly RuleContext _context;

            public Evaluator(RuleContext context)
            {
                _context = context ?? new RuleContext();
                _stack = new Stack<RuleElement>();
            }


            private Proposition PopProposition()
            {
                var element = _stack.Pop();
                return (Proposition)element;
            }

            private Variable PopVariable()
            {
                var element = _stack.Pop();
                return (Variable)element;
            }

            public bool Evaluate(IEnumerable<RuleElement> ruleElements)
            {
                _stack.Clear();
                //System.Diagnostics.Debugger.Break();

                using (var enumerator = ruleElements.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        var element = enumerator.Current;
                        if (element is Operator) {
                            switch (element.Name) {
                            case "NOT":
                                _stack.Push(new Proposition(!PopProposition().Value));
                                break;
                            case "AND":
                                _stack.Push(new Proposition(PopProposition().Value & PopProposition().Value));
                                break;
                            case "OR":
                                _stack.Push(new Proposition(PopProposition().Value | PopProposition().Value));
                                break;
                            case "XOR":
                                _stack.Push(new Proposition(PopProposition().Value ^ PopProposition().Value));
                                break;
                            case "EQUALTO":
                                _stack.Push(new Proposition(PopVariable().Value.CompareTo(PopVariable().Value) == 0));
                                break;
                            case "NOTEQUALTO":
                                _stack.Push(new Proposition(PopVariable().Value.CompareTo(PopVariable().Value) != 0));
                                break;
                            case "GREATERTHAN":
                                _stack.Push(new Proposition(PopVariable().Value.CompareTo(PopVariable().Value) > 0));
                                break;
                            case "LESSERTHAN":
                                _stack.Push(new Proposition(PopVariable().Value.CompareTo(PopVariable().Value) < 0));
                                break;
                            case "GREATERTHANOREQUALTO":
                                _stack.Push(new Proposition(PopVariable().Value.CompareTo(PopVariable().Value) >= 0));
                                break;
                            case "LESSERTHANOREQUALTO":
                                _stack.Push(new Proposition(PopVariable().Value.CompareTo(PopVariable().Value) <= 0));
                                break;
                            default:
                                throw new UnexpectedElementException();
                            }
                        } else if (element is Proposition) {
                            _stack.Push(_context[element.Name] ?? element);
                        } else if (element is Variable) {
                            _stack.Push(_context[element.Name] ?? element);
                        } else {
                            throw new UnexpectedElementException();
                        }
                    }
                }
                if (_stack.Count != 1)
                    throw new InvalidOperationException();
                var result = PopProposition();
                return result.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Evaluate(RuleContext context)
        {
            var evaluator = new Evaluator(context);
            return evaluator.Evaluate(Items);
        }

        #endregion
    }
}
