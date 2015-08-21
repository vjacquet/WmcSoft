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
                return RM.GetString(value);
            }
        }

        internal class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
        {
            public DescriptionAttribute(string resourceName)
                : base(RM.GetString(resourceName)) {
            }
        }

        internal class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
        {
            public DisplayNameAttribute(string resourceName)
                : base(RM.GetString(resourceName)) {
            }
        }

        private static readonly ResourceManager ResourceManager;
        static RM() {
            ResourceManager = new System.Resources.ResourceManager(typeof(RM));
        }

        public static string GetString(string name) {
            return ResourceManager.GetString(name);
        }
    }
}
