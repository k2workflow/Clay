#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Xunit;

namespace SourceCode.Clay.Data.SqlClient.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SqlTransactionExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_command_from_transaction()
        {
            var tsql = "SELECT * FROM [Customer].[Address];";

            using (var sqlCon = new SqlConnection())
            {
                // SqlTransactions can only be created on SqlConnections that are open.
                // So use reflection to construct fudge the SqlTransaction.
                System.Type txnType = typeof(SqlTransaction);
                ConstructorInfo txnCtor = txnType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First(n => n.GetParameters().Length == 4);

                using (var sqlTxn = (SqlTransaction)txnCtor.Invoke(new object[] { null, sqlCon, IsolationLevel.ReadCommitted, null }))
                {
                    using (SqlCommand sqlCmd = sqlTxn.CreateCommand(tsql, CommandType.Text))
                    {
                        Assert.Equal(sqlCon, sqlCmd.Connection); // Side effect of test harness
                        Assert.Equal(sqlTxn, sqlCmd.Transaction);
                        Assert.Equal(tsql, sqlCmd.CommandText);
                        Assert.Equal(CommandType.Text, sqlCmd.CommandType);
                    }

                    using (SqlCommand sqlCmd = sqlTxn.CreateCommand(tsql, CommandType.StoredProcedure, 91))
                    {
                        Assert.Equal(sqlCon, sqlCmd.Connection);
                        Assert.Equal(sqlTxn, sqlCmd.Transaction);
                        Assert.Equal(tsql, sqlCmd.CommandText);
                        Assert.Equal(91, sqlCmd.CommandTimeout);
                        Assert.Equal(CommandType.StoredProcedure, sqlCmd.CommandType);
                    }
                }
            }
        }
    }
}
