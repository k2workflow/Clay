#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data.SqlClient;
using Xunit;

namespace SourceCode.Clay.Data.SqlClient.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SqlConnectionStringBuilderTests
    {
        private static readonly string[] s_serverTokens = { "DATA SOURCE", "data source", "SERVER", "server" };

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_clear_inline_creds))]
        public static void When_clear_inline_creds()
        {
            var sqlCsb = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "AdventureWorks",
                UserID = "admin",
                Password = "innocuous " + "test code"
            };
            sqlCsb = sqlCsb.ClearInlineCredentials();

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.Equal(string.Empty, sqlCsb.UserID);
            Assert.Equal(string.Empty, sqlCsb.Password);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_set_application_name))]
        public static void When_set_application_name()
        {
            var sqlCsb = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "AdventureWorks"
            };
            sqlCsb = sqlCsb.WithApplicationName(null);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.False(sqlCsb.ConnectionString.Contains("Application Name", StringComparison.OrdinalIgnoreCase));

            sqlCsb = sqlCsb.WithApplicationName(string.Empty);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.False(sqlCsb.ConnectionString.Contains("Application Name", StringComparison.OrdinalIgnoreCase));

            sqlCsb = sqlCsb.WithApplicationName(" ");

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.False(sqlCsb.ConnectionString.Contains("Application Name", StringComparison.OrdinalIgnoreCase));

            sqlCsb = sqlCsb.WithApplicationName("a");

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.True(sqlCsb.ApplicationName == "a");

            sqlCsb = sqlCsb.WithApplicationName("b");

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.True(sqlCsb.ApplicationName == "b");

            sqlCsb = sqlCsb.WithApplicationName(null);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.False(sqlCsb.ConnectionString.Contains("Application Name", StringComparison.OrdinalIgnoreCase));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_set_application_intent))]
        public static void When_set_application_intent()
        {
            var sqlCsb = new SqlConnectionStringBuilder
            {
                DataSource = ".",
                InitialCatalog = "AdventureWorks"
            };
            sqlCsb = sqlCsb.WithReadOnlyIntent(null);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.False(sqlCsb.ConnectionString.Contains("ApplicationIntent", StringComparison.OrdinalIgnoreCase));

            sqlCsb = sqlCsb.WithReadOnlyIntent(false);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.True(sqlCsb.ApplicationIntent == ApplicationIntent.ReadWrite);

            sqlCsb = sqlCsb.WithReadOnlyIntent(true);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.True(sqlCsb.ApplicationIntent == ApplicationIntent.ReadOnly);

            sqlCsb = sqlCsb.WithReadOnlyIntent(null);

            Assert.Equal(".", sqlCsb.DataSource);
            Assert.Equal("AdventureWorks", sqlCsb.InitialCatalog);
            Assert.False(sqlCsb.ConnectionString.Contains("ApplicationIntent", StringComparison.OrdinalIgnoreCase));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Local")]
        public static void When_make_robust_local()
        {
            foreach (string svr in s_serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr} = a",
                    $"{svr} = A",
                    $"{svr}=a",
                    $"{svr}=a.",
                    $"{svr}=a.db"
                };

                foreach (string test in tests)
                {
                    var sqlCsb = new SqlConnectionStringBuilder(test);
                    sqlCsb = sqlCsb.MakeRobust(SqlConnectionRetryOptions.Default);

                    Assert.False(sqlCsb.Encrypt);
                    Assert.False(sqlCsb.DataSource.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase));
                    Assert.False(sqlCsb.DataSource.EndsWith(",1433", StringComparison.OrdinalIgnoreCase));
                    Assert.False(sqlCsb.IsAzureSql());
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Negative")]
        public static void When_make_robust_token_negative()
        {
            foreach (string svr in s_serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr}=.database",
                    $"{svr}=database.",
                    $"{svr}=.database.",
                    $"{svr}=a.database",
                    $"{svr}=database.b",
                    $"{svr}=a.database.",
                    $"{svr}=.database.b",
                    $"{svr}=a.database.b"
                };

                foreach (string test in tests)
                {
                    var sqlCsb = new SqlConnectionStringBuilder(test);
                    sqlCsb = sqlCsb.MakeRobust(SqlConnectionRetryOptions.Default);

                    Assert.False(sqlCsb.Encrypt);
                    Assert.False(sqlCsb.DataSource.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase));
                    Assert.False(sqlCsb.DataSource.EndsWith(",1433", StringComparison.OrdinalIgnoreCase));
                    Assert.False(sqlCsb.IsAzureSql());
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Positive")]
        public static void When_make_robust_token_positive()
        {
            foreach (string svr in s_serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr}=a.database.windows.net",
                    $"{svr}=a.database.windows.net,1433",
                    $"{svr}=a.database.windows.net\\foo,1433",
                    $"{svr}=a.database.chinacloudapi.cn",
                    $"{svr}=a.database.usgovcloudapi.net",
                    $"{svr}=a.database.cloudapi.de"
                };

                foreach (string test in tests)
                {
                    var sqlCsb = new SqlConnectionStringBuilder(test);
                    sqlCsb = sqlCsb.MakeRobust(SqlConnectionRetryOptions.Default);

                    Assert.True(sqlCsb.Encrypt);
                    Assert.False(sqlCsb.TrustServerCertificate);
                    Assert.StartsWith("tcp:", sqlCsb.DataSource, StringComparison.OrdinalIgnoreCase);
                    Assert.EndsWith(",1433", sqlCsb.DataSource, StringComparison.OrdinalIgnoreCase);
                    Assert.True(sqlCsb.IsAzureSql());
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Force")]
        public static void When_make_robust_token_force()
        {
            foreach (string svr in s_serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr}=a",
                    $"{svr}=database"
                };

                foreach (string test in tests)
                {
                    var sqlCsb = new SqlConnectionStringBuilder(test + ";encrypt=true");
                    sqlCsb = sqlCsb.MakeRobust(SqlConnectionRetryOptions.Default);

                    Assert.True(sqlCsb.Encrypt);
                }
            }
        }
    }
}
