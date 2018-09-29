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

 ****************************************************************************
 * Adapted from Proration.java
 * ---------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System;
using System.Diagnostics;

namespace WmcSoft
{
    /// <summary>
    /// Helper class to deal with proration of amounts.
    /// </summary>
    public static class Proration
    {
        private static decimal SumNonEmpty(decimal[] elements)
        {
            var sum = elements[0];
            for (int i = 1; i < elements.Length; i++) {
                sum += elements[i];
            }
            return sum;
        }

        /// <summary>
        /// Sums the <paramref name="elements"/>.
        /// </summary>
        /// <param name="elements">The elements to sum.</param>
        /// <returns>The sum of the elements.</returns>
        public static decimal Sum(params decimal[] elements)
        {
            if (elements != null && elements.Length > 0)
                return SumNonEmpty(elements);
            return 0m;
        }

        /// <summary>
        /// Compute the ratio of each proportions in the total.
        /// </summary>
        /// <param name="proportions">The proportions.</param>
        /// <returns>The corresponding ratio of each proportion.</returns>
        public static Ratio[] Ratios(params decimal[] proportions)
        {
            if (proportions == null)
                return null;
            if (proportions.Length == 0)
                return new Ratio[0];
            var total = SumNonEmpty(proportions);
            return Array.ConvertAll(proportions, p => new Ratio(p, total));
        }

        static decimal[] DistributeRemainderOver(decimal[] amounts, decimal remainder, decimal minimumIncrement)
        {
            Debug.Assert(minimumIncrement < 1m);
            int increments = (int)(remainder / minimumIncrement); // what about the remainder of the remainder ?
            Debug.Assert(increments <= amounts.Length);

            var results = (decimal[])amounts.Clone();
            for (int i = 0; i < increments; i++)
                results[i] = amounts[i] + minimumIncrement;
            return results;
        }

        /// <summary>
        /// Divides the <paramref name="total"/> in <paramref name="n"/> shares of the same amount.
        /// </summary>
        /// <param name="total">The total to distribute.</param>
        /// <param name="n">The number of shares.</param>
        /// <param name="minimumIncrement">The minimalIncrement</param>
        /// <returns>The shares.</returns>
        public static decimal[] DividedEvenlyIntoParts(decimal total, int n, decimal minimumIncrement)
        {
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));

            var lowResult = total / n;
            var lowResults = new decimal[n];
            for (int i = 0; i < n; i++)
                lowResults[i] = lowResult;
            var remainder = total - SumNonEmpty(lowResults);
            return DistributeRemainderOver(lowResults, remainder, minimumIncrement);
        }

#if SOURCE
	private static int defaultScaleForIntermediateCalculations(Money total) {
		return total.getCurrency().getDefaultFractionDigits() + 1;
	}

	public Money[] proratedOver(Money total, long[] longProportions) {
		BigDecimal[] proportions = new BigDecimal[longProportions.length];
		for (int i = 0; i < longProportions.length; i++) {
			proportions[i] = BigDecimal.valueOf(longProportions[i]);
		}
		return proratedOver(total, proportions);
	}
	
	public Money[] proratedOver(Money total, BigDecimal[] proportions) {
		Money[] simpleResult = new Money[proportions.length];
		int scale = defaultScaleForIntermediateCalculations(total);
		Ratio[] ratios = ratios(proportions);
		for (int i = 0; i < ratios.length; i++) {
			BigDecimal multiplier = ratios[i].decimalValue(scale, Rounding.DOWN);
			simpleResult[i] = total.times(multiplier, Rounding.DOWN);
		}
		Money remainder = total.minus(sum(simpleResult));
		return distributeRemainderOver(simpleResult, remainder);
	}

	public Money partOfWhole(Money total, long portion, long whole) {
		return partOfWhole(total, Ratio.of(portion, whole));
	}

	public Money partOfWhole(Money total, Ratio ratio) {
		int scale = defaultScaleForIntermediateCalculations(total);
		BigDecimal multiplier = ratio.decimalValue(scale, Rounding.DOWN);
		return total.times(multiplier, Rounding.DOWN);
	}

#endif
    }
}
