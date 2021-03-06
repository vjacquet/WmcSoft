﻿#region Licence

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

namespace WmcSoft.Data
{
    /// <summary>
    /// Adapts a typed enumerable as a <see cref="DataReader"/>.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class TypeDescriptorDataReader<T> : DataReaderBase
    {
        #region Fields

        private readonly IEnumerable<T> data;
        private readonly PropertyDescriptorCollection properties;
        private IEnumerator<T> enumerator;

        #endregion

        #region Lifecycle

        public TypeDescriptorDataReader(IEnumerable<T> data)
            : this(data, TypeDescriptor.GetProperties(typeof(T)))
        {
        }

        public TypeDescriptorDataReader(IEnumerable<T> data, params Attribute[] attributes)
            : this(data, TypeDescriptor.GetProperties(typeof(T), attributes))
        {
        }

        public TypeDescriptorDataReader(IEnumerable<T> data, PropertyDescriptorCollection properties)
        {
            this.properties = properties;
            this.data = data;
            enumerator = this.data.GetEnumerator();
        }

        #endregion

        #region Helpers

        private PropertyDescriptor GetProperty(string name)
        {
            return properties.Find(name, true) ?? throw new IndexOutOfRangeException();
        }

        #endregion

        #region Overrides

        protected override object GetValue(string name)
        {
            return GetProperty(name).GetValue(enumerator.Current);
        }

        /// <summary>
        /// Returns a <see cref="DataRow"/> that describes one column of the <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="i">The index of the column.</param>
        /// <returns>A <see cref="DataRow"/> that describes one column.</returns>
        protected override DataRow GetSchemaRow(DataTable dataTable, int i)
        {
            var property = properties[i];
            var dr = base.GetSchemaRow(dataTable, i);
            if (property.IsReadOnly)
                dr[SchemaTableOptionalColumn.IsReadOnly] = true;
            return dr;
        }

        /// <summary>
        /// Advances the <see cref="IDataReader"/> to the next record.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public override bool Read()
        {
            return enumerator.MoveNext();
        }

        #endregion

        #region IDisposable Membres

        protected override void Dispose(bool disposing)
        {
            if (enumerator is IDisposable disposable) {
                disposable.Dispose();
                enumerator = null;
            }
        }

        #endregion

        #region IDataRecord Membres

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.</value>
        public override int FieldCount => properties.Count;

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The data type information for the specified field.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public override Type GetFieldType(int i)
        {
            return properties[i].PropertyType;
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public override string GetName(int i)
        {
            return properties[i].Name;
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        public override int GetOrdinal(string name)
        {
            var property = GetProperty(name);
            return properties.IndexOf(property);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="IDataRecord.FieldCount"/>.</exception>
        public override object GetValue(int i)
        {
            return properties[i].GetValue(enumerator.Current);
        }

        #endregion
    }
}
