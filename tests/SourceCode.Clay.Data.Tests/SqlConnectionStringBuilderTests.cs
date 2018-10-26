#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data.SqlClient;
using Xunit;

namespace SourceCode.Clay.Data.SqlClient.Tests
{
    public static class SqlConnectionStringBuilderTests
    {
        private const string dbToken = "database";
        private static readonly string[] serverTokens = { "DATA SOURCE", "data source", "SERVER", "server" };

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
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Local")]
        public static void When_make_robust_local()
        {
            foreach (string svr in serverTokens)
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
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Negative")]
        public static void When_make_robust_token_negative()
        {
            foreach (string svr in serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr}=.{dbToken}",
                    $"{svr}={dbToken}.",
                    $"{svr}=.{dbToken}.",
                    $"{svr}=a.{dbToken}",
                    $"{svr}={dbToken}.b",
                    $"{svr}=a.{dbToken}.",
                    $"{svr}=.{dbToken}.b",
                    $"{svr}=a.{dbToken}.b",
                    $"{svr}=.{dbToken}.windows.net",
                    $"{svr}=.{dbToken}.secure.windows.net",
                    $"{svr}=.{dbToken}.chinacloudapi.cn",
                    $"{svr}=.{dbToken}.usgovcloudapi.net",
                    $"{svr}=.{dbToken}.cloudapi.de"
                };

                foreach (string test in tests)
                {
                    var sqlCsb = new SqlConnectionStringBuilder(test);
                    sqlCsb = sqlCsb.MakeRobust(SqlConnectionRetryOptions.Default);

                    Assert.False(sqlCsb.Encrypt);
                    Assert.False(sqlCsb.DataSource.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase));
                    Assert.False(sqlCsb.DataSource.EndsWith(",1433", StringComparison.OrdinalIgnoreCase));
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Positive")]
        public static void When_make_robust_token_positive()
        {
            foreach (string svr in serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr}=a.{dbToken}.windows.net",
                    $"{svr}=a.{dbToken}.secure.windows.net",
                    $"{svr}=a.{dbToken}.chinacloudapi.cn",
                    $"{svr}=a.{dbToken}.usgovcloudapi.net",
                    $"{svr}=a.{dbToken}.cloudapi.de"
                };

                foreach (string test in tests)
                {
                    var sqlCsb = new SqlConnectionStringBuilder(test);
                    sqlCsb = sqlCsb.MakeRobust(SqlConnectionRetryOptions.Default);

                    Assert.True(sqlCsb.Encrypt);
                    Assert.StartsWith("tcp:", sqlCsb.DataSource, StringComparison.OrdinalIgnoreCase);
                    Assert.EndsWith(",1433", sqlCsb.DataSource, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "SqlConnectionStringBuilderExtensions MakeRobust Force")]
        public static void When_make_robust_token_force()
        {
            foreach (string svr in serverTokens)
            {
                string[] tests = new[]
                {
                    $"{svr}=a",
                    $"{svr}={dbToken}"
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
