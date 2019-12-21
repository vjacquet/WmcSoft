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
 * Adapted from DateSpecification.java
 * -----------------------------------
 * Copyright (c) 2005 Domain Language, Inc. (http://domainlanguage.com) This
 * free software is distributed under the "MIT" licence. See file licence.txt.
 * For more information, see http://timeandmoney.sourceforge.net.
 ****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;

using static WmcSoft.Time.Algorithms;

namespace WmcSoft.Time
{
    public static class DateSpecificationExtensions
    {
        static Date GetLowerInclusive(Interval<Date> interval)
        {
            if (!interval.HasLowerLimit)
                return Date.MinValue;
            else if (interval.IsLowerIncluded)
                return interval.GetLowerOrDefault();
            else
                return interval.GetLowerOrDefault().AddDays(1);
        }

        static Date GetUpperInclusive(Interval<Date> interval)
        {
            if (!interval.HasUpperLimit)
                return Date.MaxValue;
            else if (interval.IsUpperIncluded)
                return interval.GetUpperOrDefault();
            else
                return interval.GetUpperOrDefault().AddDays(-1);
        }

        public static IEnumerable<Date> EnumerateOver<TSpecification>(this TSpecification specification, Interval<Date> interval)
            where TSpecification : IDateSpecification
        {
            if (interval.IsEmpty())
                return Enumerable.Empty<Date>();
            var since = GetLowerInclusive(interval);
            var until = GetUpperInclusive(interval);
            return specification.EnumerateBetween(since, until);
        }

        public static Date? FirstOccurrenceIn<TSpecification>(this TSpecification specification, Date since, Date until)
            where TSpecification : IDateSpecification
        {
            return specification.EnumerateBetween(since, until).FirstOrDefault();
        }

        public static Date? FirstOccurrenceIn<TSpecification>(this TSpecification specification, Interval<Date> interval)
            where TSpecification : IDateSpecification
        {
            return EnumerateOver(specification, interval).FirstOrDefault();
        }

        /// <summary>
        /// Creates a new specification applicable only since the given <paramref name="date"/>.
        /// </summary>
        /// <param name="specification">The specification to decorate.</param>
        /// <param name="date">The date</param>
        /// <returns>A new specification applicable only since the given <paramref name="date"/></returns>
        public static IDateSpecification ApplicableSince(this IDateSpecification specification, Date date)
        {
            return new SinceDateSpecification(specification, date);
        }

        /// <summary>
        /// Creates a new specification applicable only until the given <paramref name="date"/>.
        /// </summary>
        /// <param name="specification">The specification to decorate.</param>
        /// <param name="date">The date</param>
        /// <returns>A new specification applicable only until the given <paramref name="date"/></returns>
        public static IDateSpecification ApplicableUntil(this IDateSpecification specification, Date date)
        {
            return new UntilDateSpecification(specification, date);
        }

        #region Decorators

        class SinceDateSpecification : IDateSpecification
        {
            private readonly IDateSpecification _underlying;
            private readonly Date _since;

            public SinceDateSpecification(IDateSpecification underlying, Date since)
            {
                _underlying = underlying;
                _since = since;
            }

            public IEnumerable<Date> EnumerateBetween(Date since, Date until)
            {
                since = Max(since, _since);
                if (since > until)
                    return Enumerable.Empty<Date>();
                return _underlying.EnumerateBetween(since, until);
            }

            public bool IsSatisfiedBy(Date candidate)
            {
                if (_since <= candidate)
                    return _underlying.IsSatisfiedBy(candidate);
                return false;
            }
        }

        class UntilDateSpecification : IDateSpecification
        {
            private readonly IDateSpecification _underlying;
            private readonly Date _until;

            public UntilDateSpecification(IDateSpecification underlying, Date until)
            {
                _underlying = underlying;
                _until = until;
            }

            public IEnumerable<Date> EnumerateBetween(Date since, Date until)
            {
                until = Min(until, _until);
                if (since > until)
                    return Enumerable.Empty<Date>();
                return _underlying.EnumerateBetween(since, until);
            }

            public bool IsSatisfiedBy(Date candidate)
            {
                if (candidate <= _until)
                    return _underlying.IsSatisfiedBy(candidate);
                return false;
            }
        }

        #endregion
    }
}
