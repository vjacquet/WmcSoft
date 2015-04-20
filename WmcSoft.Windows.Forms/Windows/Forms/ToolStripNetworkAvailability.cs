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

using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(ToolStripNetworkAvailability), "ToolStripNetworkAvailability.png")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripNetworkAvailability : ToolStripStatusLabel
    {
        #region Private fields

        private ImageList imageList;
        private IContainer components;

        bool _isNetworkAvailable;

        #endregion

        #region Lifecycle

        public ToolStripNetworkAvailability() {
            InitializeComponent();
            PostInitializeComponent();
        }

        private void PostInitializeComponent() {
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            _isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            UpdateNetworkAvailabilityCues();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
            }
            NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAvailabilityChanged;
            base.Dispose(disposing);
        }

        #endregion

        #region Event handling

        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e) {
            _isNetworkAvailable = e.IsAvailable;
            UpdateNetworkAvailabilityCues();
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string ToolTipText {
            get { return base.ToolTipText; }
            set { base.ToolTipText = value; }
        }

        #endregion

        #region Methods

        void UpdateNetworkAvailabilityCues() {
            if (_isNetworkAvailable) {
                ImageIndex = 2;
                Text = Properties.Resources.NetworkAvailableText;
                ToolTipText = Properties.Resources.NetworkAvailableToolTip;
            } else {
                ImageIndex = 0;
                Text = Properties.Resources.NetworkNotAvailableText;
                ToolTipText = Properties.Resources.NetworkNotAvailableToolTip;
            }
        }

        #endregion

        #region Overridables

        protected override ToolStripItemDisplayStyle DefaultDisplayStyle {
            get { return ToolStripItemDisplayStyle.Image; }
        }

        #endregion

        #region Code généré par le Concepteur de composants

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolStripNetworkAvailability));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "None");
            this.imageList.Images.SetKeyName(1, "Local");
            this.imageList.Images.SetKeyName(2, "LAN");
            this.imageList.Images.SetKeyName(3, "WAN");
            // 
            // ToolStripNetworkAvailability
            // 
            this.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;

        }

        #endregion
    }

}
