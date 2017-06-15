using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace SourceCode.Clay.Data.SqlClient
{
    public static class SqlDataReaderExtensions
    {
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

        public static TE? GetSqlEnum<TE>(this SqlDataReader dr, string name)
           where TE : struct, IComparable, IFormattable, IConvertible // We cannot directly constrain by Enum, so approximate by constraining on Enum's implementation
        {
            Contract.Requires(dr != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            var ord = dr.GetOrdinal(name); // Throws IndexOutOfRangeException
            var val = dr.GetSqlString(ord);

            TE e;
            if (val.IsNull || !Enum.TryParse(val.Value, true, out e))
                return null;

            return e;
        }

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
