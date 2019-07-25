#region Licence

/****************************************************************************
          Copyright 1999-2017 Vincent J. Jacquet.  All rights reserved.

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
            // inspired from <https://github.com/jskeet/DemoCode/blob/master/Abusing%20CSharp/Code/StringInterpolation/ParameterizedSql.cs>
            private readonly Func<string, string> _capture;

            internal FormatCapturingParameter(Func<string, string> capture)
            {
                _capture = capture;
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return _capture(format);
            }
        }

        #endregion

        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public static TCommand CaptureParameters<TCommand, TParameter>(this TCommand command, Func<TParameter, string, string> capture, FormattableString commandText)
            where TCommand : IDbCommand
            where TParameter : IDbDataParameter
        {
            command.CommandType = CommandType.Text;
            var length = commandText.ArgumentCount;
            var args = new object[length];
            for (int i = 0; i < length; i++) {
                var p = (TParameter)command.CreateParameter();
                p.ParameterName = "p" + i.ToString(CultureInfo.CurrentCulture);
                p.Value = commandText.GetArgument(i) ?? DBNull.Value;
                command.Parameters.Add(p);
                args[i] = new FormatCapturingParameter(_ => capture(p, _));
            }
            command.CommandText = string.Format(commandText.Format, args);
            return command;
        }

        [Obsolete("To prevent security vulnerabilities, you must call CreateParameterizedCommand when using string interpolation for the command.", true)]
        public static IDbCommand CreateCommand(this IDbConnection connection, FormattableString commandText, TimeSpan? timeout = null, IDbTransaction transaction = null)
        {
            throw new ArgumentException("To prevent security vulnerabilities, you must call CreateParameterizedCommand when using string interpolation for the command.", nameof(commandText));
        }

        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public static IDbCommand CreateParameterizedCommand(this IDbConnection connection, Func<IDbDataParameter, string, string> capture, FormattableString commandText, TimeSpan? timeout = null, IDbTransaction transaction = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            return CaptureParameters(connection.CreateCommand(timeout, transaction), capture, commandText);
        }
    }
}
