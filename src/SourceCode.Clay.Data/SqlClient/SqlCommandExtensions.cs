using System;
using System.Data;
using System.Data.SqlClient;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// Represents extensions for <see cref="SqlCommand"/> instances.
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlCommand"/>
    public static class SqlCommandExtensions
    {
#pragma warning disable S3649 // User-provided values should be sanitized before use in SQL statements

        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlCon">The <see cref="SqlConnection"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        /// <param name="timeoutSeconds">The command timeout.</param>
        /// <returns></returns>
        public static SqlCommand CreateCommand(this SqlConnection sqlCon, string commandText, CommandType commandType, int timeoutSeconds)
        {
            if (sqlCon == null) throw new ArgumentNullException(nameof(sqlCon));
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException(nameof(commandText));

            var cmd = new SqlCommand(commandText, sqlCon)
            {
                CommandType = commandType,
                CommandTimeout = timeoutSeconds
            };

            return cmd;
        }

        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlTxn">The <see cref="SqlTransaction"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        /// <param name="timeoutSeconds">The command timeout.</param>
        /// <returns></returns>
        public static SqlCommand CreateCommand(this SqlTransaction sqlTxn, string commandText, CommandType commandType, int timeoutSeconds)
        {
            if (sqlTxn == null) throw new ArgumentNullException(nameof(sqlTxn));
            if (sqlTxn.Connection == null) throw new ArgumentNullException(nameof(sqlTxn));
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException(nameof(commandText));

            var cmd = new SqlCommand(commandText, sqlTxn.Connection)
            {
                CommandType = commandType,
                CommandTimeout = timeoutSeconds,
                Transaction = sqlTxn
            };

            return cmd;
        }

        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlCon">The <see cref="SqlConnection"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        /// <returns></returns>
        public static SqlCommand CreateCommand(this SqlConnection sqlCon, string commandText, CommandType commandType)
        {
            if (sqlCon == null) throw new ArgumentNullException(nameof(sqlCon));
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException(nameof(commandText));

            return sqlCon.CreateCommand(commandText, commandType, 3 * 60);
        }

        /// <summary>
        /// Create a <see cref="SqlCommand"/> using the provided parameters.
        /// </summary>
        /// <param name="sqlCon">The <see cref="SqlTransaction"/> to use.</param>
        /// <param name="commandText">The sql command text to use.</param>
        /// <param name="commandType">The type of command.</param>
        /// <returns></returns>
        public static SqlCommand CreateCommand(this SqlTransaction sqlTxn, string commandText, CommandType commandType)
        {
            if (sqlTxn == null) throw new ArgumentNullException(nameof(sqlTxn));
            if (string.IsNullOrWhiteSpace(commandText)) throw new ArgumentNullException(nameof(commandText));

            return sqlTxn.CreateCommand(commandText, commandType, 3 * 60);
        }

#pragma warning restore S3649 // User-provided values should be sanitized before use in SQL statements
    }
}
