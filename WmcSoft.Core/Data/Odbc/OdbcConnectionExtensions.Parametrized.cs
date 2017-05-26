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
using System.Data.Odbc;
using System.Diagnostics.CodeAnalysis;

namespace WmcSoft.Data.Odbc
{
    public static partial class OdbcConnectionExtensions
    {
        static string CaptureParameter(OdbcParameter parameter, string specification)
        {
            if (!string.IsNullOrEmpty(specification))
                parameter.OdbcType = (OdbcType)Enum.Parse(typeof(OdbcType), specification, true);
            return "?";
        }

        [SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities")]
        public static OdbcCommand CreateParametrizedCommand(this OdbcConnection connection, FormattableString commandText, TimeSpan? timeout = null, IDbTransaction transaction = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var command = (OdbcCommand)connection.CreateCommand(timeout, transaction);
            return command.CaptureParameters<OdbcCommand, OdbcParameter>(CaptureParameter, commandText);
        }
    }
}