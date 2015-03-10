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

namespace WmcSoft.Units
{
    /// <summary>
    /// Description résumée de ScaledUnit.
    /// </summary>
    public class ScaledUnit : Unit
    {
        #region Fields

        decimal scaleFactor;
        readonly Unit reference;
        readonly internal protected string definition;
        readonly internal protected string name;
        readonly internal protected string symbol;

        #endregion

        #region Lifecycle

        public ScaledUnit(decimal scaleFactor, Unit reference)
            : this(null, null, null, scaleFactor, reference) {
        }

        public ScaledUnit(string name, decimal scaleFactor, Unit reference)
            : this(name, null, null, scaleFactor, reference) {
        }

        public ScaledUnit(string name, string symbol, decimal scaleFactor, Unit reference)
            : this(name, symbol, null, scaleFactor, reference) {
        }

        public ScaledUnit(string name, string symbol, string definition, decimal scaleFactor, Unit reference) {
            if (reference == null)
                throw new ArgumentNullException("reference");
            if (symbol == null)
                throw new ArgumentNullException("symbol");
            if (name == null)
                throw new ArgumentNullException("name");
            if (scaleFactor <= Decimal.Zero)
                throw new ArgumentException(RM.GetString(RM.InvalidScaleFactorException), "scaleFactor");

            this.symbol = symbol;
            this.name = name;
            this.reference = reference;
            this.definition = definition;
            this.scaleFactor = scaleFactor;
        }

        #endregion

        #region Properties

        public decimal ScaleFactor {
            get { return scaleFactor; }
        }

        public Unit Reference {
            get { return reference; }
        }

        public override string Definition {
            get { return definition; }
        }

        public override string Name {
            get { return name; }
        }

        public override string Symbol {
            get { return this.symbol; }
        }

        public override SystemOfUnits SystemOfUnits {
            get { return reference.SystemOfUnits; }
        }

        #endregion

    }
}
