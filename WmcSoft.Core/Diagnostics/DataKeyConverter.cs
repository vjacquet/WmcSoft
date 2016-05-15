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
using System.Collections;

namespace WmcSoft.Diagnostics
{
    /// <summary>
    /// Defines strategies to convert value to key to <see cref="Exception"/>'s <see cref="Exception.Data"/> property keys.
    /// This is a static class.
    /// </summary>
    public static class DataKeyConverter
    {
        #region Internals

        public struct BasicKeyConverter : IDataKeyConverter
        {
            #region IKeyConverter Membres

            public object ConvertTo(string name) {
                return name;
            }

            public string ConvertFrom(object key) {
                return key as string;
            }

            #endregion
        }

        public struct PreventConflictKeyConverter : IDataKeyConverter
        {
            [Serializable]
            sealed class NoKeyConflict
            {
                readonly string _name;

                public NoKeyConflict(string name) {
                    _name = name;
                }

                public override int GetHashCode() {
                    return _name.GetHashCode();
                }

                public override bool Equals(object obj) {
                    if (obj == null || obj.GetType() != typeof(NoKeyConflict))
                        return false;

                    return _name.Equals(((NoKeyConflict)obj)._name);
                }

                public override string ToString() {
                    return _name.ToString();
                }

                public bool Match(Predicate<string> predicate) {
                    return predicate(_name);
                }
            }

            #region IKeyConverter Membres

            public object ConvertTo(string name) {
                return new NoKeyConflict(name);
            }

            public string ConvertFrom(object key) {
                var name = key as NoKeyConflict;
                if (name != null) {
                    return name.ToString();
                }
                return null;
            }

            #endregion
        }

        public struct PrefixedKeyConverter : IDataKeyConverter
        {
            internal readonly IDataKeyConverter _converter;
            internal readonly string _prefix;

            public PrefixedKeyConverter(IDataKeyConverter converter, string prefix) {
                _converter = converter;
                _prefix = prefix;
            }

            #region IKeyConverter Membres

            public object ConvertTo(string name) {
                return _converter.ConvertTo(_prefix + name);
            }

            public string ConvertFrom(object key) {
                var name = _converter.ConvertFrom(key);
                if (name == null || !name.StartsWith(_prefix))
                    return null;
                return name.Substring(_prefix.Length);
            }

            #endregion
        }

        #endregion

        static DataKeyConverter() {
            Basic = new BasicKeyConverter();
            PreventConflict = new PreventConflictKeyConverter();
            Default = PreventConflict;
        }

        public static IDataKeyConverter Default { get; set; }
        public static BasicKeyConverter Basic { get; private set; }
        public static PreventConflictKeyConverter PreventConflict { get; private set; }

        public static PrefixedKeyConverter WithPrefix(this IDataKeyConverter converter, string prefix) {
            if (converter is PrefixedKeyConverter) {
                // decorating a PrefixedKeyConverter by another one would invert the prefixes.
                // and it is better to concatenate only onces.
                var decorated = (PrefixedKeyConverter)converter;
                return new PrefixedKeyConverter(decorated._converter, prefix + decorated._prefix);
            }
            return new PrefixedKeyConverter(converter, prefix);
        }

        public static bool IsSupported(this IDataKeyConverter converter, object key) {
            return converter.ConvertFrom(key) != null;
        }

        public static bool IsSupported(this IDataKeyConverter converter, DictionaryEntry entry) {
            return IsSupported(converter, entry.Key);
        }
    }
}
