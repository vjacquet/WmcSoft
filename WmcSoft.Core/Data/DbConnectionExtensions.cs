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

using System.Configuration;
using System.Data.Common;

namespace WmcSoft.Data
{
    public static partial class DbConnectionExtensions
    {
        /// <summary>
        /// Opens a database connection.
        /// </summary>
        /// <param name="connectionStringSettings">Connection string from the connection strings configuration section.</param>
        /// <returns>An open instance of the connection.</returns>
        public static DbConnection OpenConnection(this ConnectionStringSettings connectionStringSettings)
        {
            var factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionStringSettings.ConnectionString;
            connection.Open();
            return connection;
        }
    }
}
