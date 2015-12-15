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
using System.Deployment.Application;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(ToolStripStatusVersion), "ToolStripStatusVersion.png")]
    [ProvideProperty("StatusText", typeof(Control))]
    [DesignerCategory("Code")]
    public class ToolStripStatusVersion : ToolStripStatusLabel
    {
        #region Lifecycle

        public ToolStripStatusVersion() {
            _fieldCount = 3;
            Paint += ToolStripStatusVersion_Paint;
        }

        private void ToolStripStatusVersion_Paint(object sender, PaintEventArgs e) {
            Paint -= ToolStripStatusVersion_Paint;
            if (_version == null) {
                Version = GetCurrentVersion();
            }
        }

        public Version GetCurrentVersion() {
            if (DesignMode)
                return new Version(1, 0);
            else if (ApplicationDeployment.IsNetworkDeployed)
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        #endregion

        [Bindable(BindableSupport.Yes)]
        [DefaultValue(null)]
        public Version Version {
            get {
                return _version;
            }
            set {
                if (_version != value) {
                    _version = value;
                    OnVersionChanged(EventArgs.Empty);
                }
            }
        }
        Version _version;

        public event EventHandler VersionChanged {
            add { Events.AddHandler(VersionChangedEvent, value); }
            remove { Events.RemoveHandler(VersionChangedEvent, value); }
        }
        private readonly object VersionChangedEvent = new object();

        protected virtual void OnVersionChanged(EventArgs e) {
            Text = (_version == null) ? "" : _version.ToString(_fieldCount);
            EventHandler handler = (EventHandler)Events[VersionChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        [DefaultValue(3)]
        public int FieldCount {
            get {
                return _fieldCount;
            }
            set {
                if (_fieldCount != value) {
                    _fieldCount = value;
                    OnVersionChanged(EventArgs.Empty);
                }
            }
        }
        int _fieldCount;
    }
}
