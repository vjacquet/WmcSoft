#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;
using WmcSoft.Internals;

namespace WmcSoft
{
    /// <summary>
    /// Represent a version when using semantic versioning.
    /// </summary>
    /// <remarks>See http://developer.telerik.com/featured/mystical-magical-semver-ranges-used-npm-bower/ for more details</remarks>
    public sealed class SemVer : ICloneable, IComparable<SemVer>, IEquatable<SemVer>, IComparable
    {
        // https://github.com/npm/node-semver/blob/master/semver.js

        const int MaxLength = 256;
        const int MaxSageInteger = int.MaxValue; //  is 9007199254740991 in Javascript

        private readonly int _major;
        private readonly int _minor;
        private readonly int _patch;
        private readonly string _prerelease;
        private readonly string _build;

        public SemVer(int major, int minor = 0, int patch = 0, string prerelease = null, string build = null)
        {
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major));
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor));
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch));

            _major = major;
            _minor = minor;
            _patch = patch;
            _prerelease = string.IsNullOrWhiteSpace(prerelease) ? null : prerelease.Trim();
            _build = string.IsNullOrWhiteSpace(build) ? null : build.Trim();
        }

        public SemVer(Version version)
        {
            if (version == null) throw new ArgumentNullException(nameof(version));

            _major = version.Major;
            _minor = version.Minor;
            _patch = version.Build;
        }

        public void Deconstruct(out int major, out int minor, out int patch)
        {
            major = _major;
            minor = _minor;
            patch = _patch;
        }

        public void Deconstruct(out int major, out int minor, out int patch, out string prerelease)
        {
            major = _major;
            minor = _minor;
            patch = _patch;
            prerelease = _prerelease;
        }

        public void Deconstruct(out int major, out int minor, out int patch, out string prerelease, out string build)
        {
            major = _major;
            minor = _minor;
            patch = _patch;
            prerelease = _prerelease;
            build = _build;
        }

        public int Major => _major;
        public int Minor => _minor;
        public int Patch => _patch;
        public string Prerelease => _prerelease;
        public string Build => _build;

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        public override int GetHashCode()
        {
            var m = ((_major & 15) << 28)
                 | ((_minor & 255) << 20)
                 | ((_patch & 255) << 12);
            if (_prerelease == null)
                return m;
            return m | (_prerelease.GetHashCode() % 0x0fff);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SemVer);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_major)
                .Append('.').Append(_minor)
                .Append('.').Append(_patch);
            if (_prerelease != null)
                sb.Append('-').Append(_prerelease);
            return sb.ToString();
        }

        public int CompareTo(SemVer other)
        {
            if (other is null)
                return 1;
            var result = _major.CompareTo(other._major);
            if (result != 0)
                return result;

            result = _minor.CompareTo(other._minor);
            if (result != 0)
                return result;

            result = _patch.CompareTo(other._patch);
            if (result != 0)
                return result;

            if (_prerelease == null)
                return other._prerelease == null ? 0 : 1;
            if (other._prerelease == null)
                return -1;
            return string.Compare(_prerelease, other._prerelease); // incomplete
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) return 1;
            var version = obj as SemVer;
            if (version == null) throw new ArgumentException();
            return CompareTo(version);
        }

        public bool Equals(SemVer other)
        {
            if (other is null)
                return false;
            return _major == other._major
                && _minor == other._minor
                && _patch == other._patch
                && _prerelease == other._prerelease;
        }

        public static implicit operator SemVer(Version version)
        {
            return new SemVer(version);
        }

        public SemVer NextMajor()
        {
            return new SemVer(_major + 1, 0, 0);
        }

        public SemVer NextMinor()
        {
            return new SemVer(_major, _minor + 1, 0);
        }

        public SemVer Increment()
        {
            return new SemVer(_major, _minor, _patch + 1);
        }

        public SemVer Increment(SemVerRelease release)
        {
            switch (release) {
            case SemVerRelease.Major:
                return NextMajor();
            case SemVerRelease.Minor:
                return NextMinor();
            case SemVerRelease.Patch:
            default:
                return Increment();
            }
        }

        public static SemVer operator ++(SemVer value)
        {
            return value.Increment();
        }

        public static bool operator <(SemVer x, SemVer y)
        {
            if (y is null)
                return false;
            return y.CompareTo(x) > 0;
        }

        public static bool operator >=(SemVer x, SemVer y)
        {
            if (x is null)
                return y is null;
            return x.CompareTo(y) >= 0;
        }

        public static bool operator >(SemVer x, SemVer y)
        {
            if (x is null)
                return false;
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(SemVer x, SemVer y)
        {
            if (x is null)
                return y is null;
            return x.CompareTo(y) <= 0;
        }

        public static bool operator ==(SemVer x, SemVer y)
        {
            if (x is null)
                return y is null;
            return x.Equals(y);
        }

        public static bool operator !=(SemVer x, SemVer y)
        {
            return !(x == y);
        }

        public static bool TryParse(string s, out SemVer result)
        {
            return TryParse(s, false, out result);
        }

        public static bool TryParse(string s, bool loose, out SemVer result)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            return SemVerParser.TryParse(s, loose, out result);
        }

        public static SemVer Parse(string s, bool loose = false)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            if (!TryParse(s, loose, out var result))
                throw new FormatException();
            return result;
        }
    }
}
