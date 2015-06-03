using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.ComponentModel
{
    public static class TypeDescriptorExtensions
    {
        public static void SetValues(this PropertyDescriptorCollection properties, object component, IDictionary<string, object> values) {
            foreach (var entry in values) {
                var property = properties[entry.Key];
                if (property != null) {
                    var value = entry.Value;
                    if (value is Type && property.PropertyType.IsAssignableFrom((Type)value)) {
                        var instance = Activator.CreateInstance((Type)value);
                        property.SetValue(component, instance);
                    } else {
                        property.SetValue(component, value);
                    }
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
    }
}
