using System;
using System.Diagnostics;
using System.Globalization;

namespace SourceCode.Clay.Distributed
{
    /// <summary>
    /// Represents a distributed identifier.
    /// </summary>
    [DebuggerDisplay("{Value,nq}")]
    public readonly struct DistributedId : IEquatable<DistributedId>, IComparable<DistributedId>
    {
        /// <summary>
        /// Represents the smallest possible value of <see cref="DistributedId"/>. This field is constant.
        /// </summary>
        public static DistributedId MinValue { get; } = new DistributedId(ulong.MinValue);

        /// <summary>
        /// Represents the largest possible value of <see cref="DistributedId"/>. This field is constant.
        /// </summary>
        public static DistributedId MaxValue { get; } = new DistributedId(ulong.MaxValue);

        /// <summary>
        /// The value of the distributed identifier.
        /// </summary>
        public ulong Value { get; }

        /// <summary>
        /// Creates a new distributed identifier.
        /// </summary>
        /// <param name="value"></param>
        public DistributedId(ulong value) => Value = value;

        /// <inheritdoc />
        public override string ToString() => Value.ToString("x", CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts the string representation of a distributed identifier to its <see cref="DistributedId"/> equivalent.
        /// </summary>
        /// <param name="value">A <see cref="string"/> that represents the distributed identifier to convert.</param>
        /// <returns>A <see cref="DistributedId"/> equivalent to the distributed identifier contained in <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
        /// <exception cref="FormatException">The <paramref name="value"/> is not in the correct format.</exception>
        /// <exception cref="OverflowException">The <paramref name="value"/> represents a value less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.</exception>
        public static DistributedId Parse(string value) => new DistributedId(ulong.Parse(value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture));

        /// <summary>
        /// Tries to convert the string representation of a distributed identifier to its <see cref="DistributedId"/>
        /// equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">A <see cref="string"/> that represents the number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <see cref="Distributed"/> that is equivalent to the number contained
        /// in <paramref name="value"/>, if the conversion succeeded, or <see cref="MinValue"/> if the conversion failed.
        /// The conversion fails if the <paramref name="value"/> parameter is null or <see cref="string.Empty"/>, is not of
        /// the correct format or represents a value less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        /// This parameter is passed unitialized; any value originally supplied in <paramref name="result"/> will be overwritten.
        /// </param>
        /// <returns>true if <paramref name="value"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string value, out DistributedId result)
        {
            if (ulong.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out ulong ulongResult))
            {
                result = new DistributedId(ulongResult);
                return true;
            }
            result = default;
            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DistributedId id && Equals(id);
        /// <inheritdoc />
        public bool Equals(DistributedId other) => Value == other.Value;
        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Determines if one <see cref="DistributedId"/> is equal to another.
        /// </summary>
        /// <param name="left">The first <see cref="DistributedId"/> to compare.</param>
        /// <param name="right">The second <see cref="DistributedId"/> to compare.</param>
        /// <returns>A value indicating whether the <see cref="DistributedId"/> values are equal.</returns>
        public static bool operator ==(DistributedId left, DistributedId right) => left.Value == right.Value;

        /// <summary>
        /// Determines if one <see cref="DistributedId"/> is unequal to another.
        /// </summary>
        /// <param name="left">The first <see cref="DistributedId"/> to compare.</param>
        /// <param name="right">The second <see cref="DistributedId"/> to compare.</param>
        /// <returns>A value indicating whether the <see cref="DistributedId"/> values are unequal.</returns>
        public static bool operator !=(DistributedId left, DistributedId right) => left.Value != right.Value;

        /// <inheritdoc />
        public int CompareTo(DistributedId other) => Value.CompareTo(other.Value);

        /// <summary>
        /// Determines if one <see cref="DistributedId"/> is smaller than another.
        /// </summary>
        /// <param name="left">The first <see cref="DistributedId"/> to compare.</param>
        /// <param name="right">The second <see cref="DistributedId"/> to compare.</param>
        /// <returns>A value indicating whether the <paramref name="left"/> <see cref="DistributedId"/> is smaller than <paramref name="right"/>.</returns>
        public static bool operator <(DistributedId left, DistributedId right) => left.Value < right.Value;

        /// <summary>
        /// Determines if one <see cref="DistributedId"/> is smaller than or equal to another.
        /// </summary>
        /// <param name="left">The first <see cref="DistributedId"/> to compare.</param>
        /// <param name="right">The second <see cref="DistributedId"/> to compare.</param>
        /// <returns>A value indicating whether the <paramref name="left"/> <see cref="DistributedId"/> is smaller than or equal to <paramref name="right"/>.</returns>
        public static bool operator <=(DistributedId left, DistributedId right) => left.Value <= right.Value;

        /// <summary>
        /// Determines if one <see cref="DistributedId"/> is greater than another.
        /// </summary>
        /// <param name="left">The first <see cref="DistributedId"/> to compare.</param>
        /// <param name="right">The second <see cref="DistributedId"/> to compare.</param>
        /// <returns>A value indicating whether the <paramref name="left"/> <see cref="DistributedId"/> is greater than <paramref name="right"/>.</returns>
        public static bool operator >(DistributedId left, DistributedId right) => left.Value > right.Value;

        /// <summary>
        /// Determines if one <see cref="DistributedId"/> is greater than or equal to another.
        /// </summary>
        /// <param name="left">The first <see cref="DistributedId"/> to compare.</param>
        /// <param name="right">The second <see cref="DistributedId"/> to compare.</param>
        /// <returns>A value indicating whether the <paramref name="left"/> <see cref="DistributedId"/> is greater than or equal to <paramref name="right"/>.</returns>
        public static bool operator >=(DistributedId left, DistributedId right) => left.Value >= right.Value;
    }
}
