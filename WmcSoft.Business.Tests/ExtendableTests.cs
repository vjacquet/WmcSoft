using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WmcSoft.Business
{
    public class ExtendableTests
    {
        public interface IKernelExtension
        {
            void Configure(IServiceProvider provider);
        }

        public class Kernel : IExtendable<IKernelExtension>
        {
            private readonly IKernelExtension[] _extensions;

            public Kernel(params IKernelExtension[] extensions)
            {
                if (extensions == null) {
                    _extensions = new IKernelExtension[0];
                } else {
                    var types = new HashSet<Type>();
                    foreach (var extension in extensions) {
                        if (extension == null) throw new ArgumentException();
                        if (!types.Add(extension.GetType())) throw new ArgumentException();
                    }
                    _extensions = extensions;
                }
            }

            public IEnumerable<IKernelExtension> Extensions {
                get { return _extensions; }
            }

            public TExtension FindExtension<TExtension>()
                where TExtension : class, IKernelExtension
            {
                return Extensions.OfType<TExtension>().SingleOrDefault();
            }
        }

        class KernelExtensionsA : IKernelExtension
        {
            public void Configure(IServiceProvider provider)
            {
            }
        }

        class KernelExtensionsB : IKernelExtension
        {
            public void Configure(IServiceProvider provider)
            {
            }
        }

        [Fact]
        public void CanFindExtension()
        {
            var kernel = new Kernel(new KernelExtensionsA(), new KernelExtensionsB());
            Assert.NotNull(kernel.FindExtension<KernelExtensionsA>());
        }
    }
}
