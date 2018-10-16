using Xunit;

namespace WmcSoft.Numerics
{
    public class BoolarrayTests
    {
        [Fact]
        public void CheckBoolarrayTruthOperators()
        {
            var x = new Boolarray(true, true, true, true);
            var y = new Boolarray(true, true, false, true);
            var z = new Boolarray(true, true, true, true);

            if (x) { } else { Assert.True(false, "x"); }

            if (!x) { Assert.True(false, "!x"); } else { }

            if (y) { Assert.True(false, "y"); } else { }

            if (x && z) { } else { Assert.True(false, "x && z"); }

            if (x || z) { } else { Assert.True(false, "x || z"); }

            if (x || y) { } else { Assert.True(false, "x || y"); }

            if (x && !y) { } else { Assert.True(false, "x && !y"); }
        }

        [Fact]
        public void CanCreateEmptyBoolarray()
        {
            var boolarray = new Boolarray();
            Assert.NotNull(boolarray);
            Assert.Equal(0, boolarray.Cardinality);
            Assert.NotSame(Valarray.Empty, boolarray);
        }
    }
}
