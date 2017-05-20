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
using System.Reflection;
using WmcSoft.Units.Properties;

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

        static UnitConverter()
        {
            unitConversions = new Dictionary<Unit, Hashtable>();
        }

        #endregion

        public delegate decimal ConvertCallback(decimal value);

        public static bool RegisterConversion(UnitConversion conversion)
        {
            return RegisterConversion(conversion, true);
        }

        static bool Register(Unit source, Unit target, ConvertCallback convert, bool throwOnDuplicates)
        {
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
                throw new ArgumentException(string.Format(Resources.DuplicateConversionException, source, target));
            }
            return false;
        }

        internal static bool RegisterConversion(UnitConversion conversion, bool throwOnDuplicate)
        {
            lock (syncRoot) {
                if (unitConversions == null) {
                    unitConversions = new Dictionary<Unit, Hashtable>();
                }

                bool registered = Register(conversion.Source, conversion.Target, new ConvertCallback(conversion.Convert), throwOnDuplicate);
                registered |= Register(conversion.Target, conversion.Source, new ConvertCallback(conversion.ConvertBack), throwOnDuplicate);
                return registered;
            }
        }

        public static bool RegisterUnit(ScaledUnit scaledUnit)
        {
            if (!unitConversions.ContainsKey(scaledUnit)) {
                var factor = 1m;
                var source = scaledUnit;
                while (scaledUnit != null) {
                    factor *= scaledUnit.ScaleFactor;
                    RegisterConversion(new LinearConversion(source, scaledUnit.Reference, factor), false);
                    scaledUnit = scaledUnit.Reference as ScaledUnit;
                }

                return true;
            }
            return false;
        }

        public static bool RegisterUnit(Unit unit)
        {
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

        public static Quantity Convert(Quantity quantity, Unit target)
        {
            if (quantity.Metric == target)
                return quantity;

            var source = quantity.Metric as Unit;
            if (source == null)
                throw new ArgumentException(Resources.NotAUnitException, "quantity");

            RegisterUnit(source);
            RegisterUnit(target);

            Hashtable table;
            if (unitConversions.TryGetValue(source, out table)) {
                var callback = table[target] as ConvertCallback;
                if (callback != null) {
                    return new Quantity(callback(quantity.Amount), target);
                }
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Converts a quantity to a quantity of the target Unit.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        /// <param name="target">The target unit.</param>
        /// <returns>The converted quantity.</returns>
        public static Quantity ConvertTo(this Quantity quantity, Unit target)
        {
            return Convert(quantity, target);
        }

        /// <summary>
        /// Converts a quantity to a quantity of the target Unit.
        /// </summary>
        /// <typeparam name="U">The target unit type.</typeparam>
        /// <param name="quantity">The quantity.</param>
        /// <returns>The converted quantity.</returns>
        public static Quantity<U> ConvertTo<U>(this Quantity quantity) where U : Unit, new()
        {
            var result = Convert(quantity, new U());
            return new Quantity<U>(result.Amount);
        }

        /// <summary>
        /// Creates a reciprocal unit conversion.
        /// </summary>
        /// <param name="conversion">The unit conversion.</param>
        /// <returns>The reciprocal unit conversion.</returns>
        /// <remarks>This function guarantee that <code>Reciprocal(Reciprocal(conversion)) == conversion</code>.</remarks>
        public static UnitConversion Reciprocal(this UnitConversion conversion)
        {
            if (conversion == null)
                throw new ArgumentNullException("conversion");

            var reciprocal = conversion as ReciprocalUnitConversion;
            if (reciprocal == null)
                return new ReciprocalUnitConversion(conversion);
            return reciprocal.BaseUnitConversion;
        }

        static UnitConversion DoCompose(LinearConversion x, LinearConversion y)
        {
            return new LinearConversion(x.Source, y.Target, x.ConversionFactor * y.ConversionFactor);
        }

        static UnitConversion DoCompose(UnitConversion x, UnitConversion y)
        {
            return new CompositeConversion(x, y);
        }

        static UnitConversion DoCompose(CompositeConversion x, UnitConversion y)
        {
            return new CompositeConversion(x, y);
        }

        static UnitConversion DoCompose(UnitConversion x, CompositeConversion y)
        {
            return new CompositeConversion(x, y);
        }

        static UnitConversion DoCompose(CompositeConversion x, CompositeConversion y)
        {
            return new CompositeConversion(x, y);
        }

        public static UnitConversion Compose(UnitConversion x, UnitConversion y)
        {
            if (x.Target != y.Source) throw new ArgumentException(string.Format(Resources.InvalidUnitConversionPath, x.Target, y.Source));

            if (x.Source == y.Target)
                return new IdentityConversion(x.Source);

            // performs double dispatch
            var factory = typeof(UnitConverter).GetMethod("DoCompose", BindingFlags.Static | BindingFlags.NonPublic, Type.DefaultBinder, new[] { x.GetType(), y.GetType() }, new ParameterModifier[0]);
            if (factory != null)
                return (UnitConversion)factory.Invoke(null, new[] { x, y });

            throw new InvalidOperationException();
        }
    }
}
