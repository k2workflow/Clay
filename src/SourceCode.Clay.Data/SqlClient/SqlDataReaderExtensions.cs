using System;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    ///   Represents extensions for <see cref="SqlDataReader"/> instances.
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlDataReader"/>
    public static class SqlDataReaderExtensions
    {
        /// <summary>
        ///   Gets the value of the specified binary column as <see cref="System.Byte"/>[].
        /// </summary>
        /// <param name="sqlDr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns></returns>
        public static byte[] GetSqlBytes(this SqlDataReader sqlDr, string name)
        {
            if (sqlDr == null) throw new ArgumentNullException(nameof(sqlDr));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var ord = sqlDr.GetOrdinal(name); // Throws IndexOutOfRangeException
            if (sqlDr.IsDBNull(ord))
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            var val = sqlDr.GetSqlBytes(ord);
            return val.IsNull ? null : val.Buffer;
        }

        /// <summary>
        ///   Gets the value of the specified text column as an <see cref="System.Enum"/>.
        /// </summary>
        /// <param name="sqlDr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <typeparam name="TEnum">The type of enum.</typeparam>
        /// <returns></returns>
        public static TEnum? GetSqlEnum<TEnum>(this SqlDataReader sqlDr, string name)
           where TEnum : struct, IComparable, IFormattable, IConvertible // We cannot directly constrain by Enum, so approximate by constraining on Enum's implementation
        {
            if (sqlDr == null) throw new ArgumentNullException(nameof(sqlDr));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var ord = sqlDr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = sqlDr.GetSqlString(ord);

            if (val.IsNull)
                return null;

            var e = (TEnum)Enum.Parse(typeof(TEnum), val.Value, true);
            return e;
        }

        /// <summary>
        ///   Gets the value of the specified xml column as a <see cref="System.String"/>.
        /// </summary>
        /// <param name="sqlDr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns></returns>
        public static string GetSqlXml(this SqlDataReader sqlDr, string name)
        {
            if (sqlDr == null) throw new ArgumentNullException(nameof(sqlDr));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var ord = sqlDr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = sqlDr.GetSqlXml(ord);

            if (val.IsNull)
                return null;

            var xml = val.Value;
            return xml;
        }

        /// <summary>
        ///   Gets the value of the specified xml column as a <see cref="XDocument"/>.
        /// </summary>
        /// <param name="sqlDr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns></returns>
        public static XDocument GetSqlXDocument(this SqlDataReader sqlDr, string name)
        {
            if (sqlDr == null) throw new ArgumentNullException(nameof(sqlDr));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var ord = sqlDr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = sqlDr.GetSqlXml(ord);

            if (val.IsNull)
                return null;

            using (var xmr = val.CreateReader())
            {
                var xml = XDocument.Load(xmr);
                return xml;
            }
        }
    }
}
