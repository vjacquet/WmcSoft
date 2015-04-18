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

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    [ToolboxBitmap(typeof(ToolStripSpringLabel), "ToolStripSpringLabel.bmp")]
    public class ToolStripSpringLabel : ToolStripLabel
    {
        void Initialize() {
            TextAlign = ContentAlignment.MiddleLeft;
        }

        public ToolStripSpringLabel()
            : base() {
            Initialize();
        }

        public ToolStripSpringLabel(Image image)
            : base(image) {
            TextAlign = ContentAlignment.MiddleLeft;
            Initialize();
        }

        public ToolStripSpringLabel(string text)
            : base(text) {
            Initialize();
        }

        public ToolStripSpringLabel(string text, Image image)
            : base(text, image) {
            Initialize();
        }

        public override Size GetPreferredSize(Size constrainingSize) {
            Size size = base.GetPreferredSize(constrainingSize);
            if (Owner != null && Owner.DisplayRectangle.Width > 0) {
                int width = Owner.ClientSize.Width - 2;
                foreach (var item in Owner.Items.Except(this)) {
                    width -= item.Margin.Left
                        + item.Padding.Left
                        + item.Width
                        + item.Padding.Right
                        + item.Margin.Right;
                }
                if (width > size.Width) {
                    size.Width = width;
                }
            }
            return size;
        }
    }
}
