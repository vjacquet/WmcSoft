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

namespace WmcSoft
{
    /// <summary>
    /// Represent a version when using semantic versioning.
    /// </summary>
    /// <remarks>See http://developer.telerik.com/featured/mystical-magical-semver-ranges-used-npm-bower/ for more details</remarks>
    public sealed class SemVer : ICloneable<SemVer>, IComparable<SemVer>, IEquatable<SemVer>, IComparable
    {
        // https://github.com/npm/node-semver/blob/master/semver.js
        private readonly int _major;
        private readonly int _minor;
        private readonly int _patch;

        public SemVer(int major, int minor = 0, int patch = 0)
        {
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major));
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor));
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch));

            _major = major;
            _minor = minor;
            _patch = patch;
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

        public int Major => _major;
        public int Minor => _minor;
        public int Patch => _patch;

        public SemVer Clone()
        {
            return (SemVer)MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public override int GetHashCode()
        {
            return ((_major & 15) << 28)
                 | ((_minor & 255) << 20)
                 | ((_patch & 255) << 12);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SemVer);
        }

        public override string ToString()
        {
            return string.Join(".", _major, _minor, _patch);
        }

        public int CompareTo(SemVer other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            var result = _major.CompareTo(other._major);
            if (result != 0)
                return result;

            result = _minor.CompareTo(other._minor);
            if (result != 0)
                return result;

            return _patch.CompareTo(other._patch);
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
            if (ReferenceEquals(other, null))
                return false;
            return _major == other._major
                && _minor == other._minor
                && _patch == other._patch;
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
            if (ReferenceEquals(y, null))
                return false;
            return y.CompareTo(x) > 0;
        }

        public static bool operator >=(SemVer x, SemVer y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);
            return x.CompareTo(y) >= 0;
        }

        public static bool operator >(SemVer x, SemVer y)
        {
            if (ReferenceEquals(x, null))
                return false;
            return x.CompareTo(y) > 0;
        }

        public static bool operator <=(SemVer x, SemVer y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);
            return x.CompareTo(y) <= 0;
        }

        public static bool operator ==(SemVer x, SemVer y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);
            return x.Equals(y);
        }

        public static bool operator !=(SemVer x, SemVer y)
        {
            return !(x == y);
        }
    }
}