#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace SourceCode.Clay.Data.SqlClient.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SqlConnectionExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_command_from_connection()
        {
            var tsql = "SELECT * FROM [Customer].[Address];";

            using (var sqlCon = new SqlConnection())
            {
                using (SqlCommand sqlCmd = sqlCon.CreateCommand(tsql, CommandType.Text))
                {
                    Assert.Equal(sqlCon, sqlCmd.Connection);
                    Assert.Equal(tsql, sqlCmd.CommandText);
                    Assert.Equal(CommandType.Text, sqlCmd.CommandType);
                }

                using (SqlCommand sqlCmd = sqlCon.CreateCommand(tsql, CommandType.StoredProcedure, 91))
                {
                    Assert.Equal(sqlCon, sqlCmd.Connection);
                    Assert.Equal(tsql, sqlCmd.CommandText);
                    Assert.Equal(91, sqlCmd.CommandTimeout);
                    Assert.Equal(CommandType.StoredProcedure, sqlCmd.CommandType);
                }
            }
        }
    }
}
