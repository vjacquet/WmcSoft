using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.PartyModel
{
    [DebuggerStepThrough]
    public abstract class SimplePartyRelationship : PartyRelationship
    {
        protected SimplePartyRelationship(PartyRole client, PartyRole supplier)
            : base(client, supplier) {
        }

        public override string Name {
            get { return GetType().Name; }
        }

        public override string Description {
            get { return null; }
        }
    }
}
