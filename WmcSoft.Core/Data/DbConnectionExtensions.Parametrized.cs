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
using System.Data;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace WmcSoft.Data
{
    public static partial class DbConnectionExtensions
    {
        #region FormatCapturingParameter

        class FormatCapturingParameter : IFormattable
        {
            private readonly IDbDataParameter _parameter;
            private readonly string _placeholder;

            internal FormatCapturingParameter(IDbDataParameter parameter, string placeholder = "?")
            {
                _parameter = parameter;
                _placeholder = placeholder;
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (!string.IsNullOrEmpty(format)) {
                    _parameter.DbType = (DbType)Enum.Parse(typeof(DbType), format, true);
                }
                return _placeholder;
            }
        }

        static Func<IDbDataParameter, FormatCapturingParameter> GetCaptureFactory(IDbConnection connection)
        {
            switch (connection.GetType().FullName) {
            case "System.Data.SqlClient.SqlConnection":
                return _ => new FormatCapturingParameter(_, "@" + _.ParameterName);
            case "System.Data.OracleClient.OracleConnection":
                return _ => new FormatCapturingParameter(_, ":" + _.ParameterName);
            default:
                return _ => new FormatCapturingParameter(_);
            }
        }

        #endregion

        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public static IDbCommand CreateParametrizedCommand(this IDbConnection connection, FormattableString commandText, CommandType commandType = CommandType.Text, TimeSpan? timeout = null, IDbTransaction transaction = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            // inspired from <https://github.com/jskeet/DemoCode/blob/master/Abusing%20CSharp/Code/StringInterpolation/ParameterizedSql.cs>
            // How to handle different connection? Should we implement a specialized version and have the base version handle a dynamic dispatch and 
            var capture = GetCaptureFactory(connection);

            var command = connection.CreateCommand();
            command.CommandType = commandType;
            var length = commandText.ArgumentCount;
            var args = new object[length];
            for (int i = 0; i < length; i++) {
                var p = command.CreateParameter();
                p.ParameterName = "p" + i.ToString(CultureInfo.CurrentCulture);
                p.Value = commandText.GetArgument(i);
                command.Parameters.Add(p);
                args[i] = capture(p);
            }
            command.CommandText = string.Format(commandText.Format, args);
            command.Transaction = transaction;
            if (timeout != null)
                command.CommandTimeout = (int)Math.Max(timeout.GetValueOrDefault().TotalSeconds, 1d);
            return command;
        }
    }
}
