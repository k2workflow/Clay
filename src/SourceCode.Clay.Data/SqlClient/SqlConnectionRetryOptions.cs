#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// A property bag to hold various connection retry options.
    /// </summary>
    public sealed class SqlConnectionRetryOptions
    {
        public static SqlConnectionRetryOptions Default { get; } = new SqlConnectionRetryOptions();

        private byte? _connectRetryCount;

        private byte? _connectRetryInterval;

        private ushort? _connectTimeout;

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

        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an
        /// idle connection failure. This must be an integer between 1 and 60. The default is 10 seconds.
        /// </summary>
        public byte ConnectRetryInterval
        {
            get => _connectRetryInterval.GetValueOrDefault(10);
            set => _connectRetryInterval = value;
        }

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the
        /// server before terminating the attempt and generating an error. The default is 30 seconds.
        /// </summary>
        public ushort ConnectTimeout
        {
            get => _connectTimeout.GetValueOrDefault(30);
            set => _connectTimeout = value;
        }

        public SqlConnectionRetryOptions()
        { }
    }
}
