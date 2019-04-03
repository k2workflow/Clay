#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Data;

namespace SourceCode.Clay.Data.SqlParser
{
    public readonly struct SqlParamInfo : IEquatable<SqlParamInfo>
    {
        private static readonly SqlParamInfo s_empty;

        public static ref readonly SqlParamInfo Empty => ref s_empty;

        public bool IsNullable { get; }

        public bool HasDefault { get; }

        public bool IsReadOnly { get; }

        public ParameterDirection Direction { get; }

        public SqlParamInfo(bool isNullable, bool hasDefault, bool isReadOnly, ParameterDirection direction)
        {
            if (!Enum.IsDefined(typeof(ParameterDirection), direction)) throw new ArgumentOutOfRangeException(nameof(direction));

            IsNullable = isNullable;
            HasDefault = hasDefault;
            IsReadOnly = isReadOnly;
            Direction = direction;
        }

        public bool Equals(SqlParamInfo other)
            => (IsNullable, HasDefault, IsReadOnly, Direction)
            == (other.IsNullable, other.HasDefault, other.IsReadOnly, other.Direction);

        public override bool Equals(object obj)
            => obj is SqlParamInfo prm
            && Equals(prm);

        public override int GetHashCode()
        {
            int hc = 11;

            unchecked
            {
                hc = hc * 7 + (IsNullable ? 3 : 0);
                hc = hc * 7 + (int)Direction;
                hc = hc * 7 + (HasDefault ? 5 : 0);
                hc = hc * 7 + (IsReadOnly ? 7 : 0);
            }

            return hc;
        }

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="SqlParamInfo"/> to compare.</param>
        /// <param name="y">The second <see cref="SqlParamInfo"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SqlParamInfo"/> is equal to <see cref="SqlParamInfo"/>.
        /// </returns>
        public static bool operator ==(SqlParamInfo x, SqlParamInfo y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="SqlParamInfo"/> to compare.</param>
        /// <param name="y">The second <see cref="SqlParamInfo"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SqlParamInfo"/> is not similar to <see cref="SqlParamInfo"/>.
        /// </returns>
        public static bool operator !=(SqlParamInfo x, SqlParamInfo y) => !(x == y);

        public override string ToString()
            => $"{Direction}"
            + (IsNullable ? ", Null" : string.Empty)
            + (HasDefault ? ", Default" : string.Empty)
            + (IsReadOnly ? ", ReadOnly" : string.Empty);
    }
}
