using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.ComponentModel
{
    public partial class ComponentWithServiceContainer : Component
    {
        public ComponentWithServiceContainer() {
            InitializeComponent();
        }

        public ComponentWithServiceContainer(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }

        private void serviceContainerComponent1_ServiceResolve(object sender, ServiceResolveEventArgs args) {

        }
    }
}
