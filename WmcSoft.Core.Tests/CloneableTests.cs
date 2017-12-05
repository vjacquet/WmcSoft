using System;
using Xunit;

namespace WmcSoft
{
    public class CloneableTests
    {
        public class BaseClass : ICloneable
        {
            public int A { get; set; }

            protected virtual object Clone(Cloning.Deep strategy)
            {
                return MemberwiseClone();
            }

            object ICloneable.Clone()
            {
                return Clone(Cloning.SuggestDeep);
            }
        }

        public class DerivedClass : BaseClass
        {
            public int B { get; set; }
        }

        [Fact]
        public void CanCloneDerivedClass()
        {
            var expected = new DerivedClass {
                A = 1,
                B = 2
            };
            var clone = expected.Clone();

            Assert.IsType<DerivedClass>(clone);
            Assert.Equal(expected.A, clone.A);
            Assert.Equal(expected.B, clone.B);
        }
    }
}
