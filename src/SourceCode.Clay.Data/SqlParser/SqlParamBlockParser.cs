using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SourceCode.Clay.Data.SqlParser
{
    public static partial class SqlParamBlockParser
    {
        #region Parse

        // https://docs.microsoft.com/en-us/sql/t-sql/statements/create-procedure-transact-sql
        public static IReadOnlyDictionary<string, SqlParamInfo> ParseProcedure(string sql, out IList<string> parseErrors)
        {
            parseErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(sql))
                return null;

            // Tokenize
            var tokens = SqlTokenizer.Tokenize(sql, true);
            using (var tokenizer = tokens.GetEnumerator())
            {
                var more = tokenizer.MoveNext();
                if (!more)
                {
                    parseErrors.Add("Unexpected end of input");
                    return null;
                }

                // CREATE
                if (!ParseLiteral(tokenizer, "CREATE"))
                {
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "CREATE"), tokenizer.Current);
                    return null;
                }

                // PROC | PROCEDURE
                if (!ParseLiteral(tokenizer, "PROCEDURE", "PROC"))
                {
                    Console.Out.WriteLine("-----");
                    Console.Out.WriteLine("PROC NOT FOUND");
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "PROCEDURE"), tokenizer.Current);
                    return null;
                }
                else
                {
                    Console.Out.WriteLine("-----");
                    Console.Out.WriteLine("PROC FOUND");
                }

                // [Name] or "Name"
                if (!ParseModuleName(tokenizer, out string schema, out string name))
                {
                    Console.Out.WriteLine("-----");
                    Console.Out.WriteLine("PROC NAME FOUND");
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "<module name>"), tokenizer.Current);
                    return null;
                }

                // (
                var parenthesized = ParseSymbol(tokenizer, '(');
                if (parenthesized)
                {
                    Console.Out.WriteLine("-----");
                    Console.Out.WriteLine("PAREN FOUND");
                }

                var parms = new Dictionary<string, SqlParamInfo>();

                while (true)
                {
                    // Exit loop if end of parameter block

                    // )
                    if (ParseSymbol(tokenizer, ')'))
                    {
                        Console.Out.WriteLine("-----");
                        Console.Out.WriteLine(") FOUND");

                        if (parenthesized) break;

                        parseErrors.Add($"Unexpected token {tokenizer.Current}.");
                        return null;
                    }

                    // AS, WITH or FOR
                    if (ParseLiteral(tokenizer, "AS", "WITH", "FOR"))
                        break;

                    // Read next parameter

                    // @param
                    if (!ParseParamName(tokenizer, out string pname))
                    {
                        BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "<param name>"), tokenizer.Current);
                        return null;
                    }

                    // AS
                    ParseLiteral(tokenizer, "AS");

                    // Foo.DECIMAL(1,2)
                    if (!ParseTypeName(tokenizer, out string tschema, out string tname))
                    {
                        BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "<param type>"), tokenizer.Current);
                        return null;
                    }

                    // VARYING
                    ParseLiteral(tokenizer, "VARYING");

                    // = <default>
                    var hasDefault = ParseDefault(tokenizer, out bool isNullable);

                    // OUT | OUTPUT
                    var dir = ParameterDirection.Input;
                    if (ParseLiteral(tokenizer, "OUT", "OUTPUT"))
                        dir = ParameterDirection.InputOutput;

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

        private static void BuildErrorMessage(IList<string> errors, SqlTokenInfo expected, SqlTokenInfo actual)
        {
            errors.Add($"Expected token {expected} but got {actual}.");
        }

        // https://docs.microsoft.com/en-us/sql/t-sql/statements/create-function-transact-sql
        public static IReadOnlyDictionary<string, SqlParamInfo> ParseFunction(string sql, out IList<string> parseErrors)
        {
            parseErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(sql))
                return null;

            // Tokenize
            var tokens = SqlTokenizer.Tokenize(sql, true);
            using (var tokenizer = tokens.GetEnumerator())
            {
                var more = tokenizer.MoveNext();
                if (!more)
                {
                    parseErrors.Add("Unexpected end of input");
                    return null;
                }

                // CREATE
                if (!ParseLiteral(tokenizer, "CREATE"))
                {
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "CREATE"), tokenizer.Current);
                    return null;
                }

                // FUNCTION
                if (!ParseLiteral(tokenizer, "FUNCTION"))
                {
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "FUNCTION"), tokenizer.Current);
                    return null;
                }

                // [Name] or "Name"
                if (!ParseModuleName(tokenizer, out string schema, out string name))
                {
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "<module name>"), tokenizer.Current);
                    return null;
                }

                // (
                if (!ParseSymbol(tokenizer, '('))
                {
                    BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Symbol, "("), tokenizer.Current);
                    return null;
                }

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
                    {
                        BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "<param name>"), tokenizer.Current);
                        return null;
                    }

                    // AS
                    ParseLiteral(tokenizer, "AS");

                    // Foo.DECIMAL(1,2)
                    if (!ParseTypeName(tokenizer, out string tschema, out string tname))
                    {
                        BuildErrorMessage(parseErrors, new SqlTokenInfo(SqlTokenKind.Literal, "<param type>"), tokenizer.Current);
                        return null;
                    }

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

            var actual = tokenizer.Current.Value;
            if (actual == null) return false;
            if (actual.Length != 1) return false;
            if (actual[0] != expected) return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseLiteral(IEnumerator<SqlTokenInfo> tokenizer, string expected)
        {
            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            var actual = tokenizer.Current.Value;
            if (!StringComparer.OrdinalIgnoreCase.Equals(actual, expected))
                return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseLiteral(IEnumerator<SqlTokenInfo> tokenizer, string expected1, string expected2)
        {
            var current = tokenizer.Current;
            if (current.Kind != SqlTokenKind.Literal)
                return false;

            var actual = current.Value; // PROC

            Console.Out.WriteLine(actual);
            Console.Out.WriteLine(expected1);
            Console.Out.WriteLine(expected2);

            bool a = StringComparer.OrdinalIgnoreCase.Equals(actual, expected1);
            Console.Out.WriteLine(a);
            bool b = StringComparer.OrdinalIgnoreCase.Equals(actual, expected2);
            Console.Out.WriteLine(b);
            Console.Out.Write(a || b);
            Console.Out.Write(StringComparer.OrdinalIgnoreCase.Equals(actual, expected1) || StringComparer.OrdinalIgnoreCase.Equals(actual, expected2));

            if (!(StringComparer.OrdinalIgnoreCase.Equals(actual, expected1)
                || StringComparer.OrdinalIgnoreCase.Equals(actual, expected2)))
            {
                if (actual == "PROCedure")
                {
                    var s = actual.ToCharArray().Select(c => (int)c).Select(n => n.ToString());
                    throw new Exception(string.Join(",", s));
                }

                return false;
            }

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseLiteral(IEnumerator<SqlTokenInfo> tokenizer, string expected1, string expected2, string expected3)
        {
            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            var actual = tokenizer.Current.Value;
            if (!StringComparer.OrdinalIgnoreCase.Equals(actual, expected1)
                && !StringComparer.OrdinalIgnoreCase.Equals(actual, expected2)
                && !StringComparer.OrdinalIgnoreCase.Equals(actual, expected3))
                return false;

            var more = tokenizer.MoveNext();
            return more;
        }

        private static bool ParseParamName(IEnumerator<SqlTokenInfo> tokenizer, out string name)
        {
            name = null;

            if (tokenizer.Current.Kind != SqlTokenKind.Literal)
                return false;

            var actual = tokenizer.Current.Value;
            if (!actual.StartsWith("@"))
                return false;

            name = actual;

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
