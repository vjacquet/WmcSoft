using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsGallery
{
    public partial class ControlsForm : Form
    {
        public ControlsForm() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            var index = deckLayoutPanel1.Controls.IndexOf(deckLayoutPanel1.ActiveControl);
            index = (index + 1) % deckLayoutPanel1.Controls.Count;
            deckLayoutPanel1.ActivateControl(deckLayoutPanel1.Controls[index]);
        }
    }
}
