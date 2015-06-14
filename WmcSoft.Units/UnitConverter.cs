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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace WmcSoft.Units
{
    /// <summary>
    /// Converts a quantity in a source unit to a quantity in a target unit.
    /// </summary>
    public static class UnitConverter
    {
        #region Fields

        static object syncRoot = new Object();
        static IDictionary<Unit, Hashtable> unitConversions;

        static UnitConverter() {
            unitConversions = new Dictionary<Unit, Hashtable>();
        }

        #endregion

        public delegate decimal ConvertCallback(decimal value);

        public static bool RegisterConversion(UnitConversion conversion) {
            return RegisterConversion(conversion, true);
        }

        static bool Register(Unit source, Unit target, ConvertCallback convert, bool throwOnDuplicates) {
            Hashtable table;
            if (!unitConversions.TryGetValue(source, out table)) {
                table = new Hashtable();
                table[target] = convert;
                unitConversions[source] = table;
                return true;
            } else if (!table.ContainsKey(target)) {
                table[target] = convert;
                return true;
            } else if (throwOnDuplicates) {
                throw new ArgumentException(RM.Format(RM.DuplicateConversionException, source, target));
            }
            return false;
        }

        internal static bool RegisterConversion(UnitConversion conversion, bool throwOnDuplicate) {
            lock (syncRoot) {
                if (unitConversions == null) {
                    unitConversions = new Dictionary<Unit, Hashtable>();
                }

                bool registered = Register(conversion.Source, conversion.Target, new ConvertCallback(conversion.Convert), throwOnDuplicate);
                registered |= Register(conversion.Target, conversion.Source, new ConvertCallback(conversion.ConvertBack), throwOnDuplicate);
                return registered;
            }
        }

        public static bool RegisterUnit(ScaledUnit scaledUnit) {
            if (!unitConversions.ContainsKey(scaledUnit)) {
                var factor = 1m;
                var source = scaledUnit;
                while (scaledUnit != null) {
                    factor *= scaledUnit.ScaleFactor;
                    RegisterConversion(new ExtrapolatedConversion(source, scaledUnit.Reference, factor), false);
                    scaledUnit = scaledUnit.Reference as ScaledUnit;
                }

                return true;
            }
            return false;
        }

        public static bool RegisterUnit(Unit unit) {
            if (!unitConversions.ContainsKey(unit)) {

                //if (scaledUnit != null) {
                //    RegisterConversion(new ExtrapolatedConversion(scaledUnit), false);
                //}
                bool registered = false;
                foreach (object attribute in unit.GetType().GetCustomAttributes(true)) {
                    var conversion = attribute as UnitConversionAttribute;
                    if (conversion != null) {
                        registered |= RegisterConversion(conversion.UnitConversion);
                    }
                }
                return registered;
            }
            return false;
        }

        public static Quantity Convert(Quantity quantity, Unit target) {
            if (quantity.Metric == (Metric)target)
                return quantity;

            var source = quantity.Metric as Unit;
            if (source == null)
                throw new ArgumentException(RM.GetString(RM.NotAUnitException), "quantity");

            RegisterUnit(source);
            RegisterUnit(target);

            Hashtable table;
            if (unitConversions.TryGetValue(source, out table)) {
                ConvertCallback callback = table[target] as ConvertCallback;
                if (callback != null) {
                    return new Quantity(callback(quantity.Amount), target);
                }
            }

            throw new NotSupportedException();
        }

        public static Quantity ConvertTo(this Quantity quantity, Unit target) {
            return Convert(quantity, target);
        }

        public static Quantity ConvertTo<U>(this Quantity quantity) where U : Unit {
            var result = Convert(quantity, Activator.CreateInstance<U>());
            return new Quantity<U>(result.Amount);
        }
    }
}
