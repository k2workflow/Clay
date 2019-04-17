#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data.SqlClient;

namespace SourceCode.Clay.Data.SqlClient // .Azure
{
    /// <summary>
    /// Represents extensions for <see cref="SqlConnectionStringBuilder"/> instances.
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlConnectionStringBuilder"/>
    public static class SqlConnectionStringBuilderExtensions
    {
        /// <summary>
        /// Clear any inline credentials store in the builder.
        /// Useful for eliding sensitive data when logging.
        /// </summary>
        /// <param name="sqlCsb">The sql connection string builder instance.</param>
        public static SqlConnectionStringBuilder ClearInlineCredentials(this SqlConnectionStringBuilder sqlCsb)
        {
            if (sqlCsb is null) throw new ArgumentNullException(nameof(sqlCsb));

            sqlCsb.UserID = string.Empty;
            sqlCsb.Password = string.Empty;

            return sqlCsb;
        }

        /// <summary>
        /// Set the application intent to ReadOnly, ReadWrite or null.
        /// </summary>
        /// <param name="sqlCsb">The builder instance.</param>
        /// <param name="readOnly">If true, sets the intent to ReadOnly, else sets the intent to ReadWrite.
        /// Use null to remove the token from the builder entirely.</param>
        public static SqlConnectionStringBuilder WithReadOnlyIntent(this SqlConnectionStringBuilder sqlCsb, bool? readOnly)
        {
            if (sqlCsb is null) throw new ArgumentNullException(nameof(sqlCsb));

            if (readOnly.HasValue)
                sqlCsb.ApplicationIntent = readOnly.Value ? ApplicationIntent.ReadOnly : ApplicationIntent.ReadWrite;
            else
                sqlCsb.Remove("ApplicationIntent");

            return sqlCsb;
        }

        /// <summary>
        /// Sets the application name.
        /// </summary>
        /// <param name="sqlCsb">The sql connection string builder instance.</param>
        /// <param name="name">The value to set. An empty or null value removes the token from the builder entirely.</param>
        public static SqlConnectionStringBuilder WithApplicationName(this SqlConnectionStringBuilder sqlCsb, string name)
        {
            if (sqlCsb is null) throw new ArgumentNullException(nameof(sqlCsb));

            if (string.IsNullOrWhiteSpace(name))
                sqlCsb.Remove("Application Name");
            else
                sqlCsb.ApplicationName = name;

            return sqlCsb;
        }

        /// <summary>
        /// Add retry and timeout settings according to AzureDb best practices.
        /// </summary>
        /// <param name="sqlCsb">The sql connection string builder instance.</param>
        /// <param name="options">The options to set.</param>
        public static SqlConnectionStringBuilder MakeRobust(this SqlConnectionStringBuilder sqlCsb, SqlConnectionRetryOptions options)
        {
            if (sqlCsb is null) throw new ArgumentNullException(nameof(sqlCsb));

            SqlConnectionRetryOptions opt = options ?? SqlConnectionRetryOptions.Default;

            // Merge AzureDb-specific tokens
            if (sqlCsb.IsAzureSql())
            {
                // OK, it seems this is an AzureDb connection string
                // Per MSFT security guidelines, set the following values
                sqlCsb.TrustServerCertificate = false;
                sqlCsb.Encrypt = true;

                // AzureDb only supports TCP:1433, though SqlClient wastes time trying to negotiate named pipes, etc
                if (!sqlCsb.DataSource.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
                    sqlCsb.DataSource = $"tcp:{sqlCsb.DataSource}";

#if NETSTANDARD2_0
                if (!sqlCsb.DataSource.Contains(","))
                    sqlCsb.DataSource = $"{sqlCsb.DataSource},1433";
#else
                if (!sqlCsb.DataSource.Contains(",", StringComparison.Ordinal))
                    sqlCsb.DataSource = $"{sqlCsb.DataSource},1433";
#endif
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

        /// <summary>
        /// Returns true if the specified DataSource is on AzureDb, else returns false.
        /// </summary>
        /// <param name="sqlCsb">The sql connection string builder instance.</param>
        public static bool IsAzureSql(this SqlConnectionStringBuilder sqlCsb)
            => sqlCsb == null ? false : IsAzureSql(sqlCsb.DataSource);

        // See SqlClient section here: https://msdn.microsoft.com/en-us/library/ms171868(v=vs.110).aspx#v462
        // Original code here:
        // https://github.com/dotnet/corefx/blob/master/src/System.Data.SqlClient/src/System/Data/Common/AdapterUtil.SqlClient.cs#L748-L752

        private static readonly string[] s_azureSqlServerEndpoints =
        {
            ".database.windows.net",
            ".database.cloudapi.de",
            ".database.usgovcloudapi.net",
            ".database.chinacloudapi.cn"
        };

        internal static bool IsAzureSql(string datasource)
        {
            if (string.IsNullOrWhiteSpace(datasource)) return false;

            // TODO: No need to mutate, just search
            string ds = datasource;

            // Remove server port
            int i = ds.LastIndexOf(',');
            if (i >= 0)
            {
                ds = ds.Substring(0, i);
            }

            // Check for the instance name
            i = ds.LastIndexOf('\\');
            if (i >= 0)
            {
                ds = ds.Substring(0, i);
            }

            // Trim redundant whitespace
            ds = ds.Trim();

            // Check if servername ends with any Azure endpoints
            for (i = 0; i < s_azureSqlServerEndpoints.Length; i++)
            {
                if (ds.EndsWith(s_azureSqlServerEndpoints[i], StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
