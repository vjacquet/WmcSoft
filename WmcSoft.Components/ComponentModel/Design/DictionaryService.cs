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

using System.Collections;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
    /// <summary>
    /// Provides an eager implementation of the IDictionaryService.
    /// </summary>
    /// <remarks>The use of this class is not restricted to designers.</remarks>
    public class DictionaryService : IDictionaryService
    {
        #region Fields

        readonly IDictionary _dictionary;

        #endregion

        #region Lifecycle

        public DictionaryService()
            : this(new Hashtable())
        {
        }

        internal DictionaryService(IDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        #endregion

        #region IDictionaryService Members

        public object GetKey(object value)
        {
            if (value != null) {
                foreach (DictionaryEntry dictionaryEntry in _dictionary) {
                    if (value.Equals(dictionaryEntry.Value)) {
                        return dictionaryEntry.Key;
                    }
                }
            }
            return null;
        }

        public object GetValue(object key)
        {
            return _dictionary[key];
        }

        public void SetValue(object key, object value)
        {
            if (value == null) {
                _dictionary.Remove(key);
            } else {
                _dictionary[key] = value;
            }
        }

        #endregion
    }
}
