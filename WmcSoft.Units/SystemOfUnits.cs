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
    /// Represents a set of related Units defined by a standard such as SI.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class SystemOfUnits
    {
        #region Fields

        private readonly string _nameOfSystem;
        private readonly string _nameOfStandardizationBody;

        #endregion

        #region Lifecycle

        protected SystemOfUnits(string nameOfSystem, string nameOfStandardizationBody) {
            _nameOfSystem = nameOfSystem;
            _nameOfStandardizationBody = nameOfStandardizationBody;
        }

        #endregion

        #region Properties

        public virtual string NameOfSystem {
            get {
                return _nameOfSystem;
            }
        }

        public string NameOfStandardizationBody {
            get {
                return _nameOfStandardizationBody;
            }
        }

        #endregion

        #region Global access

        public static SI SI {
            get { return SI.GetSystemOfUnit(); }
        }
        public static ImperialSystemOfUnit Imperial {
            get { return ImperialSystemOfUnit.GetSystemOfUnit(); }
        }
        public static USCustomarySystemOfUnit USCustomary {
            get { return USCustomarySystemOfUnit.GetSystemOfUnit(); }
        }

        public static NaturalSystemOfUnit Natural {
            get {
                return NaturalSystemOfUnit.GetSystemOfUnit();
            }
        }

        // SI
        // ImperialUnits
        // PlanckUnits
        // USCustomaryUnits
        // cgs
        // mts

        #endregion

        #region Overrides

        public override bool Equals(object obj) {
            if ((obj == null) || (GetType() != obj.GetType()))
                return false;
            SystemOfUnits that = (SystemOfUnits)obj;
            return (_nameOfSystem == that._nameOfSystem) && (_nameOfStandardizationBody == that._nameOfStandardizationBody);
        }

        public override int GetHashCode() {
            int hash = 0;

            if (_nameOfSystem != null)
                hash ^= _nameOfSystem.GetHashCode();

            if (_nameOfStandardizationBody != null)
                hash ^= _nameOfStandardizationBody.GetHashCode() << 4;

            return hash;
        }

        #endregion
        //		public static bool operator ==(SystemOfUnits left, SystemOfUnits right)
        //		{
        //			return (left.nameOfSystem == right.nameOfSystem) && (left.nameOfStandardizationBody == right.nameOfStandardizationBody);
        //		}
        //
        //		public static bool operator !=(SystemOfUnits left, SystemOfUnits right)
        //		{
        //			return (left.nameOfSystem != right.nameOfSystem) || (left.nameOfStandardizationBody != right.nameOfStandardizationBody);
        //		}
        //

    }
}
