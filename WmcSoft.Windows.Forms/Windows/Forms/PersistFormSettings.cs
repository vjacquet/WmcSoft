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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// Component to drop on a form to persist its settings such as its window state, its size or location.
    /// </summary>
    [ToolboxBitmap(typeof(PersistFormSettings), "PersistFormSettings.png")]
    [DesignerCategory("Forms")]
    public class PersistFormSettings : Component, IPersistComponentSettings
    {
        #region Lifecycle

        public PersistFormSettings() {
        }

        protected override void Dispose(bool disposing) {
            SaveComponentSettings();
            base.Dispose(disposing);
        }

        #endregion

        #region Properties

        [Category("Behavior")]
        public Form Form
        {
            get {
                if (form == null && this.DesignMode == true) {
                    var designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
                    if (designerHost != null) {
                        var rootComponent = designerHost.RootComponent;
                        this.form = rootComponent as Form;
                    }
                }
                return form;
            }
            set {
                if (form != value) {
                    Unbind(form);
                    form = value;
                    Bind(form);
                }
            }
        }
        Form form;

        private void Bind(Form form) {
            if (form != null) {
                form.Load += form_Load;
                form.FormClosing += form_FormClosing;
            }
        }

        private void Unbind(Form form) {
            if (form != null) {
                form.Load -= form_Load;
                form.FormClosing -= form_FormClosing;
            }
        }

        void form_Load(object sender, EventArgs e) {
            try {
                var screen = Screen.FromControl(form).Bounds;
                var rect = new Rectangle(Settings.Location, Settings.Size);
                var x = (rect.Right < screen.Left)
                    ? screen.Left
                    : (screen.Right < rect.Left)
                        ? screen.Right - Settings.Size.Width
                        : Settings.Location.X;
                var y = (rect.Bottom < screen.Top) || (screen.Bottom < rect.Top)
                    ? screen.Top
                    : Settings.Location.Y;

                form.DesktopBounds = new Rectangle(new Point(x, y), Settings.Size);
                form.WindowState = Settings.WindowState;
            }
            catch (NullReferenceException) {
                // the settings are missing
            }
        }

        void form_FormClosing(object sender, FormClosingEventArgs e) {
            if (FormWindowState.Normal == form.WindowState) {
                Settings.WindowState = FormWindowState.Normal;
                Settings.Size = form.Size;
                Settings.Location = form.Location;
            } else {
                Settings.WindowState = form.WindowState;
                Settings.Size = form.RestoreBounds.Size;
                Settings.Location = form.RestoreBounds.Location;
            }
        }

        #endregion

        #region IPersistComponentSettings Membres

        [Browsable(false)]
        public FormSettings Settings
        {
            get {
                if (_settings == null) {
                    _settings = new FormSettings(this, _settingsKey);
                }
                return _settings;
            }
        }
        FormSettings _settings;

        public void LoadComponentSettings() {
            if (!DesignMode) {
                Settings.Reload();
            }
        }

        public void ResetComponentSettings() {
            if (!DesignMode) {
                Settings.Reset();
                LoadComponentSettings();
            }
        }

        public void SaveComponentSettings() {
            if (!DesignMode && SaveSettings) {
                Settings.Save();
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool SaveSettings { get; set; }

        [Category("Behavior")]
        public string SettingsKey
        {
            get {
                if (DesignMode && _settingsKey == null) {
                    return Site.Name;
                }
                return _settingsKey;
            }
            set {
                _settingsKey = value;
            }
        }
        private string _settingsKey = null;

        private void ResetSettingsKey() {
            _settingsKey = null;
        }

        #endregion
    }
}