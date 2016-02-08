#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

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
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using WmcSoft.Windows.Forms.Layout;

namespace WmcSoft.Windows.Forms
{
    [DefaultProperty("Caption")]
    public class HorizontalFormLayoutSettings : LayoutSettings
    {
        class ControlSettings
        {
            public string Caption;
        }

        readonly Dictionary<Control, ControlSettings> _captions;

        public HorizontalFormLayoutSettings() {
            _captions = new Dictionary<Control, ControlSettings>();
        }

        public override LayoutEngine LayoutEngine
        {
            get { return HorizontalFormLayoutEngine.Instance; }
        }

        public void SetCaption(Control child, string value) {
            if (value == null)
                _captions.Remove(child);
            else
                _captions[child] = new ControlSettings { Caption = value };
        }

        public string GetCaption(Control child) {
            ControlSettings settings;
            if (_captions.TryGetValue(child, out settings))
                return settings.Caption;
            return child.Name;
        }

        public Size MeasureLabels(Font font) {
            var query = from c in _captions.Values
                        select TextRenderer.MeasureText(c.Caption, font);
            return query.Aggregate(Size.Empty, Combine);
        }

        public static Size Combine(Size x, Size y) {
            var w = Math.Max(x.Width, y.Width);
            var h = Math.Max(x.Height, y.Height);
            return new Size(w, h);
        }
    }
}
