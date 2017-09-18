using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SourceCode.Clay.Data.SqlParser
{
    internal static class SqlTokenizer
    {
        #region Tokenize

        /// <summary>
        ///
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="skipSundry">Do not emit sundry tokens (such as comments and whitespace) in the output.</param>
        /// <returns></returns>
        public static IEnumerable<SqlTokenInfo> Tokenize(string sql, bool skipSundry)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            using (var reader = new SqlCharReader(sql))
            {
                var tokens = Tokenize(reader, skipSundry);
                return tokens;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="skipSundry">Do not emit sundry tokens (such as comments and whitespace) in the output.</param>
        /// <returns></returns>
        public static IEnumerable<SqlTokenInfo> Tokenize(TextReader reader, bool skipSundry)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            using (var cr = new SqlCharReader(reader))
            {
                var tokens = Tokenize(cr, skipSundry);
                return tokens;
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="skipSundry">Do not emit sundry tokens (such as comments and whitespace) in the output.</param>
        /// <returns></returns>
        private static IEnumerable<SqlTokenInfo> Tokenize(SqlCharReader reader, bool skipSundry)
        {
            Debug.Assert(reader != null);
            var peekBuffer = new char[2];

            while (true)
            {
                reader.FillLength(peekBuffer, peekBuffer.Length, out var peekLength);
                if (peekLength <= 0)
                    yield break;

                SqlTokenInfo token;

                var kind = Peek(peekBuffer, peekLength);
                switch (kind)
                {
                    case SqlTokenKind.Whitespace:
                        token = ReadWhitespace(peekBuffer, peekLength, reader, skipSundry);
                        if (skipSundry) continue;
                        break;

                    case SqlTokenKind.LineComment:
                        token = ReadComment(peekBuffer, peekLength, false, reader, skipSundry);
                        if (skipSundry) continue;
                        break;

                    case SqlTokenKind.BlockComment:
                        token = ReadComment(peekBuffer, peekLength, true, reader, skipSundry);
                        if (skipSundry) continue;
                        break;

                    case SqlTokenKind.Literal:
                        token = ReadLiteral(peekBuffer, peekLength, reader);
                        break;

                    case SqlTokenKind.Symbol:
                        token = ReadSymbol(peekBuffer, peekLength, reader);
                        break;

                    case SqlTokenKind.SquareString:
                        token = ReadSquareString(peekBuffer, peekLength, reader);
                        break;

                    case SqlTokenKind.QuotedString:
                        token = ReadQuotedString(peekBuffer, peekLength, reader);
                        break;

                    default:
                        throw new InvalidOperationException($"Unknown {nameof(SqlTokenKind)}: {kind}");
                }

                yield return token;
            }
        }

        private static SqlTokenKind Peek(char[] peekBuffer, int peekLength)
        {
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekLength >= 1);

            var canPeekDouble = peekLength >= 2;

            switch (peekBuffer[0])
            {
                // Line Comment
                case '-':
                    return canPeekDouble && peekBuffer[1] == '-' ? SqlTokenKind.LineComment : SqlTokenKind.Symbol;

                // Block Comment
                case '/':
                    return canPeekDouble && peekBuffer[1] == '*' ? SqlTokenKind.BlockComment : SqlTokenKind.Symbol;

                // Quoted String
                case 'N':
                    return canPeekDouble && peekBuffer[1] == '\'' ? SqlTokenKind.QuotedString : SqlTokenKind.Literal;

                // Quoted String
                case '"':
                case '\'':
                    return SqlTokenKind.QuotedString;

                // Square String
                case '[':
                    return SqlTokenKind.SquareString;

                // Math
                case '+':
                case '*':
                case '%':

                // Logical
                case '&':
                case '|':
                case '^':
                case '~':

                // Comparison
                case '>':
                case '<':
                case '=':

                // Symbol
                case ',':
                case '.':
                case ';':
                case '(':
                case ')':
                    return SqlTokenKind.Symbol;
            }

            if (canPeekDouble)
            {
                var s2 = new string(peekBuffer, 0, 2);
                switch (s2)
                {
                    // Math
                    case "+=":
                    case "-=":
                    case "*=":
                    case "/=":
                    case "%=":

                    // Logical
                    case "&=":
                    case "|=":
                    case "^=":

                    // Comparison
                    case ">=":
                    case "<=":
                    case "<>":
                    case "!<":
                    case "!=":
                    case "!>":

                    // Symbol
                    case "::":
                        return SqlTokenKind.Symbol;
                }
            }

            if (char.IsWhiteSpace(peekBuffer[0]))
                return SqlTokenKind.Whitespace;

            return SqlTokenKind.Literal;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Contains(char[] buffer, int offset, int count, char c0)
        {
            return count - offset >= 1
                && buffer[offset] == c0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Contains(char[] buffer, int offset, int count, char c0, char c1)
        {
            return count - offset >= 2
                && buffer[offset + 0] == c0
                && buffer[offset + 1] == c1;
        }

        #endregion

        #region Whitespace

        /// <summary>
        ///
        /// </summary>
        /// <param name="peekBuffer"></param>
        /// <param name="peekLength"></param>
        /// <param name="reader"></param>
        /// <param name="skipSundry">Do not emit sundry tokens (such as comments and whitespace) in the output.</param>
        /// <returns></returns>
        private static SqlTokenInfo ReadWhitespace(char[] peekBuffer, int peekLength, SqlCharReader reader, bool skipSundry)
        {
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekLength >= 1);
            Debug.Assert(reader != null);

            // Fast path for single whitespace
            if (peekLength >= 2
                && !char.IsWhiteSpace(peekBuffer[1]))
            {
                // Undo any extraneous data
                reader.Undo(peekBuffer, 1, peekLength - 1);

                var t0 = new SqlTokenInfo(SqlTokenKind.Whitespace, peekBuffer[0]);
                return t0;
            }

            // Perf: Assume whitespace is N chars
            const int averageLen = 10;
            var cap = peekLength + averageLen;
            var buffer = new char[cap];
            var sb = skipSundry ? null : new StringBuilder(cap);

            // We already know the incoming data is "<whitespace>", so keep it
            if (!skipSundry)
                sb.Append(peekBuffer, 0, peekLength);

            while (true)
            {
                reader.FillRemaining(buffer, 0, out var count);
                if (count <= 0) break;

                // Try find any non-whitespace in current batch
                var idx = -1;
                for (var i = 0; i < count; i++)
                {
                    if (!char.IsWhiteSpace(buffer[i]))
                    {
                        idx = i;
                        break;
                    }
                }

                // If we found any non-whitespace
                if (idx >= 0)
                {
                    // Undo any extraneous data and exit the loop
                    if (!skipSundry)
                        sb.Append(buffer, 0, idx);

                    reader.Undo(buffer, idx, count - idx);
                    break;
                }

                // Else keep the whitespace and check next batch
                if (!skipSundry)
                    sb.Append(buffer, 0, count);
            }

            var token = new SqlTokenInfo(SqlTokenKind.Whitespace, sb);
            return token;
        }

        #endregion

        #region String

        private static SqlTokenInfo ReadSquareString(char[] peekBuffer, int peekLength, SqlCharReader reader)
        {
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekBuffer[0] == '[');
            Debug.Assert(peekLength >= 1);
            Debug.Assert(reader != null);

            // Sql Identifiers are max 128 chars
            var cap = peekLength + 128;
            var buffer = new char[cap];
            var sb = new StringBuilder(cap);

            // We already know the incoming data is [, so keep it
            sb.Append(peekBuffer, 0, 1);

            // Undo any extraneous data
            if (peekLength > 1)
            {
                reader.Undo(peekBuffer, 1, peekLength - 1);
            }

            while (true)
            {
                reader.FillLength(buffer, 4, out var count);
                if (count <= 0) break;

                var eof = count <= 3;
                var cnt = eof ? count : count - 3;

                // Try find a delimiter
                var idx = -1;
                for (var i = 0; i < cnt; i++)
                {
                    if (!Contains(buffer, i, count, ']'))
                        continue;

                    idx = i;
                    break;
                }

                // If we found a delimiter
                if (idx >= 0)
                {
                    // If it is an escaped delimiter
                    // https://docs.microsoft.com/en-us/sql/t-sql/functions/quotename-transact-sql
                    if (idx >= 1 && Contains(buffer, idx, count, ']', ']'))
                    {
                        // Keep going
                        sb.Append(buffer, 0, idx + 2);
                        reader.Undo(buffer, idx + 2, count - idx - 2);

                        continue;
                    }

                    // Else undo any extraneous data and exit the loop
                    sb.Append(buffer, 0, idx + 1);
                    reader.Undo(buffer, idx + 1, count - idx - 1);

                    break;
                }

                sb.Append(buffer, 0, cnt);

                // Exit if no more data
                if (eof) break;

                // Ensure we can peek again
                reader.Undo(buffer, cnt, 3);
            }

            var token = new SqlTokenInfo(SqlTokenKind.SquareString, sb);
            return token;
        }

        private static SqlTokenInfo ReadQuotedString(char[] peekBuffer, int peekLength, SqlCharReader reader)
        {
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekLength >= 1);
            Debug.Assert(reader != null);

            // Perf: Assume string is N chars
            const int averageLen = 256;
            var cap = peekLength + averageLen;
            var sb = new StringBuilder(cap);

            // We already know the incoming data is "<quote>", so keep it
            var len = peekBuffer[0] == '\'' || peekBuffer[0] == '"' ? 1 : 2;
            sb.Append(peekBuffer, 0, len);

            // Undo any extraneous data
            if (peekLength > len)
            {
                reader.Undo(peekBuffer, len, peekLength - len);
            }

            // Read string value
            switch (peekBuffer[0])
            {
                case '"': // "abc"
                    {
                        var token = ReadQuotedString(sb, '"', reader);
                        return token;
                    }

                case '\'': // 'abc'
                    {
                        var token = ReadQuotedString(sb, '\'', reader);
                        return token;
                    }

                case 'N': // N'abc'
                    if (peekLength >= 2 && peekBuffer[1] == '\'')
                    {
                        var token = ReadQuotedString(sb, '\'', reader);
                        return token;
                    }
                    break;
            }

            throw new ArgumentException("Invalid quoted string: " + new string(peekBuffer, 0, peekLength));
        }

        private static SqlTokenInfo ReadQuotedString(StringBuilder sb, char delimiter, SqlCharReader reader)
        {
            Debug.Assert(sb != null);
            Debug.Assert(reader != null);

            var buffer = new char[sb.Capacity];

            while (true)
            {
                reader.FillLength(buffer, 4, out var count);
                if (count <= 0) break;

                var eof = count <= 3;
                var cnt = eof ? count : count - 3;

                // Try find a delimiter
                var idx = -1;
                for (var i = 0; i < cnt; i++)
                {
                    if (!Contains(buffer, i, count, delimiter))
                        continue;

                    idx = i;
                    break;
                }

                // If we found a delimiter
                if (idx >= 0)
                {
                    // If it is an escaped delimiter
#pragma warning disable S4142 // Duplicate values should not be passed as arguments
                    if (idx >= 1 && Contains(buffer, idx - 1, count, delimiter, delimiter))
#pragma warning restore S4142 // Duplicate values should not be passed as arguments
                    {
                        // Keep going
                        sb.Append(buffer, 0, idx + 2);
                        reader.Undo(buffer, idx + 2, count - idx - 2);

                        continue;
                    }

                    // Else undo any extraneous data and exit the loop
                    sb.Append(buffer, 0, idx + 1);
                    reader.Undo(buffer, idx + 1, count - idx - 1);

                    break;
                }

                sb.Append(buffer, 0, cnt);

                // Exit if no more data
                if (eof) break;

                // Ensure we can peek again
                reader.Undo(buffer, cnt, 3);
            }

            var token = new SqlTokenInfo(SqlTokenKind.QuotedString, sb);
            return token;
        }

        #endregion

        #region Comment

        /// <summary>
        ///
        /// </summary>
        /// <param name="peekBuffer"></param>
        /// <param name="peekLength"></param>
        /// <param name="isBlockComment"></param>
        /// <param name="reader"></param>
        /// <param name="skipSundry">Do not emit sundry tokens (such as comments and whitespace) in the output.</param>
        /// <returns></returns>
        private static SqlTokenInfo ReadComment(char[] peekBuffer, int peekLength, bool isBlockComment, SqlCharReader reader, bool skipSundry)
        {
            Debug.Assert(reader != null);
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekLength >= 1);

            // Perf: Assume comment is N chars
            const int averageLen = 50;
            var cap = peekLength + averageLen;
            var buffer = new char[cap];
            var sb = skipSundry ? null : new StringBuilder(cap);

            // We already know the incoming data is -- or /*, so keep it
            if (!skipSundry)
                sb.Append(peekBuffer, 0, peekLength);

            // Block/Line Comment
            var kind = SqlTokenKind.LineComment;
            var delimiter = "\r\n";
            if (isBlockComment)
            {
                kind = SqlTokenKind.BlockComment;
                delimiter = "*/";
            }

            while (true)
            {
                reader.FillLength(buffer, 5, out var count);
                if (count <= 0) break;

                var eof = count <= 4;
                var cnt = eof ? count : count - 4;

                // Try find a delimiter
                var idx = -1;
                for (var i = 0; i < cnt; i++)
                {
                    if (!Contains(buffer, i, count, delimiter[0], delimiter[1]))
                        continue;

                    idx = i;
                    break;
                }

                // If we found a delimiter
                if (idx >= 0)
                {
                    // Undo any extraneous data and exit the loop
                    if (!skipSundry)
                        sb.Append(buffer, 0, idx + 2);

                    reader.Undo(buffer, idx + 2, count - idx - 2);

                    break;
                }

                if (!skipSundry)
                    sb.Append(buffer, 0, cnt);

                // Exit if no more data
                if (eof) break;

                // Ensure we can peek again
                reader.Undo(buffer, cnt, 4);
            }

            var token = new SqlTokenInfo(kind, sb);
            return token;
        }

        #endregion

        #region Symbol

        private static SqlTokenInfo ReadSymbol(char[] peekBuffer, int peekLength, SqlCharReader reader)
        {
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekLength >= 1);
            Debug.Assert(reader != null);

            if (peekLength >= 2)
            {
                // First check if it's a double-symbol
                var peek = new string(peekBuffer, 0, 2);
                switch (peek)
                {
                    // Math
                    case "+=":
                    case "-=":
                    case "*=":
                    case "/=":
                    case "%=":

                    // Logical
                    case "&=":
                    case "|=":
                    case "^=":

                    // Comparison
                    case ">=":
                    case "<=":
                    case "<>":
                    case "!<":
                    case "!=":
                    case "!>":

                    // Programmatic
                    case "::":

                        var doubleToken = new SqlTokenInfo(SqlTokenKind.Symbol, peekBuffer, 0, peekLength);
                        return doubleToken;
                }
            }

            // It must have been a single symbol
            reader.Undo(peekBuffer, 1, peekLength - 1);

            var token = new SqlTokenInfo(SqlTokenKind.Symbol, peekBuffer, 0, 1);
            return token;
        }

        #endregion

        #region Literal

        private static SqlTokenInfo ReadLiteral(char[] peekBuffer, int peekLength, SqlCharReader reader)
        {
            Debug.Assert(peekBuffer != null);
            Debug.Assert(peekBuffer.Length >= 1);
            Debug.Assert(peekLength >= 1);
            Debug.Assert(reader != null);

            // Sql literals are generally short
            var cap = peekLength + 32;
            var buffer = new char[cap];
            var sb = new StringBuilder(cap);

            // We already know the incoming data is "<literal>", so keep it
            sb.Append(peekBuffer, 0, 1);

            // Undo any extraneous data
            if (peekLength >= 2)
            {
                reader.Undo(peekBuffer, 1, peekLength - 1);
            }

            while (true)
            {
                reader.FillRemaining(buffer, 0, out var count);
                if (count <= 0) break;

                for (var i = 0; i < count; i++)
                {
                    switch (buffer[i])
                    {
                        // Math
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                        case '%':

                        // Logical
                        case '&':
                        case '|':
                        case '^':
                        case '~':

                        // Comparison
                        case '>':
                        case '<':
                        case '!':
                        case '=':

                        // Quote
                        case '\'':
                        case '"':

                        // Symbol
                        case ',':
                        case '.':
                        case ';':
                        case ':':
                        case '(':
                        case ')':
                            // Exit if delimiter
                            break;

                        default:
                            // Exit if whitespace
                            if (char.IsWhiteSpace(buffer[i])) break;

                            // Loop if not whitespace
                            continue;
                    }

                    if (i >= 1)
                        sb.Append(buffer, 0, i);

                    reader.Undo(buffer, i, count - i);

                    var t1 = new SqlTokenInfo(SqlTokenKind.Literal, sb);
                    return t1;
                }

                sb.Append(buffer, 0, count);
            }

            var token = new SqlTokenInfo(SqlTokenKind.Literal, sb);
            return token;
        }

        #endregion
    }
}
