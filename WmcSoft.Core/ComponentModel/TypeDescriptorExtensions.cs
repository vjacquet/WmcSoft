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
        static object ResolveValue(PropertyDescriptor property, object value) {
            var type = value as Type;
            if (type != null && property.PropertyType.IsAssignableFrom(type)) {
                return Activator.CreateInstance(type);
            }
            return value;
        }

        /// <summary>
        /// Sets the values of the component.
        /// </summary>
        /// <param name="properties">The property descriptor collection.</param>
        /// <param name="component">The component whose properties must be set.</param>
        /// <param name="values">The values to set.</param>
        public static void SetValues(this PropertyDescriptorCollection properties, object component, IDictionary<string, object> values) {
            if (values == null || component == null)
                return;

            foreach (var entry in values) {
                var property = properties[entry.Key];
                if (property != null) {
                    var value = ResolveValue(property, entry.Value);
                    property.SetValue(component, value);
                }
            }
        }

        public static void GetValues(this PropertyDescriptorCollection properties, object component, IDictionary<string, object> values) {
            foreach (var key in values.Keys.ToList()) {
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
            if (property != null && component != null) {
                value = property.GetValue(component);
                return true;
            }
            value = null;
            return false;
        }

        public static TAttribute GetMetadataAttribute<TAttribute>(this PropertyDescriptor propertyDescriptor)
            where TAttribute : Attribute {
            return propertyDescriptor.Attributes[typeof(TAttribute)] as TAttribute;
        }
    }
}