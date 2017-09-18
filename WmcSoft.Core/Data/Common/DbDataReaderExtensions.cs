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
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using WmcSoft.Data.SqlClient;

namespace WmcSoft.Data.Common
{
    /// <summary>
    /// Defines the extension methods to the <see cref="DbDataReader"/> class.
    /// This is a static class. 
    /// </summary>
    public static class DbDataReaderExtensions
    {
        public static T GetObjectFromXml<T>(this DbDataReader reader, int i)
            where T : class
        {
            return GetObjectFromXml<T>(reader, i, new DataContractSerializer(typeof(T)));
        }

        public static T GetObjectFromXml<T>(this DbDataReader reader, int i, IDataContractSurrogate surrogate)
            where T : class
        {
            return GetObjectFromXml<T>(reader, i, new DataContractSerializer(typeof(T), Type.EmptyTypes, Int16.MaxValue, false, true, surrogate));
        }

        public static T GetObjectFromXml<T>(this DbDataReader reader, int i, XmlObjectSerializer serializer)
            where T : class
        {
            var sqlClient = reader as SqlDataReader;
            if (sqlClient != null)
                return SqlDataReaderExtensions.GetObjectFromXml<T>(sqlClient, i, serializer);
            return WmcSoft.Runtime.Serialization.DataRecordExtensions.GetObjectFromXml<T>(reader, i, serializer);
        }
    }
}
