using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace SourceCode.Clay.Data.SqlParser
{
    public static partial class SqlParamBlockParser
    {
        #region Parse

        // https://docs.microsoft.com/en-us/sql/t-sql/statements/create-procedure-transact-sql
        public static IReadOnlyDictionary<string, SqlParamInfo> ParseProcedure(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return null;

            // Tokenize
            var tokens = SqlTokenizer.Tokenize(sql, true);
            using (var tokenizer = tokens.GetEnumerator())
            {
                var more = tokenizer.MoveNext();
                if (!more)
                    return null;

                // CREATE
                if (!ParseLiteral(tokenizer, "CREATE"))
                    return null;

                // PROC | PROCEDURE
                if (!ParseLiteral(tokenizer, "PROC", "PROCEDURE"))
                    return null;

                // [Name] or "Name"
                if (!ParseModuleName(tokenizer, out string schema, out string name))
                    return null;

                // (
                var parenthesized = ParseSymbol(tokenizer, '(');

                var parms = new Dictionary<string, SqlParamInfo>();

                while (true)
                {
                    // Exit loop if end of parameter block

                    // )
                    if (parenthesized
                        && ParseSymbol(tokenizer, ')'))
                        break;

                    // AS, WITH or FOR
                    if (ParseLiteral(tokenizer, "AS", "WITH", "FOR"))
                        break;

                    // Read next parameter

                    // @param
                    if (!ParseParamName(tokenizer, out string pname))
                        return null;

                    // AS
                    ParseLiteral(tokenizer, "AS");

                    // Foo.DECIMAL(1,2)
                    if (!ParseTypeName(tokenizer, out string tschema, out string tname))
                        return null;

                    // VARYING
                    ParseLiteral(tokenizer, "VARYING");

                    // = <default>
                    var hasDefault = ParseDefault(tokenizer, out bool isNullable);

                    // OUT | OUTPUT
                    var dir = ParameterDirection.Input;
                    if (ParseLiteral(tokenizer, "OUT", "OUTPUT"))
                    {
                        dir = ParameterDirection.InputOutput;
                    }

                    // READONLY
                    var isReadOnly = ParseLiteral(tokenizer, "READONLY");

                    // Done
                    var parm = new SqlParamInfo(isNullable, hasDefault, isReadOnly, dir);
                    parms.Add(pname, parm);

                    // ,
                    ParseSymbol(tokenizer, ',');
                }

                return parms;
            }
        }

        // https://docs.microsoft.com/en-us/sql/t-sql/statements/create-function-transact-sql
        public static IReadOnlyDictionary<string, SqlParamInfo> ParseFunction(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return null;

            // Tokenize
            var tokens = SqlTokenizer.Tokenize(sql, true);
            using (var tokenizer = tokens.GetEnumerator())
            {
                var more = tokenizer.MoveNext();
                if (!more)
                    return null;

                // CREATE
                if (!ParseLiteral(tokenizer, "CREATE"))
                    return null;

                // FUNCTION
                if (!ParseLiteral(tokenizer, "FUNCTION"))
                    return null;

                // [Name] or "Name"
                if (!ParseModuleName(tokenizer, out string schema, out string name))
                    return null;

                // (
                if (!ParseSymbol(tokenizer, '('))
                    return null;

                var parms = new Dictionary<string, SqlParamInfo>(StringComparer.Ordinal);

                while (true)
                {
                    // Exit loop if end of parameter block

                    // )
                    if (ParseSymbol(tokenizer, ')'))
                        break;

                    // RETURNS
                    if (ParseLiteral(tokenizer, "RETURNS"))
                        break;

                    // Read next parameter

                    // @param
                    if (!ParseParamName(tokenizer, out string pname))
                        return null;

                    // AS
                    ParseLiteral(tokenizer, "AS");

                    // Foo.DECIMAL(1,2)
                    if (!ParseTypeName(tokenizer, out string tschema, out string tname))
                        return null;

                    // = default
                    var hasDefault = ParseDefault(tokenizer, out bool isNullable);

                    // READONLY
                    var isReadOnly = ParseLiteral(tokenizer, "READONLY");

                    // Done
                    var parm = new SqlParamInfo(isNullable, hasDefault, isReadOnly, ParameterDirection.Input);
                    parms.Add(pname, parm);

                    // ,
                    ParseSymbol(tokenizer, ',');
                }

                return parms;
            }
        }

        #endregion

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ParseSymbol(IEnumerator<SqlTokenInfo> tokenizer, char expected)
        {
            if (tokenizer.Current.Kind != SqlTokenKind.Symbol)
                return false;

            if (tokenizer.Current.Value == null) return false;
            if (tokenizer.Current.Value.Length != 1) return false;
            if (tokenizer.Current.Value[0] != expected) return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseLiteral(IEnumerator<SqlTokenInfo> tokenizer, string expected)
        {
            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            if (!StringComparer.OrdinalIgnoreCase.Equals(tokenizer.Current.Value, expected))
                return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseLiteral(IEnumerator<SqlTokenInfo> tokenizer, string expected1, string expected2)
        {
            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            if (!StringComparer.OrdinalIgnoreCase.Equals(tokenizer.Current.Value, expected1)
                && !StringComparer.OrdinalIgnoreCase.Equals(tokenizer.Current.Value, expected2))
                return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseLiteral(IEnumerator<SqlTokenInfo> tokenizer, string expected1, string expected2, string expected3)
        {
            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            if (!StringComparer.OrdinalIgnoreCase.Equals(tokenizer.Current.Value, expected1)
                && !StringComparer.OrdinalIgnoreCase.Equals(tokenizer.Current.Value, expected2)
                && !StringComparer.OrdinalIgnoreCase.Equals(tokenizer.Current.Value, expected3))
                return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseParamName(IEnumerator<SqlTokenInfo> tokenizer, out string name)
        {
            name = null;

            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            if (!tokenizer.Current.Value.StartsWith("@"))
                return false;

            name = tokenizer.Current.Value;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseDefault(IEnumerator<SqlTokenInfo> tokenizer, out bool isNullable)
        {
            isNullable = false;

            if (!ParseSymbol(tokenizer, '='))
                return false;

            if (tokenizer.Current.Kind != SqlTokenKind.Literal
                && tokenizer.Current.Kind != SqlTokenKind.QuotedString)
                return false;

            isNullable = ParseLiteral(tokenizer, "NULL");

            // Fail if EOF
            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseModuleName(IEnumerator<SqlTokenInfo> tokenizer, out string schema, out string name)
        {
            schema = null;
            name = null;

            if (tokenizer.Current.Kind != SqlTokenKind.Literal
                && tokenizer.Current.Kind != SqlTokenKind.QuotedString
                && tokenizer.Current.Kind != SqlTokenKind.SquareString)
                return false;

            var str0 = tokenizer.Current.Value; // Capture before .MoveNext

            // Fail if EOF
            var more = tokenizer.MoveNext();
            if (!more) return false;

            // Simple name - no schema
            if (tokenizer.Current.Kind != SqlTokenKind.Symbol || tokenizer.Current.Value != ".")
            {
                name = str0;
                return true;
            }

            // Fail if EOF
            more = tokenizer.MoveNext();
            if (!more) return false;

            if (tokenizer.Current.Kind != SqlTokenKind.Literal
                && tokenizer.Current.Kind != SqlTokenKind.QuotedString
                && tokenizer.Current.Kind != SqlTokenKind.SquareString)
                return false;

            var str1 = tokenizer.Current.Value; // Capture before .MoveNext

            // Fail if EOF
            more = tokenizer.MoveNext();
            if (!more) return false;

            // Schematized name
            schema = str0;
            name = str1;
            return true;
        }

        private static bool ParseTypeName(IEnumerator<SqlTokenInfo> tokenizer, out string schema, out string type)
        {
            schema = null;
            type = null;

            if (tokenizer.Current.Kind != SqlTokenKind.Literal
                && tokenizer.Current.Kind != SqlTokenKind.QuotedString
                && tokenizer.Current.Kind != SqlTokenKind.SquareString)
                return false;

            var sb = new StringBuilder(tokenizer.Current.Value); // Capture before .MoveNext

            // Fail if EOF
            var more = tokenizer.MoveNext();
            if (!more) return false;

            // Simple type - no schema, no type details
            if (tokenizer.Current.Kind != SqlTokenKind.Symbol)
            {
                type = sb.ToString();
                return true;
            }

            // Schematized type
            if (tokenizer.Current.Value == ".")
            {
                // Fail if EOF
                more = tokenizer.MoveNext();
                if (!more) return false;

                if (tokenizer.Current.Kind != SqlTokenKind.Literal
                    && tokenizer.Current.Kind != SqlTokenKind.QuotedString
                    && tokenizer.Current.Kind != SqlTokenKind.SquareString)
                    return false;

                // Previous name must have been the schema
                schema = sb.ToString();

                // Start capturing the new name
                sb.Clear();
                sb.Append(tokenizer.Current.Value); // Capture before .MoveNext

                // Fail if EOF
                more = tokenizer.MoveNext();
                if (!more) return false;
            }

            // Simple schematized type - no type details
            if (tokenizer.Current.Kind != SqlTokenKind.Symbol || tokenizer.Current.Value != "(")
            {
                type = sb.ToString();
                return true;
            }

            sb.Append('(');

            // Fail if EOF
            more = tokenizer.MoveNext();
            if (!more) return false;

            // First specifier
            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            sb.Append(tokenizer.Current.Value);

            // Fail if EOF
            more = tokenizer.MoveNext();
            if (!more) return false;

            // Second specifier
            if (tokenizer.Current.Kind != SqlTokenKind.Symbol)
                return false;

            if (tokenizer.Current.Value == ",")
            {
                sb.Append(',');

                // Fail if EOF
                more = tokenizer.MoveNext();
                if (!more) return false;

                if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                    return false;

                sb.Append(tokenizer.Current.Value);

                // Fail if EOF
                more = tokenizer.MoveNext();
                if (!more) return false;
            }

            // )
            if (tokenizer.Current.Kind != SqlTokenKind.Symbol || tokenizer.Current.Value != ")")
                return false;

            sb.Append(')');

            // Fail if EOF
            more = tokenizer.MoveNext();
            if (!more) return false;

            type = sb.ToString();
            return true;
        }

        #endregion
    }
}
