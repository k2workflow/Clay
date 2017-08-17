using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

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
            Contract.Requires(sqlCon != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

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
            Contract.Requires(sqlTxn != null);
            Contract.Requires(sqlTxn.Connection != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

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
            Contract.Requires(sqlCon != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

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
            Contract.Requires(sqlTxn != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

            return sqlTxn.CreateCommand(commandText, commandType, 3 * 60);
        }

#pragma warning restore S3649 // User-provided values should be sanitized before use in SQL statements
    }
}
