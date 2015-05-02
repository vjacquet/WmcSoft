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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using WmcSoft.ComponentModel.Design;

namespace WmcSoft.ComponentModel
{
    [Designer(typeof(TracingComponentDesigner))]
    [ToolboxBitmap(typeof(TracingComponent), "TracingComponent.png")]
    public partial class TracingComponent : Component, ISupportInitialize
    {
        TraceSource _traceSource;

        #region Lifecycle

        public TracingComponent() {
            TraceSource = "";
            InitializeComponent();
        }

        public TracingComponent(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region ISupportInitialize Membres

        void ISupportInitialize.BeginInit() {
            Trace.TraceInformation("ISupportInitialize.BeginInit");
        }

        void ISupportInitialize.EndInit() {
            if (LaunchDebugger && !Debugger.IsAttached)
                Debugger.Break();
            Trace.TraceInformation("ISupportInitialize.EndInit");
        }

        #endregion

        #region Properties

        [DefaultValue("")]
        public string TraceSource {
            get {
                if (_traceSource == null)
                    return "";
                return _traceSource.Name;
            }
            set {
                if (value == null) {
                    _traceSource = null;
                } else {
                    _traceSource = new TraceSource(value);
                }
            }
        }

        [DefaultValue(false)]
        public bool LaunchDebugger { get; set; }

        #endregion

        #region Methods

        public void TraceMessage(string category, string message) {
            if (_traceSource != null) {
                _traceSource.TraceInformation(category + ": " + message);
            } else {
                Debugger.Log(0, category, message);
            }
        }

        #endregion

    }
}
