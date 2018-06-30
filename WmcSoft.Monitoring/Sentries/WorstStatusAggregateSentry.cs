#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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


namespace WmcSoft.Monitoring.Sentries
{
    /// <summary>
    /// Represents a sentry that observes and returns the worst status of other sentries.
    /// </summary>
    public class WorstStatusAggregateSentry : AggregateSentry
    {
        public WorstStatusAggregateSentry(string name, params ISentry[] sentries)
            : base(name, sentries)
        {
        }

        protected override SentryStatus AggregateStatus(ISentry[] sentries)
        {
            var result = SentryStatus.None;
            foreach (var sentry in sentries) {
                switch (sentry.Status) {
                case SentryStatus.Success:
                    if (result != SentryStatus.Warning)
                        result = SentryStatus.Success;
                    break;
                case SentryStatus.Warning:
                    result = SentryStatus.Warning;
                    break;
                case SentryStatus.Error:
                    return SentryStatus.Error;
                default:
                    break;
                }
            }
            return result;
        }
    }
}
