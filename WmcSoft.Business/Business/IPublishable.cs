using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
