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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

    internal class SemVerParser
    {
        const int Plain = 0;
        const int Comparator = 1;
        const int Tilde = 2;
        const int Caret = 3;
        const int XRange = 4;
        const int HyphenRange = 5;
        const int Star = 6;
        const int ComparatorTrim = 7;
        const int TildeTrim = 8;
        const int CaretTrim = 9;

        static Regex[] Initialize(bool strict)
        {
            // ## Numeric Identifier
            // A single `0`, or a non-zero digit followed by zero or more digits.
            var numericIdentifier = strict ? "0|[1-9]\\d*" : "[0-9]+";
            // ## Non-numeric Identifier
            // Zero or more digits, followed by a letter or hyphen, and then zero or
            // more letters, digits, or hyphens.
            var nonNumericIdentifier = "\\d*[a-zA-Z-][a-zA-Z0-9-]*";
            // ## Main Version
            // Three dot-separated numeric identifiers.
            var mainVersion = "(" + numericIdentifier + ")\\.(" + numericIdentifier + ")\\.(" + numericIdentifier + ")";
            // ## Pre-release Version Identifier
            // A numeric identifier, or a non-numeric identifier.
            var prereleaseIdentifier = "(?:" + numericIdentifier + "|" + nonNumericIdentifier + ")";
            // ## Pre-release Version
            // Hyphen, followed by one or more dot-separated pre-release version
            // identifiers.
            var prerelease = strict
                ? "(?:-(" + prereleaseIdentifier + "(?:\\." + prereleaseIdentifier + ")*))"
                : "(?:-?(" + prereleaseIdentifier + "(?:\\." + prereleaseIdentifier + ")*))";
            // ## Build Metadata Identifier
            // Any combination of digits, letters, or hyphens.
            var buildIdentifier = "[0-9A-Za-z-]+";
            // ## Build Metadata
            // Plus sign, followed by one or more period-separated build metadata
            // identifiers.
            var build = "(?:\\+(" + buildIdentifier + "(?:\\." + buildIdentifier + ")*))";

            // ## Full Version String
            // A main version, followed optionally by a pre-release version and
            // build metadata.

            // Note that the only major, minor, patch, and pre-release sections of
            // the version string are capturing groups.  The build metadata is not a
            // capturing group, because it should not ever be used in version
            // comparison.

            // The loose version is like full, but allows v1.2.3 and =1.2.3, which people do sometimes.
            // also, 1.0.0alpha1 (prerelease without the hyphen) which is pretty
            // common in the npm registry.

            var plain = strict
                ? "^v?" + mainVersion + prerelease + "?" + build + "?$"
                : "^[v=\\s]*" + mainVersion + prerelease + "?" + build + "?$";

            var gtlt = "((?:<|>)?=?)";

            // Something like "2.*" or "1.2.x".
            // Note that "x.x" is a valid xRange identifer, meaning "any version"
            // Only the first item is strictly required.
            var xrangeIdentifier = numericIdentifier + "|x|X|\\*";
            var xrangePlain = "[v=\\s]*(" + xrangeIdentifier + ")"
                + "(?:\\.(" + xrangeIdentifier + ")"
                + "(?:\\.(" + xrangeIdentifier + ")"
                + "(?:" + prerelease + ")?"
                + build + "?"
                + ")?)?";
            var xrange = "^" + gtlt + "\\s*" + xrangePlain + "$";

            // Tilde ranges.
            // Meaning is "reasonably at or greater than"
            var loneTilde = "(?:~>?)";
            var tildeTrim = "(\\s*)" + loneTilde + "\\s+";
            var tilde = "^" + loneTilde + xrangePlain + "$";

            // Caret ranges.
            // Meaning is "at least and backwards compatible with"
            var loneCaret = "(?:\\^)";
            var caretTrim = "(\\s*)" + loneCaret + "\\s+";
            var caret = "^" + loneCaret + xrangePlain + "$";

            // A simple gt/lt/eq thing, or just "" to indicate "any version"
            var comparator = "^" + gtlt + "\\s*(" + plain + ")$|^$";

            // An expression to strip any whitespace between the gtlt and the thing
            // it modifies, so that `> 1.2.3` ==> `>1.2.3`
            var comparatorTrim = "(\\s*)" + gtlt + "\\s*(" + plain + "|" + xrangePlain + ")";

            // Something like `1.2.3 - 1.2.4`
            // Note that these all use the loose form, because they'll be
            // checked against either the strict or loose comparator form
            // later.
            var hyphenRange = "^\\s*(" + xrangePlain + ")\\s+-\\s+(" + xrangePlain + ")\\s*$";

            // Star ranges basically just allow anything at all.
            var star = "(<|>)?=?\\s*\\*";

            return new[] {
                new Regex(plain),
                new Regex(comparator),
                new Regex(tilde),
                new Regex(caret),
                new Regex(xrange),
                new Regex(hyphenRange),
                new Regex(star),
                new Regex(comparatorTrim),
                new Regex(tildeTrim),
                new Regex(caretTrim),
            };
        }

        static Regex[] Strict = Initialize(true);
        static Regex[] Loose = Initialize(false);

        public static bool TryParse(string s, bool loose, out SemVer result)
        {
            if (!string.IsNullOrWhiteSpace(s)) {
                var expressions = loose ? Loose : Strict;
                s = s.Trim();
                var m = expressions[Plain].Match(s);
                if (m.Success) {
                    int major = int.Parse(m.Groups[1].Value);
                    int minor = int.Parse(m.Groups[2].Value);
                    int path = int.Parse(m.Groups[3].Value);
                    string prerelease = m.Groups[4].Value;
                    if (prerelease == string.Empty)
                        prerelease = null;
                    result = new SemVer(major, minor, path, prerelease);
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}
