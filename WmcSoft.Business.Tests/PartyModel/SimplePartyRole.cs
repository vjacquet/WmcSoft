using System.Diagnostics;

namespace WmcSoft.Business.PartyModel
{
    [DebuggerStepThrough]
    public abstract class SimplePartyRole : PartyRole
    {
        protected SimplePartyRole(Party party)
            : base(party) {
        }

        public override string Name {
            get { return GetType().Name; }
        }

        public override string Description {
            get { return null; }
        }
    }
}
