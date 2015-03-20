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
using System.Globalization;

namespace WmcSoft.Units
{
    /// <summary>
    /// Description r�sum�e de RoundingPolicy.
    /// </summary>
    public class RoundingPolicy
    {
        public RoundingPolicy()
            : this(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalDigits, 5, RoundingStrategy.Round) {
        }

        public RoundingPolicy(int numberOfDigit)
            : this(numberOfDigit, 5, RoundingStrategy.Round) {
        }

        public RoundingPolicy(int numberOfDigit, int roundingDigit)
            : this(numberOfDigit, numberOfDigit, RoundingStrategy.Round) {
        }

        public RoundingPolicy(int numberOfDigit, RoundingStrategy roundingStrategy)
            : this(numberOfDigit, 5, roundingStrategy) {
        }

        public RoundingPolicy(RoundingStrategy roundingStrategy)
            : this(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalDigits, 5, roundingStrategy) {
        }

        public RoundingPolicy(int numberOfDigit, int roundingDigit, RoundingStrategy roundingStrategy) {
            this.numberOfDigit = numberOfDigit;
            this.roundingDigit = roundingDigit;
            this.roundingStrategy = roundingStrategy;
            this.roundingStep = new Decimal(System.Math.Pow(0.1, numberOfDigit));
        }

        public RoundingPolicy(decimal roundingStep)
            : this(roundingStep, RoundingStrategy.RoundUpByStep) {
        }

        public RoundingPolicy(decimal roundingStep, RoundingStrategy roundingStrategy)
            : this() {
            if ((roundingStrategy != RoundingStrategy.RoundUpByStep) && (roundingStrategy != RoundingStrategy.RoundDownByStep))
                throw new ArgumentOutOfRangeException("roundingStrategy");
            if (roundingStep == Decimal.Zero)
                throw new DivideByZeroException();
            this.roundingStrategy = roundingStrategy;
            this.roundingStep = roundingStep;
        }

        public decimal Round(decimal value) {
            Decimal power = new Decimal(System.Math.Pow(10, this.numberOfDigit));
            switch (roundingStrategy) {
            case RoundingStrategy.RoundUp:
                if (value >= 0)
                    return Decimal.Ceiling(power * value) / power;
                else
                    return Decimal.Floor(power * value) / power;
            case RoundingStrategy.RoundDown:
                if (value >= 0)
                    return Decimal.Floor(power * value) / power;
                else
                    return Decimal.Ceiling(power * value) / power;
            case RoundingStrategy.Round:
                if (roundingDigit == 5) {
                    //return Math.Round(value, numberOfDigit);
                    if (value >= 0)
                        return Decimal.Floor(power * value + 0.5m) / power;
                    else
                        return Decimal.Floor(power * value - 0.5m) / power;
                } else {
                    if (value >= 0)
                        return Decimal.Floor(power * value + (1.0m - 0.1m * roundingDigit)) / power;
                    else
                        return Decimal.Floor(power * value + (0.1m * roundingDigit)) / power;
                }
            case RoundingStrategy.RoundUpByStep:
                if (value >= 0)
                    return Decimal.Ceiling(value / roundingStep) * roundingStep;
                else
                    return Decimal.Floor(value / roundingStep) * roundingStep;
            case RoundingStrategy.RoundDownByStep:
                if (value >= 0)
                    return Decimal.Floor(value / roundingStep) * roundingStep;
                else
                    return Decimal.Ceiling(value / roundingStep) * roundingStep;
            case RoundingStrategy.RoundTowardsPositive:
                return Decimal.Ceiling(power * value) / power;
            case RoundingStrategy.RoundTowardsNegative:
                return Decimal.Floor(power * value) / power;
            }
            return value;
        }

        public int NumberOfDigit {
            get {
                return this.numberOfDigit;
            }
        } int numberOfDigit;

        public int RoundingDigit {
            get {
                return this.roundingDigit;
            }
        } int roundingDigit;

        public decimal RoundingStep {
            get {
                return this.roundingStep;
            }
        } decimal roundingStep;

        public RoundingStrategy RoundingStrategy {
            get {
                return this.roundingStrategy;
            }
        } RoundingStrategy roundingStrategy;
    }

    public enum RoundingStrategy
    {
        RoundUp,
        RoundDown,
        Round,
        RoundUpByStep,
        RoundDownByStep,
        RoundTowardsPositive,
        RoundTowardsNegative
    }
}