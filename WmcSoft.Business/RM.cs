using System;
using System.Resources;

namespace WmcSoft
{
    internal class RM
    {
        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        internal sealed class CategoryAttribute : System.ComponentModel.CategoryAttribute
        {
            // Methods
            public CategoryAttribute(string resourceName)
                : base(resourceName) {
            }

            protected override string GetLocalizedString(string value) {
                return GetString(value);
            }
        }

        internal class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
        {
            public DescriptionAttribute(string resourceName)
                : base(GetString(resourceName)) {
            }
        }

        internal class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
        {
            public DisplayNameAttribute(string resourceName)
                : base(GetString(resourceName)) {
            }
        }

        private static readonly ResourceManager ResourceManager;
        static RM() {
            ResourceManager = new ResourceManager(typeof(RM));
        }

        public static string GetString(string name) {
            return ResourceManager.GetString(name);
        }
    }
}
