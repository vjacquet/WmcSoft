#region Licence

/****************************************************************************
          Copyright 1999-2018 Vincent J. Jacquet.  All rights reserved.

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
using System.Data.SqlClient;

namespace WmcSoft.Data.SqlClient
{
    public class SqlDataType
    {
        private SqlDataType(SqlParameter p)
        {
            DbType = p.SqlDbType;
            Size = p.Size;
            Precision = p.Precision;
            Scale = p.Scale;
        }

        public SqlDataType(SqlDbType dbType, byte precision, byte scale)
            : this(new SqlParameter("p", dbType) { Precision = precision, Scale = scale })
        {
        }

        public SqlDataType(SqlDbType dbType, int size)
            : this(new SqlParameter("p", dbType) { Size = size })
        {
        }

        public SqlDataType(SqlDbType dbType)
           : this(new SqlParameter("p", dbType))
        {
        }

        public SqlDbType DbType { get; }
        public int Size { get; }
        public byte Scale { get; }
        public byte Precision { get; }

        public override string ToString()
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
            switch (DbType) {
            case SqlDbType.Variant:
                return "sql_variant";
            case SqlDbType.BigInt:
            case SqlDbType.Bit:
            case SqlDbType.DateTime:
            case SqlDbType.Image:
            case SqlDbType.Int:
            case SqlDbType.Money:
            case SqlDbType.UniqueIdentifier:
            case SqlDbType.SmallDateTime:
            case SqlDbType.SmallInt:
            case SqlDbType.SmallMoney:
            case SqlDbType.NText:
            case SqlDbType.Text:
            case SqlDbType.Timestamp:
            case SqlDbType.TinyInt:
            case SqlDbType.Xml:
            case SqlDbType.Date:
            case SqlDbType.Time:
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
                return Format(DbType);
            case SqlDbType.Binary:
            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.NVarChar:
            case SqlDbType.VarBinary:
            case SqlDbType.VarChar:
                return Format(DbType) + Format(Size);
            case SqlDbType.Decimal:
                return Format(DbType) + Format(Precision, Scale);
            case SqlDbType.Float:
            case SqlDbType.Real:
                return Format(DbType) + Format(Size);
            case SqlDbType.Udt:
            case SqlDbType.Structured:
            default:
                throw new NotSupportedException();
            }
        }

        static string Format(SqlDbType dbType) => dbType.ToString().ToLowerInvariant();
        static string Format(int size)
        {
            switch (size) {
            case -1: return "(max)";
            case 0: return "";
            default: return ("(" + size + ")");
            };
        }
        static string Format(byte precision, byte scale) => "(" + precision + (scale > 0 ? ("," + scale + ")") : ")");

        #region Factory methods

        static Dictionary<Type, SqlDataType> Defaults = new Dictionary<Type, SqlDataType>()
        {
            { typeof(long), new SqlDataType(SqlDbType.BigInt) },
            { typeof(int), new SqlDataType(SqlDbType.Int) },
        };

        public static SqlDataType Create<T>()
        {
            if (Defaults.TryGetValue(typeof(T), out SqlDataType result))
                return result;
            throw new NotSupportedException();
        }

        #endregion
    }
}
