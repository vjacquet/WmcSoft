using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace WmcSoft.Windows.Forms.Design
{
    /// <summary>
    /// List the Professional colors.
    /// </summary>
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class ProfessionalColorsTab : PropertyTab
    {
        internal class ToolStripCategoryAttribute : System.ComponentModel.CategoryAttribute
        {
            public ToolStripCategoryAttribute(string properyName)
                : base(properyName) {
            }

            protected override string GetLocalizedString(string value) {
                if (value.StartsWith("ToolStrip") || value.StartsWith("Button") || value.StartsWith("OverflowButton")) {
                    return "ToolStrip";
                } else if (value.StartsWith("StatusStrip")) {
                    return "StatusStrip";
                } else if (value.StartsWith("Menu") || value.StartsWith("Check") || value.StartsWith("ImageMargin")) {
                    return "MenuStrip";
                } else {
                    return CategoryAttribute.Appearance.Category;
                }
            }
        }

        #region Lifecycle

        public ProfessionalColorsTab() {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the properties of the specified component extended with 
        /// a ToolStripCategoryAttribute reflecting the name of the type of the property.
        /// </summary>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <param name="attributes">An array of type <see cref="System.Attribute"/> that indicates the attributes of the properties to retrieve.</param>
        /// <returns>A <see cref="System.ComponentModel.PropertyDescriptorCollection"/> that contains the properties.</returns>
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes) {
            PropertyDescriptor[] properties = TypeDescriptor.GetProperties(component)
                .Cast<PropertyDescriptor>()
                .Where(pd => typeof(System.Drawing.Color).IsAssignableFrom(pd.PropertyType))
                .ToArray();
            return new PropertyDescriptorCollection(properties);
        }

        /// <summary>
        /// Gets the properties of the specified component that match the specified attributes.
        /// </summary>
        /// <param name="component">The component to retrieve properties from.</param>
        /// <returns>A <see cref="System.ComponentModel.PropertyDescriptorCollection"/> that contains the properties.</returns>
        public override PropertyDescriptorCollection GetProperties(object component) {
            return this.GetProperties(component, new Attribute[0]);
        }

        /// <summary>
        /// Provides the name for the property tab.
        /// </summary>
        public override string TabName {
            get {
                return "Color scheme";
            }
        }

        /// <summary>
        /// Provides an image for the property tab.
        /// </summary>
        public override Bitmap Bitmap {
            get {
                return (Bitmap)ToolboxBitmapAttribute.GetImageFromResource(typeof(ProfessionalColorsTab), "ProfessionalColorsTab.bmp", false);
            }
        }

        public override bool CanExtend(object extendee) {
            return base.CanExtend(extendee) && extendee is CustomProfessionalColors;
        }

        #endregion

    }
}