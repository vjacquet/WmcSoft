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
using System.Drawing;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(System.Windows.Forms.SplitContainer), "SplitContainer.bmp")]
    public class SplitContainer : System.Windows.Forms.SplitContainer
    {
        #region Lifecycle

        public SplitContainer() {
            ResetBackColor();
        }

        #endregion

        #region Properties

        private bool ShouldSerializeBackColor() {
            return BackColor != Color.Transparent;
        }

        public override void ResetBackColor() {
            BackColor = Color.Transparent;
        }

        public override bool Focused {
            get {
                if (_supportFocus)
                    return base.Focused;
                return false;
            }
        }

        [DefaultValue(false)]
        public bool SupportFocus {
            get { return _supportFocus; }
            set { _supportFocus = value; }
        }
        bool _supportFocus = false;

        #endregion
    }
}
