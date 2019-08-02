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

using static System.Decimal;

namespace WmcSoft.Units
{
    /// <summary>
    /// Policy describing how to round decimal numbers.
    /// </summary>
    public class RoundingPolicy
    {
        public RoundingPolicy()
            : this(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalDigits, 5, RoundingStrategy.Round)
        {
        }

        public RoundingPolicy(int numberOfDigit)
            : this(numberOfDigit, 5, RoundingStrategy.Round)
        {
        }

        public RoundingPolicy(int numberOfDigit, int roundingDigit)
            : this(numberOfDigit, roundingDigit, RoundingStrategy.Round)
        {
        }

        public RoundingPolicy(int numberOfDigit, RoundingStrategy roundingStrategy)
            : this(numberOfDigit, 5, roundingStrategy)
        {
        }

        public RoundingPolicy(RoundingStrategy roundingStrategy)
            : this(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalDigits, 5, roundingStrategy)
        {
        }

        public RoundingPolicy(int numberOfDigit, int roundingDigit, RoundingStrategy roundingStrategy)
        {
            NumberOfDigit = numberOfDigit;
            RoundingDigit = roundingDigit;
            RoundingStrategy = roundingStrategy;
            RoundingStep = new decimal(Math.Pow(0.1, numberOfDigit));
        }

        public RoundingPolicy(decimal roundingStep)
            : this(roundingStep, RoundingStrategy.RoundUpByStep)
        {
        }

        public RoundingPolicy(decimal roundingStep, RoundingStrategy roundingStrategy)
            : this()
        {
            if ((roundingStrategy != RoundingStrategy.RoundUpByStep) && (roundingStrategy != RoundingStrategy.RoundDownByStep))
                throw new ArgumentOutOfRangeException(nameof(roundingStrategy));
            if (roundingStep == Zero)
                throw new DivideByZeroException();
            RoundingStrategy = roundingStrategy;
            RoundingStep = roundingStep;
        }

        public decimal Round(decimal value)
        {
            if (value == 0m)
                return 0m;

            var power = new decimal(Math.Pow(10, NumberOfDigit));
            switch (RoundingStrategy) {
            case RoundingStrategy.RoundUp:
                return value >= 0m
                    ? Ceiling(power * value) / power
                    : Floor(power * value) / power;
            case RoundingStrategy.RoundDown:
                return value >= 0m
                    ? Floor(power * value) / power
                    : Ceiling(power * value) / power;
            case RoundingStrategy.Round when RoundingDigit == 5:
                return value >= 0m
                    ? Floor(power * value + 0.5m) / power
                    : Floor(power * value - 0.5m) / power;
            case RoundingStrategy.Round:
                return value >= 0m
                    ? Floor(power * value + (1.0m - 0.1m * RoundingDigit)) / power
                    : Floor(power * value + (0.1m * RoundingDigit)) / power;
            case RoundingStrategy.RoundUpByStep:
                return value >= 0m
                    ? Ceiling(value / RoundingStep) * RoundingStep
                    : Floor(value / RoundingStep) * RoundingStep;
            case RoundingStrategy.RoundDownByStep:
                return value >= 0m
                    ? Floor(value / RoundingStep) * RoundingStep
                    : Ceiling(value / RoundingStep) * RoundingStep;
            case RoundingStrategy.RoundTowardsPositive:
                return Ceiling(power * value) / power;
            case RoundingStrategy.RoundTowardsNegative:
                return Floor(power * value) / power;
            }
            return value;
        }

        public int NumberOfDigit { get; }

        public int RoundingDigit { get; }

        public decimal RoundingStep { get; }

        public RoundingStrategy RoundingStrategy { get; }
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
