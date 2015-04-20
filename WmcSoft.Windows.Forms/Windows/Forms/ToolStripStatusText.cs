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
using System.Windows.Forms;

// Inspired from Genghis.Windows.Forms.StatusBarExtender

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(ToolStripStatusText), "ToolStripStatusText.png")]
    [ProvideProperty("StatusText", typeof(Control)), System.ComponentModel.DesignerCategory("Code")]
    public class ToolStripStatusText : ToolStripStatusLabel
        , IExtenderProvider
    {
        #region Lifecycle

        public ToolStripStatusText() {
            statusTexts = new Dictionary<Control, string>();
        }

        #endregion

        #region IExtenderProvider Membres

        IDictionary<Control, string> statusTexts;

        public bool CanExtend(object extendee) {
            return true;
        }

        [DefaultValue("")]
        public string GetStatusText(Control extendee) {
            string text;
            if (statusTexts.TryGetValue(extendee, out text))
                return text;
            return "";
        }

        public void SetStatusText(Control extendee, string value) {
            if (value == null) {
                this.statusTexts.Remove(extendee);
            } else {
                this.statusTexts[extendee] = value;

                // Watch for the focus event
                if (!DesignMode) {
                    extendee.GotFocus += new EventHandler(extendee_GotFocus);
                }
            }
        }

        void extendee_GotFocus(object sender, EventArgs e) {
            var extendee = (Control)sender;
            var text = GetStatusText(extendee);
            if (!String.IsNullOrWhiteSpace(text) && this.Text != text) {
                TextReset.Attach(this, extendee);
                this.Text = text;
            }
        }

        class TextReset
        {
            public static void Attach(ToolStripStatusText toolStripStatusText, Control extendee) {
                new TextReset(toolStripStatusText, extendee);
            }

            readonly ToolStripStatusText _toolStripStatusText;
            readonly string _text;

            TextReset(ToolStripStatusText toolStripStatusText, Control extendee) {
                _toolStripStatusText = toolStripStatusText;
                _text = toolStripStatusText.Text;
                extendee.LostFocus += new EventHandler(extendee_LostFocus);
            }

            void extendee_LostFocus(object sender, EventArgs e) {
                var extendee = (Control)sender;
                extendee.LostFocus -= new EventHandler(extendee_LostFocus);
                _toolStripStatusText.Text = _text;
            }
        }

        #endregion
    }
}
