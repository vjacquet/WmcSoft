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
using System.Diagnostics;
using WmcSoft.Units.Properties;

namespace WmcSoft.Units
{
    /// <summary>
    /// Represents a unit with a different scale of the reference unit.
    /// </summary>
    [DebuggerDisplay("{_name,nq} = {_scaleFactor,nq} * {_reference.ToString(\"s\"),nq}")]
    public class ScaledUnit : Unit
    {
        #region Fields

        readonly decimal _scaleFactor;
        readonly Unit _reference;
        readonly internal protected string _definition;
        readonly internal protected string _name;
        readonly internal protected string _symbol;

        #endregion

        #region Lifecycle

        public ScaledUnit(string name, decimal scaleFactor, Unit reference)
            : this(name, null, null, scaleFactor, reference) {
        }

        public ScaledUnit(string name, string symbol, decimal scaleFactor, Unit reference)
            : this(name, symbol, null, scaleFactor, reference) {
        }

        public ScaledUnit(string name, string symbol, string definition, decimal scaleFactor, Unit reference) {
            if (reference == null) throw new ArgumentNullException("reference");
            if (name == null) throw new ArgumentNullException("name");
            if (scaleFactor <= Decimal.Zero) throw new ArgumentException(Resources.InvalidScaleFactorException, "scaleFactor");

            _reference = reference;
            _symbol = symbol ?? name;
            _name = name;
            _definition = definition;
            _scaleFactor = scaleFactor;

            UnitConverter.RegisterUnit(this);
        }

        #endregion

        #region Properties

        public decimal ScaleFactor {
            get { return _scaleFactor; }
        }

        public Unit Reference {
            get { return _reference; }
        }

        public override string Definition {
            get { return _definition; }
        }

        public override string Name {
            get { return _name; }
        }

        public override string Symbol {
            get { return _symbol; }
        }

        public override SystemOfUnits SystemOfUnits {
            get { return _reference.SystemOfUnits; }
        }

        #endregion
    }
}
