using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business
{
    public enum ModerationStatus
    {
        [RM.Description("ModerationStatus.Draft")]
        Draft,
        [RM.Description("ModerationStatus.Pending")]
        Pending,
        [RM.Description("ModerationStatus.Denied")]
        Denied,
        [RM.Description("ModerationStatus.Scheduled")]
        Scheduled,
        [RM.Description("ModerationStatus.Approved")]
        Approved,
    }
}
