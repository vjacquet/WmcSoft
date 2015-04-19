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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ToolStrip))]
    public class ToolStrip : System.Windows.Forms.ToolStrip
    {
        #region "ClickThrough" properties

        [Category("Behavior")]
        public bool ClickThrough { get; set; }

        [Category("Behavior")]
        public bool SuppressHighlighting { get; set; }

        #endregion

        #region Overridables

        protected override void WndProc(ref System.Windows.Forms.Message m) {
            // If we don't want highlighting, throw away mousemove commands
            // when the parent form or one of its children does not have the focus
            if (m.Msg == NativeMethods.WM_MOUSEMOVE
                && SuppressHighlighting
                && !TopLevelControl.ContainsFocus)
                return;

            base.WndProc(ref m);

            // If we want ClickThrough, replace "Activate and Eat" with
            // "Activate" on WM_MOUSEACTIVATE messages
            if (m.Msg == NativeMethods.WM_MOUSEACTIVATE
                && ClickThrough && m.Result == (IntPtr)NativeMethods.MA_ACTIVATEANDEAT)
                m.Result = (IntPtr)NativeMethods.MA_ACTIVATE;

        }

        #endregion
    }
}
