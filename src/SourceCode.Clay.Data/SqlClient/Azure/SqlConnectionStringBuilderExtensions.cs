#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data.SqlClient;

namespace SourceCode.Clay.Data.SqlClient.Azure
{
    /// <summary>
    /// Represents extensions for <see cref="SqlConnectionStringBuilder"/> instances.
    /// See <see cref="https://msdn.microsoft.com/en-us/library/ms171868(v=vs.110).aspx#v462"/>
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlConnectionStringBuilder"/>
    public static class SqlConnectionStringBuilderAzureExtensions
    {
        /// <summary>
        /// Add retry and timeout settings according to AzureDb best practices.
        /// </summary>
        /// <param name="sqlCsb">The builder instance.</param>
        /// <param name="options">The options to set.</param>
        /// <returns></returns>
        public static SqlConnectionStringBuilder MakeRobust(this SqlConnectionStringBuilder sqlCsb, SqlConnectionRetryOptions options)
        {
            if (sqlCsb is null) throw new ArgumentNullException(nameof(sqlCsb));

            SqlConnectionRetryOptions opt = options ?? SqlConnectionRetryOptions.Default;

            // Add AzureDb-specific tokens
            if (!string.IsNullOrWhiteSpace(sqlCsb.DataSource))
            {
                // Datasource                   len p1  p2  p1>=1   p2<len  ok
                // .database.b                  11  0   10  F       T       F
                // a.database.                  11  1   11  T       F       F
                // a.database.b                 12  1   11  T       T       T
                // a.database.windows.net       22  1   11  T       T       T
                // abcd.database.windows.net    25  4   14  T       T       T

                const string token = ".database."; // len=10
                var p1 = sqlCsb.DataSource.IndexOf(token, StringComparison.OrdinalIgnoreCase);
                var p2 = p1 + token.Length;

                if (p1 >= 1 && p2 < sqlCsb.DataSource.Length)
                {
                    var suffix = sqlCsb.DataSource.Substring(p2);

                    // See SqlClient section here: https://msdn.microsoft.com/en-us/library/ms171868(v=vs.110).aspx#v462
                    if (suffix.StartsWith("windows.net", StringComparison.OrdinalIgnoreCase) ||
                        suffix.StartsWith("secure.windows.net", StringComparison.OrdinalIgnoreCase) ||
                        suffix.StartsWith("chinacloudapi.cn", StringComparison.OrdinalIgnoreCase) ||
                        suffix.StartsWith("usgovcloudapi.net", StringComparison.OrdinalIgnoreCase) ||
                        suffix.StartsWith("cloudapi.de", StringComparison.OrdinalIgnoreCase))
                    {
                        // OK, it seems this is an AzureDb connection string
                        // Per MSFT security guidelines, set the following values
                        sqlCsb.TrustServerCertificate = false;
                        sqlCsb.Encrypt = true;

                        // AzureDb only supports TCP:1433, though SqlClient wastes time trying to negotiate named pipes, etc
                        if (!sqlCsb.DataSource.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
                            sqlCsb.DataSource = $"tcp:{sqlCsb.DataSource},1433";
                    }
                }
            }

            // Add connectivity robustness
            if (sqlCsb.ConnectRetryCount < opt.ConnectRetryCount)
                sqlCsb.ConnectRetryCount = opt.ConnectRetryCount;

            if (sqlCsb.ConnectRetryInterval < opt.ConnectRetryInterval)
                sqlCsb.ConnectRetryInterval = opt.ConnectRetryInterval;

            if (sqlCsb.ConnectTimeout < opt.ConnectTimeout)
                sqlCsb.ConnectTimeout = opt.ConnectTimeout;

            return sqlCsb;
        }
    }
}
