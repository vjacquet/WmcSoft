using System.ComponentModel;

namespace WmcSoft.CommandLine
{
    public partial class BenchComponent : Component
    {
        public BenchComponent()
        {
            InitializeComponent();
        }

        public BenchComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
