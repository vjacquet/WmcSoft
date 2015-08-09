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
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(ToolStripBackgroundWorkerProgressBar), "ToolStripBackgroundWorkerProgressBar.bmp")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ToolStripBackgroundWorkerProgressBar : ToolStripProgressBar
    {
        #region Lifecycle

        public ToolStripBackgroundWorkerProgressBar() {
            Available = false;
        }

        public ToolStripBackgroundWorkerProgressBar(string name):base(name) {
            Available = false;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
            }
            Unwire();
            base.Dispose(disposing);
        }

        public override ISite Site {
            get {
                return base.Site;
            }
            set {
                base.Site = value;
                Available = (value != null && value.DesignMode);
            }
        }
        #endregion

        #region Properties

        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public BackgroundWorker BackgroundWorker {
            get {
                return backgroundWorker;
            }
            set {
                if (backgroundWorker != value) {
                    Unwire();
                    backgroundWorker = value;
                    Wire();
                    OnBackgroundWorkerChanged(EventArgs.Empty);
                }
            }
        }
        BackgroundWorker backgroundWorker = null;

        public event EventHandler BackgroundWorkerChanged {
            add { this.Events.AddHandler(BackgroundWorkerChangedEvent, value); }
            remove { this.Events.RemoveHandler(BackgroundWorkerChangedEvent, value); }
        }
        static object BackgroundWorkerChangedEvent = new Object();

        protected virtual void OnBackgroundWorkerChanged(EventArgs e) {
            var handler = this.Events[BackgroundWorkerChangedEvent] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Methods

        private void Wire() {
            if (backgroundWorker != null) {
                if (backgroundWorker.WorkerReportsProgress) {
                    backgroundWorker.ProgressChanged += worker_ProgressChanged;
                } else {
                }
                backgroundWorker.RunWorkerCompleted += worker_RunWorkerCompleted;
            }
        }

        private void Unwire() {
            if (backgroundWorker != null) {
                if (backgroundWorker.WorkerReportsProgress) {
                    backgroundWorker.ProgressChanged -= worker_ProgressChanged;
                } else {
                }
                backgroundWorker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            Available = true;
            Value = e.ProgressPercentage;
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Available = false;
            Value = 0;
        }

        #endregion
    }
}
