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
    /// <remarks>See http://developer.telerik.com/featured/mystical-magical-semver-ranges-used-npm-bower/ </remarks>
    public sealed class SemVer : ICloneable<SemVer>, IComparable<SemVer>, IEquatable<SemVer>, IComparable
    {
        private readonly Version _version;

        private SemVer(Version version, PiecewiseConstruct tag)
        {
            _version = version;
        }

        public SemVer(int major, int minor = 0, int patch = 0)
        {
            _version = new Version(major, minor, patch);
        }

        public SemVer(Version version)
        {
            if (version == null) throw new ArgumentNullException(nameof(version));

            _version = (version.Revision != 0)
                ? new Version(version.Major, version.Minor, version.Build)
                : version;
        }

        public void Deconstruct(out int major, out int minor, out int patch)
        {
            major = _version.Major;
            minor = _version.Minor;
            patch = _version.Build;
        }

        public int Major { get { return _version.Major; } }
        public int Minor { get { return _version.Minor; } }
        public int Patch { get { return _version.Build; } }

        public SemVer Clone()
        {
            return new SemVer(_version, PiecewiseConstruct.Tag);
        }

        public override int GetHashCode()
        {
            return _version.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SemVer);
        }

        public override string ToString()
        {
            return _version.ToString(3);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public int CompareTo(SemVer other)
        {
            if (ReferenceEquals(other, null))
                return 1;
            return _version.CompareTo(other._version);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            var other = obj as SemVer;
            if (ReferenceEquals(other, null))
                throw new ArgumentException(nameof(obj));
            return _version.CompareTo(other._version);
        }

        public bool Equals(SemVer other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return _version.Equals(other._version);
        }

        public static implicit operator SemVer(Version version)
        {
            return new SemVer(version);
        }

        public SemVer NextMajor()
        {
            return new SemVer(_version.Major + 1, 0, 0);
        }

        public SemVer NextMinor()
        {
            return new SemVer(_version.Major, _version.Minor + 1, 0);
        }

        public SemVer Increment()
        {
            return new SemVer(_version.Major, _version.Minor, _version.Build + 1);
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