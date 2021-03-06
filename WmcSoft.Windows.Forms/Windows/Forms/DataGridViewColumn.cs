﻿#region Licence

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

namespace WmcSoft.Windows.Forms
{
    public class DataGridViewColumn<TCell> : System.Windows.Forms.DataGridViewColumn
        where TCell : System.Windows.Forms.DataGridViewCell, new()
    {
        public DataGridViewColumn()
            : base(new TCell()) {
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Windows.Forms.DataGridViewCell CellTemplate {
            get {
                return base.CellTemplate;
            }
            set {
                if (value != null && value is TCell)
                    throw new InvalidCastException();
                base.CellTemplate = value;
            }
        }

        protected TCell TypedCellTemplate {
            get {
                var cell = CellTemplate;
                if (cell == null)
                    throw new InvalidOperationException();
                return (TCell)cell;
            }
        }

        protected void ForEachCell(Action<TCell> action) {
            DataGridView.ForEach(Index, action);
        }
        protected void ForEachCell(Action<TCell, int> action) {
            DataGridView.ForEach(Index, action);
        }
    }
}
