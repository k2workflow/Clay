#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data.SqlClient;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// Represents extensions for <see cref="SqlConnectionStringBuilder"/> instances.
    /// See <see cref="https://msdn.microsoft.com/en-us/library/ms171868(v=vs.110).aspx#v462"/>
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlConnectionStringBuilder"/>
    public static class SqlConnectionStringBuilderExtensions
    {
        /// <summary>
        /// Clear any inline credentials store in the builder. This is useful for logging.
        /// </summary>
        /// <param name="sqlCsb"></param>
        /// <returns></returns>
        public static SqlConnectionStringBuilder ClearInlineCredentials(this SqlConnectionStringBuilder sqlCsb)
        {
            if (sqlCsb == null) throw new ArgumentNullException(nameof(sqlCsb));

            sqlCsb.UserID = string.Empty;
            sqlCsb.Password = string.Empty;

            return sqlCsb;
        }
    }
}
