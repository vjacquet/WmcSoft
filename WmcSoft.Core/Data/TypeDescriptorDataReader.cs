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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace WmcSoft.Data
{
    public class TypeDescriptorDataReader<T> : DataReaderBase
    {
        #region Fields

        private readonly IEnumerable<T> _data;
        private readonly PropertyDescriptorCollection _properties;
        private IEnumerator<T> _enumerator;

        #endregion

        #region Lifecycle

        public TypeDescriptorDataReader(IEnumerable<T> data)
            : this(data, TypeDescriptor.GetProperties(typeof(T))) {
        }

        public TypeDescriptorDataReader(IEnumerable<T> data, Attribute[] attributes)
            : this(data, TypeDescriptor.GetProperties(typeof(T), attributes)) {
        }

        public TypeDescriptorDataReader(IEnumerable<T> data, PropertyDescriptorCollection properties) {
            _properties = properties;
            _data = data;
            _enumerator = _data.GetEnumerator();
        }

        #endregion

        #region Helpers

        private PropertyDescriptor GetProperty(string name) {
            var property = _properties.Find(name, true);
            if (property == null)
                throw new IndexOutOfRangeException();
            return property;
        }

        #endregion

        #region Overrides

        protected override object GetValue(string name) {
            return GetProperty(name).GetValue(_enumerator.Current);
        }

        public override DataTable GetSchemaTable() {
            var dt = new DataTable("SchemaTable");
            dt.Locale = System.Globalization.CultureInfo.InvariantCulture;

            dt.AddColumn<string>(SchemaTableColumn.ColumnName);
            dt.AddColumn<int>(SchemaTableColumn.ColumnOrdinal);
            dt.AddColumn<Type>(SchemaTableColumn.DataType);
            dt.AddColumn<int>(SchemaTableColumn.ColumnSize, defaultValue: -1);
            dt.AddColumn<short>(SchemaTableColumn.NumericPrecision);
            dt.AddColumn<short>(SchemaTableColumn.NumericScale);
            dt.AddColumn<int>(SchemaTableColumn.ProviderType);
            dt.AddColumn<bool>(SchemaTableColumn.IsLong);
            dt.AddColumn<bool>(SchemaTableColumn.AllowDBNull);
            dt.AddColumn<bool>(SchemaTableOptionalColumn.IsReadOnly, defaultValue: false);
            dt.AddColumn<bool>(SchemaTableOptionalColumn.IsRowVersion);
            dt.AddColumn<bool>(SchemaTableColumn.IsUnique);
            dt.AddColumn<bool>(SchemaTableColumn.IsKey);

            int i = 0;
            foreach (var property in _properties.Cast<PropertyDescriptor>()) {
                var dr = dt.NewRow();
                dr[0] = property.Name;
                dr[1] = i;
                dr[2] = property.PropertyType;
                if (property.IsReadOnly)
                    dr[SchemaTableOptionalColumn.IsReadOnly] = true;
                dt.Rows.Add(dr);
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

        public override int FieldCount
        {
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
