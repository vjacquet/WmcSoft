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

namespace WmcSoft.AI.FuzzyLogic
{
    public class FuzzyCategory : IFuzzyCategory
    {
        double highValue;
        double midValue;
        double lowValue;

        public FuzzyCategory(string displayName, double lowValue, double midValue, double highValue) {
            if (highValue < 0.0 || highValue > 1.0) {
                throw new ArgumentOutOfRangeException("highValue");
            }
            if (midValue < 0.0 || midValue > 1.0) {
                throw new ArgumentOutOfRangeException("midValue");
            }
            if (lowValue < 0.0 || lowValue > 1.0) {
                throw new ArgumentOutOfRangeException("lowValue");
            }

            this.highValue = highValue;
            this.midValue = midValue;
            this.lowValue = lowValue;

            this.displayName = displayName;
        }

        #region Membres de IFuzzySet

        public virtual double EvaluateMembership(double value) {
            if ((value <= lowValue) || (value >= highValue))
                return 0;
            else if (value > midValue)
                return (highValue - value) / (highValue - midValue);
            else if (value < midValue)
                return (lowValue - value) / (lowValue - midValue);
            else
                return 1;
        }

        #endregion

        public double LowValue {
            get { return this.lowValue; }
            set { this.lowValue = value; }
        }

        public double MidValue {
            get { return this.midValue; }
            set { this.midValue = value; }
        }

        public double HighValue {
            get { return this.highValue; }
            set { this.highValue = value; }
        }

        [DefaultValue("")]
        public string DisplayName {
            get { return displayName; }
            set { displayName = value; }
        }
        private string displayName;

        public override string ToString() {
            return displayName;
        }
    }
}
