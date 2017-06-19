using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// Represents extensions for <see cref="SqlConnectionStringBuilder"/> instances.
    /// See <see cref="https://msdn.microsoft.com/en-us/library/ms171868(v=vs.110).aspx#v462"/>
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlConnectionStringBuilder"/>
    public static class SqlConnectionStringBuilderExtensions
    {
        /// <summary>
        /// Clear any inline credentials store in the builder. This is useful for logging.
        /// </summary>
        /// <param name="sqlCsb"></param>
        /// <returns></returns>
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
