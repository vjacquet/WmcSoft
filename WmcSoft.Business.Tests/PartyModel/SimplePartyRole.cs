using System.Diagnostics;

namespace WmcSoft.Business.PartyModel
{
    [DebuggerStepThrough]
    public abstract class SimplePartyRole : PartyRole
    {
        protected SimplePartyRole(Party party)
            : base(party) {
        }
    }
}
