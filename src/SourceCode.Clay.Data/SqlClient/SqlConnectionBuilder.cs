using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Security;

namespace SourceCode.Clay.Data.SqlClient
{
    public sealed class SqlConnectionBuilder
    {
        #region Fields

        private readonly SqlConnectionStringBuilder _sqlCsb;
        private readonly SqlCredential _sqlCredential;

        #endregion

        #region Properties

        [Pure]
        public string DatabaseName => _sqlCsb.InitialCatalog;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SqlConnectionBuilder"/>.
        /// Extracts and clears credentials from the <paramref name="connectionString"/>, creating a <see cref="SqlCredential"/> instead.
        /// Detects whether the referenced <see cref="SqlConnectionStringBuilder.DataSource"/> is a SqlAzure instance, and
        /// if so, injects robustness tokens such as <see cref="SqlConnectionStringBuilder.ConnectTimeout"/>.
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlConnectionBuilder(string connectionString)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionString));

            var csb = new SqlConnectionStringBuilder(connectionString);

            // Derive SqlCredential
            var ss = ToSecureString(csb.Password);
            _sqlCredential = new SqlCredential(csb.UserID, ss);

            _sqlCsb = csb
                .ClearInlineCredentials()
                .MakeRobust(SqlConnectionRetryOptions.Default);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlConnectionBuilder"/>.
        /// Clears any credentials from the <paramref name="connectionString"/>, using the <paramref name="sqlCredential"/> value instead.
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlConnectionBuilder(string connectionString, SqlCredential sqlCredential)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionString));
            Contract.Requires(sqlCredential != null);

            var csb = new SqlConnectionStringBuilder(connectionString);

            _sqlCredential = sqlCredential;

            _sqlCsb = csb
                .ClearInlineCredentials()
                .MakeRobust(SqlConnectionRetryOptions.Default);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlConnectionBuilder"/>.
        /// Clears any credentials from the <paramref name="builder"/>, using the <paramref name="sqlCredential"/> value instead.
        /// </summary>
        /// <param name="builder"></param>
        public SqlConnectionBuilder(SqlConnectionStringBuilder builder, SqlCredential sqlCredential)
        {
            Contract.Requires(builder != null);
            Contract.Requires(sqlCredential != null);

            // Clone the CSB so that we do not mutate the original
            var csb = new SqlConnectionStringBuilder(builder.ConnectionString);

            _sqlCredential = sqlCredential;

            _sqlCsb = csb
                .ClearInlineCredentials()
                .MakeRobust(SqlConnectionRetryOptions.Default);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlConnectionBuilder"/>.
        /// Clears any credentials from the <paramref name="builder"/>, using the supplied credentials to create a <see cref="SqlCredential"/> instead.
        /// </summary>
        /// <param name="builder"></param>
        public SqlConnectionBuilder(SqlConnectionStringBuilder builder, string username, string password)
        {
            Contract.Requires(builder != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(username));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));

            // Clone the CSB so that we do not mutate the original
            var csb = new SqlConnectionStringBuilder(builder.ConnectionString);

            // Create SqlCredential
            var ss = ToSecureString(password);
            _sqlCredential = new SqlCredential(username, ss);

            _sqlCsb = csb
                .ClearInlineCredentials()
                .MakeRobust(SqlConnectionRetryOptions.Default);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SqlConnectionBuilder"/>.
        /// Extracts and clears credentials from the <paramref name="builder"/>, creating a <see cref="SqlCredential"/> instead.
        /// Detects whether the referenced <see cref="SqlConnectionStringBuilder.DataSource"/> is a SqlAzure instance, and
        /// if so, injects robustness tokens such as <see cref="SqlConnectionStringBuilder.ConnectTimeout"/>.
        /// </summary>
        /// <param name="builder"></param>
        public SqlConnectionBuilder(SqlConnectionStringBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(builder.UserID));
            Contract.Requires(!string.IsNullOrWhiteSpace(builder.Password));

            // Clone the CSB so that we do not mutate the original
            var csb = new SqlConnectionStringBuilder(builder.ConnectionString);

            // Derive SqlCredential
            var ss = ToSecureString(csb.Password);
            _sqlCredential = new SqlCredential(csb.UserID, ss);

            _sqlCsb = csb
                .ClearInlineCredentials()
                .MakeRobust(SqlConnectionRetryOptions.Default);
        }

        #endregion

        #region Fluent

        public SqlConnectionBuilder WithDatabase(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Ensures(Contract.Result<SqlConnectionBuilder>() != null);

            _sqlCsb.InitialCatalog = name;
            return this;
        }

        public SqlConnectionBuilder WithApplication(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Ensures(Contract.Result<SqlConnectionBuilder>() != null);

            _sqlCsb.ApplicationName = name;
            return this;
        }

        //[Pure]
        //public SqlConnection CreateSqlConnection()
        //{
        //    Contract.Ensures(Contract.Result<SqlConnection>() != null);
        //    Contract.Ensures(Contract.Result<SqlConnection>() != null);

        //    if (_sqlCsb.IntegratedSecurity)
        //        return new SqlConnection(_sqlCsb.ConnectionString);

        //    var sqlCon = new SqlConnection(_sqlCsb.ConnectionString, _sqlCredential);
        //    return sqlCon;
        //}

        //[Pure]
        //public SqlConnection CreateSqlConnection(SqlCredential credential)
        //{
        //    Contract.Requires(credential != null);
        //    Contract.Ensures(Contract.Result<SqlConnection>() != null);

        //    var sqlCon = new SqlConnection(_sqlCsb.ConnectionString, credential);
        //    return sqlCon;
        //}

        /// <summary>
        /// Returns the SqlConnectionString without any User credentials
        /// </summary>
        /// <returns></returns>
        [Pure]
        public override string ToString() => _sqlCsb.ConnectionString;

        #endregion

        #region Helpers

        // Reimplement method from Clay.Security (avoids incurring a Nuget dependency)
        private static SecureString ToSecureString(string secret)
        {
            if (secret == null) return null;

            var ss = new SecureString();
            for (var i = 0; i < secret.Length; i++)
            {
                ss.AppendChar(secret[i]);
            }
            ss.MakeReadOnly();

            return ss;
        }

        #endregion
    }
}
