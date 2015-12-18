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

namespace WmcSoft.Data
{
    public static class DataReaderExtensions
    {
        #region ReadXxx

        public static bool ReadNext<T>(this IDataReader reader, Func<IDataRecord, T> materializer, out T entity) {
            if (reader.Read()) {
                entity = materializer(reader);
                return true;
            }

            entity = default(T);
            return false;
        }

        public static IEnumerable<T> ReadAll<T>(this IDataReader reader, Func<IDataRecord, T> materializer) {
            T entity;
            while (reader.ReadNext(materializer, out entity))
                yield return entity;
        }

        #endregion
    }
}
