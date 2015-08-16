using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace WmcSoft.Data
{
    public class TypeDescriptorDataReader<T> : DataReaderBase
    {
        private readonly IEnumerable<T> _data;
        private readonly PropertyDescriptorCollection _properties;
        private IEnumerator<T> _enumerator;

        public TypeDescriptorDataReader(IEnumerable<T> data) {
            _properties = TypeDescriptor.GetProperties(typeof(T));
            _data = data;
            _enumerator = _data.GetEnumerator();
        }

        private PropertyDescriptor GetProperty(string name) {
            var property = _properties.Find(name, true);
            if (property == null)
                throw new IndexOutOfRangeException();
            return property;
        }

        #region Overrides

        protected override object GetValue(string name) {
            return GetProperty(name).GetValue(_enumerator.Current);
        }

        public override DataTable GetSchemaTable() {
            var dt = new DataTable(typeof(T).Name);
            foreach (var property in _properties.Cast<PropertyDescriptor>()) {
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            return dt;
        }

        public override bool Read() {
            return _enumerator.MoveNext();
        }

        #endregion

        #region IDisposable Membres

        protected override void Dispose(bool disposing) {
            var disposable = _enumerator as IDisposable;
            if (disposable != null) {
                disposable.Dispose();
                _enumerator = null;
            }
        }

        #endregion

        #region IDataRecord Membres

        public override int FieldCount {
            get { return _properties.Count; }
        }

        public override Type GetFieldType(int i) {
            return _properties[i].PropertyType;
        }

        public override string GetName(int i) {
            return _properties[i].Name;
        }

        public override int GetOrdinal(string name) {
            var property = GetProperty(name);
            return _properties.IndexOf(property);
        }

        public override object GetValue(int i) {
            return _properties[i].GetValue(_enumerator.Current);
        }

        #endregion
    }
}
