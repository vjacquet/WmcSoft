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

namespace WmcSoft.Business
{
    public interface IPublishable
    {
        ModerationStatus ModerationStatus { get; set; }
    }

    public static class PublishableExtensions
    {
        public static void RequestApproval(IPublishable publishable) {
            var flags = publishable.ModerationStatus;
            switch (flags) {
            case ModerationStatus.Draft:
            case ModerationStatus.Denied:
                publishable.ModerationStatus = ModerationStatus.Pending;
                break;
            case ModerationStatus.Scheduled:
            case ModerationStatus.Approved:
            default:
                throw new InvalidOperationException();
            }
        }

        public static void Deny(IPublishable publishable) {
            publishable.ModerationStatus = ModerationStatus.Denied;
        }

        public static void Publish(this IPublishable publishable) {
            publishable.ModerationStatus = ModerationStatus.Approved;
        }

        public static void Unpublish<P>(this IPublishable publishable) {
            publishable.ModerationStatus = ModerationStatus.Draft;
        }

        public static void Publish<P>(this P publishable, DateTime when)
            where P : IPublishable, ITemporal {
            publishable.ModerationStatus = ModerationStatus.Approved;
            if (!publishable.ValidSince.HasValue || publishable.ValidSince.GetValueOrDefault() > when)
                publishable.ValidSince = when;
            publishable.ValidUntil = null;
        }

        public static void Schedule<P>(this P publishable, DateTime since, DateTime? until)
            where P : IPublishable, ITemporal {
            publishable.ModerationStatus = ModerationStatus.Scheduled;
            publishable.ValidSince = since;
            publishable.ValidUntil = until;
        }
    }
}
