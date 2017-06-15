using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace SourceCode.Clay.Data.SqlClient
{
    public static class SqlConnectionStringBuilderExtensions
    {
        public static SqlConnectionStringBuilder MakeRobust(this SqlConnectionStringBuilder sqlCsb, SqlConnectionRetryOptions options)
        {
            Contract.Requires(sqlCsb != null);
            Contract.Ensures(Contract.Result<SqlConnectionStringBuilder>() != null);

            var opt = options ?? SqlConnectionRetryOptions.Default;

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

        public static SqlConnectionStringBuilder ClearInlineCredentials(this SqlConnectionStringBuilder sqlCsb)
        {
            Contract.Requires(sqlCsb != null);
            Contract.Ensures(Contract.Result<SqlConnectionStringBuilder>() != null);

            sqlCsb.UserID = string.Empty;
            sqlCsb.Password = string.Empty;

            return sqlCsb;
        }
    }
}
