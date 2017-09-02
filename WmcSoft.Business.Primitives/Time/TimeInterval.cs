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
 * Adapted from TimeInterval.java
 * ------------------------------
 * Copyright (c) 2004 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

namespace WmcSoft.Time
{
    public static class TimeInterval
    {
        public static readonly Interval<Timepoint> Always = new Interval<Timepoint>(IntervalLimit<Timepoint>.UnboundedLower, IntervalLimit<Timepoint>.UnboundedUpper);

        public static Interval<Timepoint> Over(Timepoint start, bool closedStart, Timepoint end, bool closedEnd)
        {
            return Interval.Over(start, closedStart, end, closedEnd);
        }

        public static Interval<Timepoint> Over(Timepoint start, Timepoint end)
        {
            return Over(start, true, end, false);
        }

        public static Interval<Timepoint> StartingFrom(Timepoint start, bool startClosed, Duration length, bool endClosed)
        {
            Timepoint end = start + length;
            return Over(start, startClosed, end, endClosed);
        }

        /// <remarks>Uses the common default for time intervals, [start, end)</remarks>
        public static Interval<Timepoint> StartingFrom(Timepoint start, Duration length)
        {
            return StartingFrom(start, true, length, false);
        }

        public static Interval<Timepoint> Preceding(Timepoint end, bool startClosed, Duration length, bool endClosed)
        {
            Timepoint start = end - length;
            return Over(start, startClosed, end, endClosed);
        }

        /// <remarks>Uses the common default for time intervals, [start, end)</remarks>
        public static Interval<Timepoint> Preceding(Timepoint end, Duration length)
        {
            return Preceding(end, true, length, false);
        }

        public static Interval<Timepoint> Closed(Timepoint start, Timepoint end)
        {
            return Over(start, true, end, true);
        }

        public static Interval<Timepoint> Open(Timepoint start, Timepoint end)
        {
            return Over(start, false, end, false);
        }

        public static Interval<Timepoint> Since(Timepoint start)
        {
            return new Interval<Timepoint>(Interval.LowerLimit(start, true), IntervalLimit<Timepoint>.Undefined);
        }

        public static Interval<Timepoint> Until(Timepoint end)
        {
            return new Interval<Timepoint>(IntervalLimit<Timepoint>.Undefined, Interval.UpperLimit(end, false));
        }

        public static Duration? Length(this Interval<Timepoint> interval)
        {
            if (!interval.Lower.HasValue || !interval.Upper.HasValue)
                return null;

            return interval.Upper.GetValueOrDefault() - interval.Lower.GetValueOrDefault();
        }

        public static Timepoint Start(this Interval<Timepoint> interval)
        {
            return interval.Lower.Value;
        }

        public static Timepoint End(this Interval<Timepoint> interval)
        {
            return interval.Upper.Value;
        }
    }
}
