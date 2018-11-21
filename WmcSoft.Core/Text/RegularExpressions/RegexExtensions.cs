#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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
using System.Text.RegularExpressions;

namespace WmcSoft.Text.RegularExpressions
{
    /// <summary>
    /// Provides a set of static methods to extend the <see cref="Regex"/> class.
    /// This is a static class.
    /// </summary>
    public static class RegexExtensions
    {
        /// <summary>
        /// Gets the value of the specified group if successful.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="groupName">The group's name.</param>
        /// <returns>The value of the group if successful; otherwise, <c>null</c>.</returns>
        public static string GetGroupValue(this Match match, string groupName)
        {
            var value = match.Groups[groupName];
            if (value.Success)
                return value.Value;
            return null;
        }

        /// <summary>
        /// Gets the value of the specified group if successful.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="match">The match.</param>
        /// <param name="groupName">The group's name.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information. </param>
        /// <returns>The value of the group if successful; otherwise, <c>null</c>.</returns>
        public static T? GetNullableGroupValue<T>(this Match match, string groupName, IFormatProvider provider = null)
            where T : struct
        {
            var value = match.Groups[groupName];
            if (value.Success)
                return (T)Convert.ChangeType(value.Value, typeof(T), provider);
            return null;
        }

        /// <summary>
        /// Gets the value of the specified group if successful.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="match">The match.</param>
        /// <param name="groupName">The group's name.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information. </param>
        /// <returns>The value of the group if successful; otherwise, <c>null</c>.</returns>
        public static T GetGroupValue<T>(this Match match, string groupName, IFormatProvider provider = null)
        {
            var value = match.Groups[groupName];
            if (value.Success)
                return (T)Convert.ChangeType(value.Value, typeof(T), provider);
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Gets the value of the specified group if successful.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="match">The match.</param>
        /// <param name="groupName">The group's name.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information. </param>
        /// <returns>The value of the group if successful; otherwise, <c>null</c>.</returns>
        public static T GetGroupValueOrDefault<T>(this Match match, string groupName, T defaultValue = default, IFormatProvider provider = null)
        {
            var value = match.Groups[groupName];
            if (value.Success)
                return (T)Convert.ChangeType(value.Value, typeof(T), provider);
            return defaultValue;
        }

        /// <summary>
        /// Searches the specified input string for the first occurrence of the regular expression specified in the <see cref="Regex"/>constructor.
        /// </summary>
        /// <param name="regex">The regular expression.</param>
        /// <param name="input">The string to search for a match. </param>
        /// <param name="match">An object that contains information about the match.</param>
        /// <returns><c>true</c> if the match is successfull; otherwise, <c>false</c>.</returns>
        public static bool TryMatch(this Regex regex, string input, out Match match)
        {
            match = regex.Match(input);
            return match.Success;
        }
    }
}
