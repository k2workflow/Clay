#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data;
using System.Data.SqlClient;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// Represents extensions for <see cref="SqlTransaction"/> instances.
    /// </summary>
    /// <seealso cref="SqlTransaction"/>
    public static class SqlTransactionExtensions
    {
        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlTxn">The <see cref="SqlTransaction"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        public static SqlCommand CreateCommand(this SqlTransaction sqlTxn, string commandText, CommandType commandType)
        {
            if (sqlTxn is null) throw new ArgumentNullException(nameof(sqlTxn));
            if (sqlTxn.Connection is null) throw new ArgumentNullException(nameof(sqlTxn));
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException(nameof(commandText));

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            var cmd = new SqlCommand(commandText, sqlTxn.Connection)
            {
                CommandType = commandType,
                Transaction = sqlTxn
            };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

            return cmd;
        }

        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlTxn">The <see cref="SqlTransaction"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        /// <param name="timeoutSeconds">The command timeout.</param>
        public static SqlCommand CreateCommand(this SqlTransaction sqlTxn, string commandText, CommandType commandType, int timeoutSeconds)
        {
            if (timeoutSeconds < 0) throw new ArgumentOutOfRangeException(nameof(timeoutSeconds)); // 0 means infinite timeout in SqlCommand

            SqlCommand cmd = CreateCommand(sqlTxn, commandText, commandType);
            cmd.CommandTimeout = timeoutSeconds;

            return cmd;
        }

        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlTxn">The <see cref="SqlTransaction"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        /// <param name="timeout">The command timeout.</param>
        public static SqlCommand CreateCommand(this SqlTransaction sqlTxn, string commandText, CommandType commandType, TimeSpan timeout)
            => CreateCommand(sqlTxn, commandText, commandType, (int)timeout.TotalSeconds);
    }
}
