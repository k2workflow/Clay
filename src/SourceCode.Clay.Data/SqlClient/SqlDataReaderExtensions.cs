using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace SourceCode.Clay.Data.SqlClient
{
    /// <summary>
    /// Represents extensions for <see cref="SqlDataReader"/> instances.
    /// </summary>
    /// <seealso cref="System.Data.SqlClient.SqlDataReader"/>
    public static class SqlDataReaderExtensions
    {
        /// <summary>
        /// Gets the value of the specified binary column as <see cref="System.Byte"/>[].
        /// </summary>
        /// <param name="dr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns></returns>
        public static byte[] GetSqlBytes(this SqlDataReader dr, string name)
        {
            Contract.Requires(dr != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var ord = dr.GetOrdinal(name); // Throws IndexOutOfRangeException
            if (dr.IsDBNull(ord))
                return null;

            var val = dr.GetSqlBytes(ord);
            return val.IsNull ? null : val.Buffer;
        }

        /// <summary>
        /// Gets the value of the specified text column as an <see cref="System.Enum"/>.
        /// </summary>
        /// <param name="dr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <typeparam name="TEnum">The type of enum.</typeparam>
        /// <returns></returns>
        public static TEnum? GetSqlEnum<TEnum>(this SqlDataReader dr, string name)
           where TEnum : struct, IComparable, IFormattable, IConvertible // We cannot directly constrain by Enum, so approximate by constraining on Enum's implementation
        {
            Contract.Requires(dr != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var ord = dr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = dr.GetSqlString(ord);

            if (val.IsNull)
                return null;

            var e = (TEnum)Enum.Parse(typeof(TEnum), val.Value, true);
            return e;
        }

        /// <summary>
        /// Gets the value of the specified xml column as a <see cref="System.String"/>.
        /// </summary>
        /// <param name="dr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns></returns>
        public static string GetSqlXml(this SqlDataReader dr, string name)
        {
            Contract.Requires(dr != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var ord = dr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = dr.GetSqlXml(ord);

            if (val.IsNull)
                return null;

            var xml = val.Value;
            return xml;
        }

        /// <summary>
        /// Gets the value of the specified xml column as a <see cref="XDocument"/>.
        /// </summary>
        /// <param name="dr">The data reader.</param>
        /// <param name="name">The column name.</param>
        /// <returns></returns>
        public static XDocument GetSqlXDocument(this SqlDataReader dr, string name)
        {
            Contract.Requires(dr != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var ord = dr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = dr.GetSqlXml(ord);

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
