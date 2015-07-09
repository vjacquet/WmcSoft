using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Units
{
    class CompositeConversion : UnitConversion
    {
        private readonly List<UnitConversion> _sequence;

        internal CompositeConversion(UnitConversion x, UnitConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(2);
            _sequence.Add(x);
            _sequence.Add(y);
        }

        internal CompositeConversion(UnitConversion x, CompositeConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(y._sequence.Count + 1);
            _sequence.Add(x);
            _sequence.AddRange(y._sequence);
        }

        internal CompositeConversion(CompositeConversion x, UnitConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(x._sequence.Count + 1);
            _sequence.AddRange(x._sequence);
            _sequence.Add(y);
        }

        internal CompositeConversion(CompositeConversion x, CompositeConversion y)
            : base(x.Source, y.Target) {
            _sequence = new List<UnitConversion>(x._sequence.Count + y._sequence.Count);
            _sequence.AddRange(x._sequence);
            _sequence.AddRange(y._sequence);
        }

        internal CompositeConversion(params UnitConversion[] conversions)
            : base(conversions.First().Source, conversions.Last().Target) {
            // ensures the conversions forms a path.
            for (int i = 1; i < conversions.Length; i++) {
                if (conversions[i - 1].Target != conversions[i].Source)
                    throw new ArgumentException("conversions");
            }
            _sequence = new List<UnitConversion>(conversions);
        }

        public override decimal Convert(decimal value) {
            for (int i = 0; i < _sequence.Count; i++) {
                value = _sequence[i].Convert(value);
            }
            return value;
        }

        public override decimal ConvertBack(decimal value) {
            for (int i = _sequence.Count - 1; i >= 0; i--) {
                value = _sequence[i].ConvertBack(value);
            }
            return value;
        }
    }
}
