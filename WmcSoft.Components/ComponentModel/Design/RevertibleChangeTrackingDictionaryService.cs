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
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
    public sealed class RevertibleChangeTrackingDictionaryService : IDictionaryService, IRevertibleChangeTracking
    {
        #region Fields

        static readonly object Removed = new object();

        readonly IDictionaryService _dictionaryService;
        readonly Hashtable _changes;

        #endregion

        public RevertibleChangeTrackingDictionaryService(IDictionaryService dictionaryService) {
            _dictionaryService = dictionaryService;
            _changes = new Hashtable();
        }

        #region IDictionaryService Members

        public object GetKey(object value) {
            throw new NotImplementedException();
        }

        public object GetValue(object key) {
            var value = _changes[key] ?? _dictionaryService.GetValue(key);
            if (ReferenceEquals(Removed, value))
                return null;
            return value;
        }

        public void SetValue(object key, object value) {
            _changes[key] = value ?? Removed;
        }

        #endregion

        #region IRevertibleChangeTracking Members

        public void RejectChanges() {
            _changes.Clear();
        }

        #endregion

        #region IChangeTracking Members

        public void AcceptChanges() {
            foreach (DictionaryEntry dictionaryEntry in _changes) {
                var value = ReferenceEquals(Removed, dictionaryEntry.Value) ? null : dictionaryEntry.Value;
                _dictionaryService.SetValue(dictionaryEntry.Key, value);
            }
        }

        public bool IsChanged {
            get { return _changes.Count > 0; }
        }

        #endregion
    }
}
