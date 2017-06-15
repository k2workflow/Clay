using System;
using System.Security;

namespace SourceCode.Clay.Data.SqlClient
{
    public sealed class SqlCredential
    {
        public string UserId { get; }

        public SecureString Password { get; }

        public SqlCredential(string userId, SecureString password)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (userId.Length > 128) throw new ArgumentOutOfRangeException(nameof(userId));

            if (password == null) throw new ArgumentNullException(nameof(password));
            if (password.Length > 128) throw new ArgumentOutOfRangeException(nameof(password));
            if (password.Length > 128) throw new ArgumentException("Password must be readonly", nameof(password));

            UserId = userId;
            Password = password;
        }
    }
}
