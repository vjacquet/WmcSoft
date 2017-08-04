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
    public static class Proration
    {
        private static decimal SumNonEmpty(decimal[] elements, decimal sum = 0m)
        {
            for (int i = 0; i < elements.Length; i++) {
                sum += elements[i];
            }
            return sum;
        }

        public static decimal Sum(params decimal[] elements)
        {
            if (elements == null || elements.Length == 0)
                return 0m;
            return SumNonEmpty(elements);
        }

        public static Ratio[] Ratios(params decimal[] proportions)
        {
            if (proportions == null)
                return null;
            if (proportions.Length == 0)
                return new Ratio[0];
            var total = SumNonEmpty(proportions);
            var ratios = new Ratio[proportions.Length];
            for (int i = 0; i < proportions.Length; i++) {
                ratios[i] = new Ratio(proportions[i], total);
            }
            return ratios;
        }

        static decimal[] DistributeRemainderOver(decimal[] amounts, decimal remainder, decimal minimumIncrement)
        {
            Debug.Assert(minimumIncrement < 1m);
            int increments = (int)(remainder / minimumIncrement);
            Debug.Assert(increments <= amounts.Length);

            var results = (decimal[])amounts.Clone();
            for (int i = 0; i < increments; i++)
                results[i] = amounts[i] + minimumIncrement;
            return results;
        }

        public static decimal[] DividedEvenlyIntoParts(decimal total, int n, decimal minimumIncrement)
        {
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
