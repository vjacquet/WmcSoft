using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WmcSoft.ComponentModel.Design
{
    public partial class DesignerHostListForm : Form
    {
        public DesignerHostListForm() {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IDesignerHost host) {
            var collection = componentListBox.Items;

            collection.Clear();

            var container = host.Container;
            foreach (IComponent component in container.Components) {
                collection.Add(component.GetType().Name + ": " + component.Site.Name);
            }

            return ShowDialog();
        }
    }
}
