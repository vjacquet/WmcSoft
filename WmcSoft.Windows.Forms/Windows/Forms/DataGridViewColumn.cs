using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            DataGridView.ForEach<TCell>(Index, action);
        }
        protected void ForEachCell(Action<TCell, int> action) {
            DataGridView.ForEach<TCell>(Index, action);
        }
    }
}
