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

using System.Text.RegularExpressions;

namespace WmcSoft.Internals
{
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
