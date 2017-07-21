using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// Represents extensions for <see cref="SqlConnection"/> instances.
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlConnection"/>
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Reopens the specified <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="sqlCon">The connection.</param>
        public static void Reopen(this SqlConnection sqlCon)
        {
            Contract.Requires(sqlCon != null);

            switch (sqlCon.State)
            {
                case ConnectionState.Broken:
                    {
                        sqlCon.Close();
                        sqlCon.Open();
                        return;
                    }

                case ConnectionState.Closed:
                    {
                        sqlCon.Open();
                        return;
                    }

                default: return;
            }
        }

        /// <summary>
        /// Reopens the specified <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="sqlCon">The connection.</param>
        public static async Task ReopenAsync(this SqlConnection sqlCon)
        {
            Contract.Requires(sqlCon != null);

            switch (sqlCon.State)
            {
                case ConnectionState.Broken:
                    {
                        sqlCon.Close();
                        await sqlCon.OpenAsync().ConfigureAwait(false);
                        return;
                    }

                case ConnectionState.Closed:
                    {
                        await sqlCon.OpenAsync().ConfigureAwait(false);
                        return;
                    }

                default:
                    return;
            }
        }

        /// <summary>
        /// Opens the specified <see cref="SqlConnection"/> using impersonation.
        /// </summary>
        /// <param name="sqlCon">The SqlConnection to use.</param>
        /// <param name="impersonatedUsername">The username to be impersonated.</param>
        /// <returns>Cookie value that will be required to close the connection</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Static query with no user inputs")]
        public static byte[] Open(this SqlConnection sqlCon, string impersonatedUsername)
        {
            Contract.Requires(sqlCon != null);

            // Open the underlying connection
            sqlCon.Reopen();

            // If username is null, empty or whitespace-only then don't try impersonate
            if (string.IsNullOrEmpty(impersonatedUsername))
                return null;

            // Set impersonation context using EXECUTE AS
            try
            {
                var user = impersonatedUsername;

                // We need to properly-quote the username in order to avoid injection attacks
                const string sql = "SELECT QUOTENAME(@username, N'''') AS [username];";
                using (var cmd = sqlCon.CreateCommand(sql, CommandType.Text))
                {
                    cmd.Parameters.AddWithValue("username", user);

                    var o = cmd.ExecuteScalar();

                    // Check that the result is non-empty
                    if (o == null)
                        throw new ArgumentNullException(nameof(impersonatedUsername));

                    user = o.ToString();
                    if (string.IsNullOrEmpty(user))
                        throw new ArgumentNullException(nameof(impersonatedUsername));
                }

                // If we successfully quoted the username, then execute the impersonation switch
                // Remember to use the cookie option so we can deterministically undo the impersonation
                // and put the connection back in the connection pool when we are done with it
                var sql1 = $@"
                    DECLARE @cookie VARBINARY(100);
                    EXECUTE AS LOGIN = {user} WITH COOKIE INTO @cookie;
                    SELECT @cookie;";
                using (var cmd = sqlCon.CreateCommand(sql1, CommandType.Text))
                {
                    // Do not use ExecuteNonQuery(), it doesn't like the COOKIE option
                    var oc = cmd.ExecuteScalar();

                    var cookie = (byte[])oc;
                    return cookie;
                }
            }
            catch
            {
                sqlCon.Close();
                throw;
            }
        }

        /// <summary>
        /// Opens the specified <see cref="SqlConnection"/> using impersonation.
        /// </summary>
        /// <param name="sqlCon">The SqlConnection to use.</param>
        /// <param name="impersonatedUsername">The username to be impersonated.</param>
        /// <returns>Cookie value that will be required to close the connection</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Static query with no user inputs")]
        public static async Task<byte[]> OpenAsync(this SqlConnection sqlCon, string impersonatedUsername)
        {
            Contract.Requires(sqlCon != null);

            // Open the underlying connection
            await sqlCon.ReopenAsync().ConfigureAwait(false);

            // If username is null, empty or whitespace-only then don't try impersonate
            if (string.IsNullOrEmpty(impersonatedUsername))
                return null;

            // Set impersonation context using EXECUTE AS
            try
            {
                var user = impersonatedUsername;

                // We need to properly-quote the username in order to avoid injection attacks
                const string sql = "SELECT QUOTENAME(@username, N'''') AS [username];";
                using (var cmd = sqlCon.CreateCommand(sql, CommandType.Text))
                {
                    cmd.Parameters.AddWithValue("username", user);

                    var o = await cmd.ExecuteScalarAsync().ConfigureAwait(false);

                    // Check that the result is non-empty
                    if (o == null)
                        throw new ArgumentNullException(nameof(impersonatedUsername));

                    user = o.ToString();
                    if (string.IsNullOrEmpty(user))
                        throw new ArgumentNullException(nameof(impersonatedUsername));
                }

                // If we successfully quoted the username, then execute the impersonation switch
                // Remember to use the cookie option so we can deterministically undo the impersonation
                // and put the connection back in the connection pool when we are done with it
                var sql1 = $@"
                    DECLARE @cookie VARBINARY(100);
                    EXECUTE AS LOGIN = {user} WITH COOKIE INTO @cookie;
                    SELECT @cookie;";
                using (var cmd = sqlCon.CreateCommand(sql1, CommandType.Text))
                {
                    // Do not use ExecuteNonQuery(), it doesn't like the COOKIE option
                    var oc = await cmd.ExecuteScalarAsync().ConfigureAwait(false);

                    var cookie = (byte[])oc;
                    return cookie;
                }
            }
            catch
            {
                sqlCon.Close();
                throw;
            }
        }

        /// <summary>
        /// Close the specified <see cref="SqlConnection"/> and revert any impersonation.
        /// </summary>
        /// <param name="sqlCon">The SqlConnection to use.</param>
        /// <param name="cookie">The impersonation cookie returned from the Open() method</param>
        public static void Close(this SqlConnection sqlCon, byte[] cookie)
        {
            Contract.Requires(sqlCon != null);

            // Check that the underlying connection is still open
            if (sqlCon.State == ConnectionState.Open)
            {
                try
                {
                    // Only revert the cookie if it is provided
                    if (cookie != null && cookie.Length > 0) // @COOKIE is VARBINARY(100)
                    {
                        const string sql = "REVERT WITH COOKIE = @cookie;";
                        using (var cmd = sqlCon.CreateCommand(sql, CommandType.Text))
                        {
                            var p = cmd.Parameters.Add("cookie", SqlDbType.VarBinary, 100);
                            p.Value = cookie;

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    sqlCon.Close();
                }
            }
        }

        /// <summary>
        /// Close the specified <see cref="SqlConnection"/> and revert any impersonation.
        /// </summary>
        /// </summary>
        /// <param name="sqlCon">The SqlConnection to use.</param>
        /// <param name="cookie">The impersonation cookie returned from the Open() method</param>
        public static async Task CloseAsync(this SqlConnection sqlCon, byte[] cookie)
        {
            Contract.Requires(sqlCon != null);

            // Check that the underlying connection is still open
            if (sqlCon.State == ConnectionState.Open)
            {
                try
                {
                    // Only revert the cookie if it is provided
                    if (cookie != null && cookie.Length > 0)
                    {
                        const string sql = "REVERT WITH COOKIE = @cookie;";
                        using (var cmd = sqlCon.CreateCommand(sql, CommandType.Text))
                        {
                            var p = cmd.Parameters.Add("cookie", SqlDbType.VarBinary, 100); // @COOKIE is VARBINARY(100)
                            p.Value = cookie;

                            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                        }
                    }
                }
                finally
                {
                    sqlCon.Close();
                }
            }
        }
    }
}
