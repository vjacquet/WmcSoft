using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.ComponentModel.Design
{
    public sealed class RevertibleChangeTrackingDictionaryService : IDictionaryService, IRevertibleChangeTracking
    {
        #region Fields

        static readonly object Removed = new Object();

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
            if (Object.ReferenceEquals(Removed, value))
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
                var value = Object.ReferenceEquals(Removed, dictionaryEntry.Value) ? null : dictionaryEntry.Value;
                _dictionaryService.SetValue(dictionaryEntry.Key, value);
            }
        }

        public bool IsChanged {
            get { return _changes.Count > 0; }
        }

        #endregion
    }
}
