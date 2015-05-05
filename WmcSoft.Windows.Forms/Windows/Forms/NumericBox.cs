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
using System.Globalization;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxBitmap(typeof(NumericBox), "NumericBox.png")]
    public class NumericBox : TextBox
    {
        #region Lifecycle

        public NumericBox() {
            AllowDecimal = true;
            AllowNegative = true;
        }

        #endregion

        #region Properties

        [DefaultValue(true)]
        public bool AllowDecimal { get; set; }

        [DefaultValue(true)]
        public bool AllowNegative { get; set; }

        #endregion

        #region Overrides

        private bool IsValidChar(char charCode) {
            if (charCode == '\b')
                return true;

            var nfi = Thread.CurrentThread.CurrentUICulture.NumberFormat;
            string code = charCode.ToString();
            for (int i = 0; i < nfi.NativeDigits.Length; i++) {
                if (nfi.NativeDigits[i] == code)
                    return true;
            }
            if ((AllowNegative && nfi.NegativeSign == code) || (AllowDecimal && nfi.NumberDecimalSeparator == code))
                return true;

            return false;
        }

        int lastKeyDown = 0;

        protected override void WndProc(ref Message m) {
            if (m.Msg == NativeMethods.WM_KEYDOWN) {
                lastKeyDown = (int)m.WParam;
            } else if (m.Msg == NativeMethods.WM_CHAR) {
                char charCode = (char)((int)m.WParam);
                if (charCode == '.' && lastKeyDown == 0x6E) {
                    var nfi = Thread.CurrentThread.CurrentUICulture.NumberFormat;
                    m.WParam = (IntPtr)nfi.NumberDecimalSeparator[0];
                } else if (IsInputChar(charCode)
                    && (Control.ModifierKeys == Keys.None)
                    && !IsValidChar(charCode)) {
                    SystemSounds.Beep.Play();
                    return;
                }
            }
            base.WndProc(ref m);
        }

        #endregion
    }
}
