using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WmcSoft.Business.PartyModel;

namespace WmcSoft
{
    public interface ISupportPreferences
    {
        WeightedPreferenceCollection Preferences { get; }
    }
}
