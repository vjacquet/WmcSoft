using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Units
{
    class IdentityConversion : UnitConversion
    {
        public IdentityConversion(Unit unit)
            : base(unit, unit) {
        }

        public override decimal Convert(decimal value) {
            return value;
        }

        public override decimal ConvertBack(decimal value) {
            return value;
        }
    }
}
