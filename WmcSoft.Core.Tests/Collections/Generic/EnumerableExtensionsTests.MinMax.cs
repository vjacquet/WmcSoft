using Xunit;

namespace WmcSoft.Collections.Generic
{
    public partial class EnumerableExtensionsTests
    {
        struct NamedValue<T>
        {
            public NamedValue(string name, T value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public T Value { get; }
        }

        static NamedValue<T> Create<T>(string name, T value)
        {
            return new NamedValue<T>(name, value);
        }

        [Fact]
        void CanGetMaxByInt()
        {
            var source = new[] { Create("a", 3), Create("b", 5), Create("c", 2), Create("d", 3), Create("e", 5), Create("f", 4) };
            var max = source.MaxBy(x => x.Value);
            Assert.Equal("e", max.Name);
        }

        [Fact]
        void CanGetMaxByLong()
        {
            var source = new[] { Create("a", 3L), Create("b", 5L), Create("c", 2L), Create("d", 3L), Create("e", 5L), Create("f", 4L) };
            var max = source.MaxBy(x => x.Value);
            Assert.Equal("e", max.Name);
        }

        [Fact]
        void CanGetMaxByDouble()
        {
            var source = new[] { Create("a", 3d), Create("b", 5d), Create("c", 2d), Create("d", 3d), Create("e", 5d), Create("f", 4d) };
            var max = source.MaxBy(x => x.Value);
            Assert.Equal("e", max.Name);
        }

        [Fact]
        void CanGetMaxByFloat()
        {
            var source = new[] { Create("a", 3f), Create("b", 5f), Create("c", 2f), Create("d", 3f), Create("e", 5f), Create("f", 4f) };
            var max = source.MaxBy(x => x.Value);
            Assert.Equal("e", max.Name);
        }

        [Fact]
        void CanGetMinByInt()
        {
            var source = new[] { Create("a", 3), Create("b", 5), Create("c", 2), Create("d", 3), Create("e", 2), Create("f", 4) };
            var min = source.MinBy(x => x.Value);
            Assert.Equal("c", min.Name);
        }

        [Fact]
        void CanGetMinByLong()
        {
            var source = new[] { Create("a", 3L), Create("b", 5L), Create("c", 2L), Create("d", 3L), Create("e", 2L), Create("f", 4L) };
            var min = source.MinBy(x => x.Value);
            Assert.Equal("c", min.Name);
        }

        [Fact]
        void CanGetMinByDouble()
        {
            var source = new[] { Create("a", 3d), Create("b", 5d), Create("c", 2d), Create("d", 3d), Create("e", 2d), Create("f", 4d) };
            var min = source.MinBy(x => x.Value);
            Assert.Equal("c", min.Name);
        }

        [Fact]
        void CanGetMinByFloat()
        {
            var source = new[] { Create("a", 3f), Create("b", 5f), Create("c", 2f), Create("d", 3f), Create("e", 2f), Create("f", 4f) };
            var min = source.MinBy(x => x.Value);
            Assert.Equal("c", min.Name);
        }
    }
}
