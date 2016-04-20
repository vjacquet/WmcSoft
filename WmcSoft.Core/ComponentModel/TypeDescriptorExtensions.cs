using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WmcSoft.ComponentModel
{
    /// <summary>
    /// Defines extension methods to the <see cref="TypeDescriptor"/>, <see cref="PropertyDescriptor"/> or <see cref="PropertyDescriptorCollection"/> classes. 
    /// This is a static class.
    /// </summary>
    public static class TypeDescriptorExtensions
    {
        static object ResolveValue(Type target, object value) {
            var type = value as Type;
            if (type != null && target.IsAssignableFrom(type)) {
                return Activator.CreateInstance(type);
            }
            return value;
        }

        /// <summary>
        /// Sets the values of the component.
        /// </summary>
        /// <param name="properties">The property descriptor collection.</param>
        /// <param name="component">The component whose properties to be set.</param>
        /// <param name="values">The values to set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public static void SetValues(this PropertyDescriptorCollection properties, object component, IDictionary<string, object> values) {
            if (values == null) throw new ArgumentNullException("values");

            foreach (var entry in values) {
                var property = properties[entry.Key];
                if (property != null) {
                    var value = ResolveValue(property.PropertyType, entry.Value);
                    property.SetValue(component, value);
                }
            }
        }

        /// <summary>
        /// Gets the values of the component.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="component"></param>
        /// <param name="properties">The property descriptor collection.</param>
        /// <param name="component">The component whose properties to get.</param>
        /// <param name="values">The values to get.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public static void GetValues(this PropertyDescriptorCollection properties, object component, IDictionary<string, object> values) {
            if (values == null) throw new ArgumentNullException("values");

            foreach (var key in values.Keys.ToArray()) {
                var property = properties[key];
                if (property != null) {
                    values[key] = property.GetValue(component);
                }
            }
        }

        /// <summary>
        /// Gets the current value of the property on a component.
        /// </summary>
        /// <param name="property">The property descriptor.</param>
        /// <param name="component">The component.</param>
        /// <param name="value">When this method returns, contains the value of the property, if the property descriptor or the component are not null; otherwhise, null.</param>
        /// <returns>Returns false when the property or the component are null.</returns>
        public static bool TryGetValue(this PropertyDescriptor property, object component, out object value) {
            if (property == null) {
                value = null;
                return false;
            }
            value = property.GetValue(component);
            return true;
        }

        /// <summary>
        /// Gets the attribute of the specified type for this member.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="descriptor">The descriptor of the member.</param>
        /// <returns>The attribute.</returns>
        public static TAttribute GetMetadataAttribute<TAttribute>(this MemberDescriptor descriptor)
            where TAttribute : Attribute {
            return (TAttribute)descriptor.Attributes[typeof(TAttribute)];
        }
    }
}