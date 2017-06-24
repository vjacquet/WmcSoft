using System.Collections.Generic;
using Xunit;

namespace WmcSoft
{
    public class WeakReferenceTests
    {
        [Fact]
        public void CanCreateWeakReference()
        {
            var r = new WeakReference<List<int>>(new List<int>());
            r.Target.Add(5);
        }
    }
}
