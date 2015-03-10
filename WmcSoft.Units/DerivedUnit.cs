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
using System.Text;

namespace WmcSoft.Units
{
    /// <summary>
    /// Represents the combination of one or more base units according to 
    /// a specific equation.
    /// </summary>
    public class DerivedUnit : Unit
    {
        #region Fields

        readonly protected string _name;
        readonly protected string _symbol;
        readonly protected string _definition;
        readonly SystemOfUnits _systemOfUnits;
        readonly DerivedUnitTerm[] _terms;

        #endregion

        #region Lifecycle

        public DerivedUnit(params DerivedUnitTerm[] terms)
            : this(null, null, null, null, terms) {
        }

        public DerivedUnit(string name, params DerivedUnitTerm[] terms)
            : this(name, null, null, null, terms) {
        }

        public DerivedUnit(string name, string symbol, params DerivedUnitTerm[] terms)
            : this(name, symbol, null, null, terms) {
        }

        public DerivedUnit(string name, SystemOfUnits systemOfUnits, params DerivedUnitTerm[] terms)
            : this(name, null, null, systemOfUnits, terms) {
        }

        public DerivedUnit(string name, string symbol, SystemOfUnits systemOfUnits, params DerivedUnitTerm[] terms)
            : this(name, symbol, null, systemOfUnits, terms) {
        }

        public DerivedUnit(string name, string symbol, string definition, params DerivedUnitTerm[] terms)
            : this(name, symbol, definition, null, terms) {
        }

        public DerivedUnit(string name, string symbol, string definition, SystemOfUnits systemOfUnits, params DerivedUnitTerm[] terms) {
            if (terms == null)
                throw new ArgumentNullException("terms");
            this._terms = terms;
            this._definition = definition;
            this._name = name;
            this._symbol = symbol;
            this._systemOfUnits = systemOfUnits;
        }

        public DerivedUnit(string name, string symbol, Unit unit)
            : this(name, symbol, null, null, new[] { new DerivedUnitTerm(unit) }) {
        }

        #endregion

        #region Properties

        public DerivedUnitTerm[] Terms {
            get { return _terms; }
        }

        public override string Definition {
            get { return _definition; }
        }

        public override string Name {
            get { return _name; }
        }

        public override string Symbol {
            get {
                if (this._symbol == null) {
                    var builder = new StringBuilder();
                    foreach (DerivedUnitTerm term in this._terms) {
                        string symbol = term.Symbol;
                        if (symbol == null)
                            return null;
                        builder.Append(symbol);
                    }
                    return builder.ToString();
                }
                return this._symbol;
            }
        }

        public override SystemOfUnits SystemOfUnits {
            get { return _systemOfUnits; }
        }

        #endregion

        //		public override bool Equals(object obj)
        //		{
        //			if(base.Equals (obj))
        //			{
        //				DerivedUnit that = (DerivedUnit)obj;
        //				if(terms.Length == that.terms.Length)
        //				{
        //					for(int i = 0; i != terms.Length; ++i)
        //					{
        //						if(!terms[i].Equals(that.terms[i]))
        //							return false;
        //					}
        //					return true;
        //				}
        //			}
        //			return false;
        //		}
        //
        //		public override int GetHashCode()
        //		{
        //			int hash = base.GetHashCode ();
        //			for(int i = 0; i != terms.Length; ++i)
        //			{
        //				hash ^= terms[i].GetHashCode();
        //			}
        //			return hash;
        //		}
    }
}
