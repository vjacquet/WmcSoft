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
    }
}
