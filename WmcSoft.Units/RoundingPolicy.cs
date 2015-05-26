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
    /// Policy describing how to round decimal numbers.
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
            _numberOfDigit = numberOfDigit;
            _roundingDigit = roundingDigit;
            _roundingStrategy = roundingStrategy;
            _roundingStep = new Decimal(System.Math.Pow(0.1, numberOfDigit));
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
            _roundingStrategy = roundingStrategy;
            _roundingStep = roundingStep;
        }

        public decimal Round(decimal value) {
            Decimal power = new Decimal(System.Math.Pow(10, _numberOfDigit));
            switch (_roundingStrategy) {
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
                if (_roundingDigit == 5) {
                    //return Math.Round(value, numberOfDigit);
                    if (value >= 0)
                        return Decimal.Floor(power * value + 0.5m) / power;
                    else
                        return Decimal.Floor(power * value - 0.5m) / power;
                } else {
                    if (value >= 0)
                        return Decimal.Floor(power * value + (1.0m - 0.1m * _roundingDigit)) / power;
                    else
                        return Decimal.Floor(power * value + (0.1m * _roundingDigit)) / power;
                }
            case RoundingStrategy.RoundUpByStep:
                if (value >= 0)
                    return Decimal.Ceiling(value / _roundingStep) * _roundingStep;
                else
                    return Decimal.Floor(value / _roundingStep) * _roundingStep;
            case RoundingStrategy.RoundDownByStep:
                if (value >= 0)
                    return Decimal.Floor(value / _roundingStep) * _roundingStep;
                else
                    return Decimal.Ceiling(value / _roundingStep) * _roundingStep;
            case RoundingStrategy.RoundTowardsPositive:
                return Decimal.Ceiling(power * value) / power;
            case RoundingStrategy.RoundTowardsNegative:
                return Decimal.Floor(power * value) / power;
            }
            return value;
        }

        public int NumberOfDigit {
            get { return _numberOfDigit; }
        }
        readonly int _numberOfDigit;

        public int RoundingDigit {
            get { return _roundingDigit; }
        }
        readonly int _roundingDigit;

        public decimal RoundingStep {
            get { return _roundingStep; }
        }
        readonly decimal _roundingStep;

        public RoundingStrategy RoundingStrategy {
            get { return _roundingStrategy; }
        }
        readonly RoundingStrategy _roundingStrategy;
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
