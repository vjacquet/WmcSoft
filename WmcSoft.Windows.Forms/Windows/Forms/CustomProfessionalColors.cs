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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Configuration;
using System.Xml.Serialization;
using System.Diagnostics;

namespace WmcSoft.Windows.Forms
{

    [Designer(typeof(Design.CustomProfessionalColorsDesigner))]
    [ProvideProperty("OverridePolicy", typeof(IComponent))]
    [PropertyTab(typeof(Design.ProfessionalColorsTab), PropertyTabScope.Component)]
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class CustomProfessionalColors : Component,
        ISupportInitialize,
        INotifyPropertyChanged,
        IPersistComponentSettings,
        IExtenderProvider
    {

        #region Private fields

        Dictionary<IComponent, RendererOverridePolicy> policies = new Dictionary<IComponent, RendererOverridePolicy>();
        internal SerializableColorTable colors;

        #endregion

        public CustomProfessionalColors(IContainer container) {
            container.Add(this);
            colors = new SerializableColorTable();
        }

        Color GetColor(string colorName, Color defaultColor) {
            Color color;
            if (colors.TryGetValue(colorName, out color)) {
                return color;
            }
            return defaultColor;
        }

        void SetColor(string colorName, Color value, Color defaultColor) {
            Color color;
            if (value == defaultColor) {
                int count = colors.Count;
                colors.Remove(colorName);
                if (colors.Count != count) {
                    FirePropertyChanged(colorName);
                }
            } else if (!colors.TryGetValue(colorName, out color) || color != value) {
                colors[colorName] = value;
                FirePropertyChanged(colorName);
            }
        }

        bool ShouldSerializeColor(string colorName, Color defaultColor) {
            Color color;
            if (colors.TryGetValue(colorName, out color)) {
                return color != defaultColor;
            }
            return false;
        }

        void ResetColor(string colorName, Color defaultColor) {
            colors.Remove(colorName);
        }

        #region INotifyPropertyChanged Membres

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged {
            add {
                this.Events.AddHandler(EventPropertyChanged, value);
            }
            remove {
                this.Events.RemoveHandler(EventPropertyChanged, value);
            }
        }
        static object EventPropertyChanged = new Object();

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            PropertyChangedEventHandler handler = this.Events[EventPropertyChanged] as PropertyChangedEventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        protected void FirePropertyChanged(string propertyName) {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region ISupportInitialize Membres

        void ISupportInitialize.BeginInit() {

        }

        void ISupportInitialize.EndInit() {
        }

        #endregion

        #region Properties

        [DefaultValue(false)]
        [Category("Appearance")]
        public bool IsToolStripManagerRenderer {
            get {
                return isToolStripManagerRenderer;
            }
            set {
                if (isToolStripManagerRenderer != value) {
                    if (value) {
                        ToolStripManager.Renderer = CreateRenderer();
                    } else {
                        ToolStripManager.Renderer = null;
                    }
                    isToolStripManagerRenderer = value;
                }
            }
        }
        bool isToolStripManagerRenderer = false;

        public ToolStripRenderer CreateRenderer() {
            CustomProfessionalColorTable colorTable = new CustomProfessionalColorTable(this);
            ToolStripRenderer renderer = new ToolStripProfessionalRenderer(colorTable);
            renderer.RenderItemText += new ToolStripItemTextRenderEventHandler(renderer_RenderItemText);
            return renderer;
        }

        void renderer_RenderItemText(object sender, ToolStripItemTextRenderEventArgs e) {
            ToolStripItem item = e.Item;
            ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
            ToolStripStatusLabel toolStripStatusLabel = item as ToolStripStatusLabel;

            if (toolStripMenuItem != null) {
                if (!toolStripMenuItem.Enabled) {
                } else
                    if (toolStripMenuItem.Pressed) {
                        e.TextColor = this.MenuItemPressedText;
                    } else if (toolStripMenuItem.Selected) {
                        e.TextColor = this.MenuItemSelectedText;
                    } else if (toolStripMenuItem.IsOnDropDown) {
                        e.TextColor = this.ToolStripDropDownText;
                    } else {
                        e.TextColor = this.MenuItemText;
                    }
            } else if (toolStripStatusLabel != null) {
                e.TextColor = this.StatusStripText;
            }

            ToolStripRenderer renderer = ToolStripManager.Renderer;
            renderer.RenderItemText -= new ToolStripItemTextRenderEventHandler(renderer_RenderItemText);
            renderer.DrawItemText(e);
            renderer.RenderItemText += new ToolStripItemTextRenderEventHandler(renderer_RenderItemText);

            if (item.Enabled && item.Selected) {
                Graphics g = e.Graphics;
                Color color = this.MenuBorder;
                Rectangle rect = new Rectangle(Point.Empty, item.Size);
                if (item.IsOnDropDown) {
                    rect.X += 2;
                    rect.Width -= 3;
                    color = this.MenuItemBorder;
                } else if (toolStripMenuItem != null && !toolStripMenuItem.Pressed) {
                    color = this.MenuItemBorder;
                }
                using (Pen pen = new Pen(color)) {
                    g.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                }
            }
        }

        #endregion

        #region IPersistComponentSettings Membres

        public void LoadComponentSettings() {
            if (!this.DesignMode) {
                this.Settings.Reload();
                SerializableColorTable colorTable = this.Settings["ColorTable"] as SerializableColorTable;
                if (colorTable != null)
                    colors = colorTable;
                else
                    this.Settings["ColorTable"] = colors;
                //foreach (SettingsProperty property in this.Settings.Properties) {
                //    object color = this.Settings[property.Name];
                //    if (color != null) {
                //        this.colors[property.Name] = (Color)color;
                //    }
                //}
            }
        }

        public void ResetComponentSettings() {
            if (!this.DesignMode) {
                this.Settings.Reset();
                this.LoadComponentSettings();
            }
        }

        public void SaveComponentSettings() {
            if (!this.DesignMode && saveSettings) {
                //foreach (string colorName in this.colors.Keys) {
                //    SettingsProperty property = this.Settings.Properties[colorName];
                //    if (property == null) {
                //        property = new SettingsProperty(this.Settings.Properties["ToolStripBorder"]);
                //        property.Name = colorName;
                //        this.Settings.Properties.Add(property);
                //    }
                //    this.Settings[colorName] = colors[colorName];
                //}
                this.Settings.Save();
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool SaveSettings {
            get {
                return saveSettings;
            }
            set {
                saveSettings = value;
            }
        }
        private bool saveSettings = false;

        [Category("Behavior")]
        public string SettingsKey {
            get {
                if (this.DesignMode && settingsKey == null) {
                    return this.Site.Name;
                }
                return settingsKey;
            }
            set {
                settingsKey = value;
            }
        }
        private string settingsKey = null;

        public void ResetSettingsKey() {
            settingsKey = null;
        }

        public CustomProfessionalColorsSettings Settings {
            get {
                if (settings == null) {
                    settings = new CustomProfessionalColorsSettings(this, settingsKey);
                }
                return settings;
            }
        }
        CustomProfessionalColorsSettings settings;

        #endregion

        #region IExtenderProvider Membres

        [DebuggerStepThrough]
        bool IExtenderProvider.CanExtend(object extendee) {
            if (extendee is IComponent && extendee != this) {
                return extendee is ToolStrip;
            }
            return false;
        }

        [DefaultValue(RendererOverridePolicy.None)]
        public RendererOverridePolicy GetRegisterAs(IComponent component) {
            RendererOverridePolicy policy;
            if (policies.TryGetValue(component, out policy)) {
                return policy;
            }
            return RendererOverridePolicy.None;
        }

        public void SetRegisterAs(IComponent component, RendererOverridePolicy policy) {
            RendererOverridePolicy previousPolicy = RendererOverridePolicy.None;
            if (!policies.TryGetValue(component, out previousPolicy)) {
                if (policy != RendererOverridePolicy.None) {
                    policies[component] = policy;
                    WireComponent(component, policy);
                }
            } else if (policy != previousPolicy) {
                if (policy == RendererOverridePolicy.None) {
                    UnwireComponent(component);
                    policies.Remove(component);
                } else {
                    policies[component] = policy;
                    WireComponent(component, policy);
                }
            }
        }

        void WireComponent(IComponent component, RendererOverridePolicy policy) {
            ToolStrip toolstrip = component as ToolStrip;
            if (toolstrip != null) {
                if ((policy == RendererOverridePolicy.OverrideAlways)
                    || (policy == RendererOverridePolicy.OverrideProfessional && toolstrip.RenderMode == ToolStripRenderMode.ManagerRenderMode && ToolStripManager.RenderMode == ToolStripManagerRenderMode.Professional)
                    || (policy == RendererOverridePolicy.OverrideProfessional && toolstrip.RenderMode == ToolStripRenderMode.Professional)) {
                    toolstrip.Renderer = new ToolStripProfessionalRenderer(new CustomProfessionalColorTable(this));
                }
                return;
            }
        }

        void UnwireComponent(IComponent component) {
            ToolStrip toolstrip = component as ToolStrip;
            if (toolstrip != null) {
                toolstrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                return;
            }
        }

        #endregion
    }

    public partial class CustomProfessionalColorTable
    {
        CustomProfessionalColors professionalColors;

        internal CustomProfessionalColorTable(CustomProfessionalColors professionalColors) {
            this.professionalColors = professionalColors;
        }
    }

    public class CustomProfessionalColorsSettings : ApplicationSettingsBase
    {

        public CustomProfessionalColorsSettings(IComponent owner, string settingsKey)
            : base(owner, settingsKey) {
            CustomProfessionalColors customProfessionalColors = owner as CustomProfessionalColors;
            this.ColorTable = customProfessionalColors.colors;
            //if (customProfessionalColors != null) {
            //    foreach (string colorName in customProfessionalColors.colors.Keys) {
            //        SettingsProperty property = this.Properties[colorName];
            //        if (property == null) {
            //            property = new SettingsProperty(this.Properties["ToolStripBorder"]);
            //            property.Name = colorName;
            //            this.Properties.Add(property);
            //        }
            //    }
            //}
        }

        //[UserScopedSetting]
        //public Color ToolStripBorder {
        //    get { return (Color)this["ToolStripBorder"]; }
        //    set { this["ToolStripBorder"] = value; }
        //}

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Xml)]
        public SerializableColorTable ColorTable {
            get { return (SerializableColorTable)this["ColorTable"]; }
            set { this["ColorTable"] = value; }
        }
    }

    public class SerializableColorTable : Dictionary<string, Color>, IXmlSerializable
    {
        #region IXmlSerializable Membres

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema() {
            return (null);
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            ColorConverter converter = new ColorConverter();
            this.Clear();
            while (reader.Read()) {
                if (reader.NodeType == System.Xml.XmlNodeType.Element) {
                    try {
                        string localName = reader.LocalName;
                        string value = reader.ReadString();
                        this[localName] = (Color)converter.ConvertFromString(value);
                    }
                    catch (ArgumentException) {
                    }
                }
            }
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            ColorConverter converter = new ColorConverter();
            Enumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext()) {
                writer.WriteElementString(enumerator.Current.Key, converter.ConvertToString(enumerator.Current.Value));
            }
        }

        #endregion
    }

    public enum RendererOverridePolicy
    {
        None,
        OverrideProfessional,
        OverrideAlways
    }
}