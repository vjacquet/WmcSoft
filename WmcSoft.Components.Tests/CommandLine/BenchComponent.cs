using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.CommandLine
{
    public partial class BenchComponent : Component
    {
        public BenchComponent() {
            InitializeComponent();
        }

        public BenchComponent(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }
    }
}
