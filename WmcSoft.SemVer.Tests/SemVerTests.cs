using System;
using Xunit;

namespace WmcSoft
{
    public class SemVerTests
    {
        [Fact]
        public void CanCreateSemVerFromVersion()
        {
            var version = new Version(1, 2, 3, 4);
            var semver = (SemVer)version;

            Assert.Equal(version.Major, semver.Major);
            Assert.Equal(version.Minor, semver.Minor);
            Assert.Equal(version.Build, semver.Patch);
        }

        [Fact]
        public void CanDeconstructSemVer()
        {
            var semver = new SemVer(1, 2, 3);
            var (major, minor, patch) = semver;
            Assert.Equal(1, major);
            Assert.Equal(2, minor);
            Assert.Equal(3, patch);
        }

        [Theory]
        [InlineData(1, 2, 3, null, "1.2.3")]
        [InlineData(2, 0, 0, null, "2.0.0")]
        public void CanConvertSemVerToString(int major, int minor, int patch, string prerelease, string expected)
        {
            var semver = new SemVer(major, minor, patch, prerelease);
            var actual = semver.ToString();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 3, null, "1.2.4")]
        public void CanIncrement(int major, int minor, int patch, string prerelease, string expected)
        {
            var semver = new SemVer(major, minor, patch, prerelease);
            semver++;
            var actual = semver.ToString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCompare()
        {
            SemVer v0 = null;
            SemVer w0 = null;
            var v1 = new SemVer(1, 2, 3);
            var v2 = new SemVer(1, 2, 4);
            var v3 = new SemVer(1, 3, 3);
            var v4 = new SemVer(2, 2, 3);

            Assert.True(v1 < v2);
            Assert.True(v2 > v1);
            Assert.True(v1 <= v2);
            Assert.True(v2 >= v1);
            Assert.True(v2 < v3);
            Assert.True(v3 < v4);

            Assert.False(v1 < v0);
            Assert.False(v0 > v1);
            Assert.True(v1 <= v2);
            Assert.True(v2 >= v1);

            Assert.True(v0 == w0);
            Assert.False(v0 != w0);
        }

        [Theory]
        [InlineData("1.2.3", 1, 2, 3, null, null)]
        [InlineData("2.0.0", 2, 0, 0, null, null)]
        //[InlineData("1.0.0-alpha+001", 1, 0, 0, "alpha", "001", Skip = "Not ready")]
        public void CanParseStrict(string s, int major, int minor, int patch, string prerelease, string build)
        {
            var v = SemVer.Parse(s);
            Assert.True(v.Major == major);
            Assert.True(v.Minor == minor);
            Assert.True(v.Patch == patch);
            Assert.True(v.Prerelease == prerelease);
            Assert.True(v.Build == build);
        }
    }
}
