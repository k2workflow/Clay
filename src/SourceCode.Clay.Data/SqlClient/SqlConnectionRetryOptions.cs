using System.Diagnostics.Contracts;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// A property bag to hold various connection retry options.
    /// </summary>
    public sealed class SqlConnectionRetryOptions
    {
        #region Constants

        public static SqlConnectionRetryOptions Default { [Pure]get; } = new SqlConnectionRetryOptions();

        #endregion

        #region Properties

        private byte? _connectRetryCount;

        /// <summary>
        /// The number of reconnections attempted after identifying that there was an
        /// idle connection failure. This must be an integer between 0 and 255. Default is 5.
        /// Set to 0 to disable reconnecting on idle connection failures.
        /// </summary>
        public byte ConnectRetryCount
        {
            get => _connectRetryCount.GetValueOrDefault(5);
            set => _connectRetryCount = value;
        }

        private byte? _connectRetryInterval;

        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an
        /// idle connection failure. This must be an integer between 1 and 60. The default is 10 seconds.
        /// </summary>
        public byte ConnectRetryInterval
        {
            get => _connectRetryInterval.GetValueOrDefault(10);
            set => _connectRetryInterval = value;
        }

        private ushort? _connectTimeout;

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the
        /// server before terminating the attempt and generating an error. The default is 30 seconds.
        /// </summary>
        public ushort ConnectTimeout
        {
            get => _connectTimeout.GetValueOrDefault(30);
            set => _connectTimeout = value;
        }

        #endregion

        #region Constructors

        public SqlConnectionRetryOptions(byte retryCount, byte intervalSeconds, ushort timeoutSeconds)
        {
            Contract.Requires(intervalSeconds >= 1 && intervalSeconds <= 60);

            ConnectRetryCount = retryCount;
            ConnectRetryInterval = intervalSeconds;
            ConnectTimeout = timeoutSeconds;
        }

        public SqlConnectionRetryOptions()
        { }

        #endregion
    }
}
