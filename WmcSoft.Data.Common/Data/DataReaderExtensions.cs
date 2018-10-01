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
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using WmcSoft.Data.Common;

namespace WmcSoft.Data
{
    /// <summary>
    /// Defines the extension methods to the <see cref="IDataReader"/> interface.
    /// This is a static class. 
    /// </summary>
    public static class DataReaderExtensions
    {
        #region AsDbDataReader

        public static DbDataReader AsDbDataReader(this IDataReader reader)
        {
            if (reader == null) throw new NullReferenceException("reader, the `this` parameter of the extension method should not be null.");

            var db = reader as DbDataReader;
            return db ?? new DbDataReaderAdapter(reader);
        }

        #endregion

        #region ReadXxx

        /// <summary>
        /// Reads and materializes the next record.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="materializer">The materializer.</param>
        /// <param name="entity">The materialized entity.</param>
        /// <returns><c>true</c> if there was a next element. Otherwise, false</returns>
        public static bool ReadNext<T>(this IDataReader reader, Func<IDataRecord, T> materializer, out T entity)
        {
            Debug.Assert(reader != null);
            if (materializer == null) throw new ArgumentNullException(nameof(materializer));

            if (reader.Read()) {
                entity = materializer(reader);
                return true;
            }

            entity = default(T);
            return false;
        }

        static IEnumerable<T> UnguardedReadAll<T>(IDataReader reader, Func<IDataRecord, T> materializer)
        {
            while (reader.ReadNext(materializer, out var entity))
                yield return entity;
        }

        /// <summary>
        /// Reads and materializes all records.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <param name="materializer">The materializer.</param>
        /// <returns>The entities.</returns>
        public static IEnumerable<T> ReadAll<T>(this IDataReader reader, Func<IDataRecord, T> materializer)
        {
            Debug.Assert(reader != null);
            if (materializer == null) throw new ArgumentNullException(nameof(materializer));

            return UnguardedReadAll(reader, materializer);
        }

        #endregion
    }
}
