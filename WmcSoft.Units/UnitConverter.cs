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

        #endregion

        public delegate decimal ConvertCallback(decimal value);

        public static void RegisterConversion(UnitConversion conversion) {
            RegisterConversion(conversion, true);
        }

        static void Register(Unit source, Unit target, ConvertCallback convert, bool throwOnDuplicates) {
            Hashtable table;
            if (!unitConversions.TryGetValue(source, out table)) {
                table = new Hashtable();
                table[target] = convert;
                unitConversions[source] = table;
            } else if (!table.ContainsKey(target)) {
                table[target] = convert;
            } else if (throwOnDuplicates) {
                throw new ArgumentException(RM.Format(RM.DuplicateConversionException, source, target));
            }
        }

       internal static void RegisterConversion(UnitConversion conversion, bool throwOnDuplicate) {
            lock (syncRoot) {
                if (unitConversions == null) {
                    unitConversions = new Dictionary<Unit, Hashtable>();
                }

                Register(conversion.Source, conversion.Target, new ConvertCallback(conversion.Convert), throwOnDuplicate);
                Register(conversion.Target, conversion.Source, new ConvertCallback(conversion.ConvertBack), throwOnDuplicate);
            }
        }

        public static void RegisterUnit(Unit unit) {
            if (unitConversions == null) {
                lock (syncRoot) {
                    if (unitConversions == null) {
                        unitConversions = new Dictionary<Unit, Hashtable>();
                    }
                }
            }
            if (!unitConversions.ContainsKey(unit)) {
                var scaledUnit = unit as ScaledUnit;
                if (scaledUnit != null) {
                    RegisterConversion(new ExtrapolatedConversion(scaledUnit), false);
                }
                foreach (object attribute in unit.GetType().GetCustomAttributes(true)) {
                    var conversion = attribute as UnitConversionAttribute;
                    if (conversion != null) {
                        RegisterConversion(conversion.UnitConversion);
                    }
                }
            }
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
