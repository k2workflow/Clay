using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace SourceCode.Clay.Data.SqlClient
{
    public static class SqlCommandExtensions
    {
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SqlCommand extension method")]
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

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SqlCommand extension method")]
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

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SqlCommand extension method")]
        public static SqlCommand CreateCommand(this SqlConnection sqlCon, string commandText, CommandType commandType)
        {
            Contract.Requires(sqlCon != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

            return sqlCon.CreateCommand(commandText, commandType, 3 * 60);
        }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SqlCommand extension method")]
        public static SqlCommand CreateCommand(this SqlTransaction sqlTxn, string commandText, CommandType commandType)
        {
            Contract.Requires(sqlTxn != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));
            Contract.Ensures(Contract.Result<SqlCommand>() != null);

            return sqlTxn.CreateCommand(commandText, commandType, 3 * 60);
        }
    }
}
