using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SourceCode.Clay.Net.Http
{
    internal readonly struct RawUriTemplate
    {
        private const string InvalidTokenError = "The URI template contains an invalid token.";
        private const int TokenHeuristic = 8;

        private const char TokenStart = '{';
        private const char TokenEnd = '}';
        private const char DefaultStart = '=';
        private const char FormatStart = ':';
        private const char QueryStart = '?';
        private const char QuerySeparator = '&';
        private const char QueryAssign = '=';

        public IReadOnlyList<UriToken> Path { get; }
        public IReadOnlyList<UriQuery> Query { get; }
        public int LengthEstimate { get; }
        public ConstantExpression LengthEstimateConstant { get; }

        private RawUriTemplate(IReadOnlyList<UriToken> path, IReadOnlyList<UriQuery> query)
        {
            Path = path;
            Query = query;
            LengthEstimate = TokenHeuristic;

            for (var i = 0; i < path.Count; i++)
                LengthEstimate += path[i].Type == UriTokenType.Literal ? path[i].Default?.Length ?? TokenHeuristic : TokenHeuristic;

            for (var i = 0; i < query.Count; i++)
            {
                UriQuery q = query[i];

                LengthEstimate += 2; // & =
                for (var j = 0; j < q.Name.Count; j++)
                    LengthEstimate += q.Name[j].Type == UriTokenType.Literal ? q.Name[j].Default?.Length ?? TokenHeuristic : TokenHeuristic;
                for (var j = 0; j < q.Value.Count; j++)
                    LengthEstimate += q.Value[j].Type == UriTokenType.Literal ? q.Value[j].Default?.Length ?? TokenHeuristic : TokenHeuristic;
            }

            LengthEstimateConstant = Expression.Constant(LengthEstimate);
        }

        public static RawUriTemplate Parse(string template)
        {
            return Parse(template.AsSpan());
        }

        private static RawUriTemplate Parse(ReadOnlySpan<char> template)
        {
            (_, IReadOnlyList<UriToken> path) = ParseTokens(ref template, QueryStart, QueryStart);
            IReadOnlyList<UriQuery> query = ParseQuery(ref template);
            return new RawUriTemplate(path, query);
        }

        private static IReadOnlyList<UriQuery> ParseQuery(ref ReadOnlySpan<char> template)
        {
            var query = new List<UriQuery>();
            while (template.Length != 0)
            {
                (char c, IReadOnlyList<UriToken> name) = ParseTokens(ref template, QuerySeparator, QueryAssign);

                IReadOnlyList<UriToken> value = Array.Empty<UriToken>();
                if (c == QueryAssign)
                    (_, value) = ParseTokens(ref template, QuerySeparator, QuerySeparator);

                query.Add(new UriQuery(name, value));
            }

            return query;
        }

        private static (char, IReadOnlyList<UriToken>) ParseTokens(ref ReadOnlySpan<char> template, char end1, char end2)
        {
            var tokens = new List<UriToken>();
            var index = 0;
            while (index < template.Length)
            {
                var nextIndex = IndexOf(template, TokenStart, index);
                if (nextIndex < 0) nextIndex = template.Length;

                // Start of token.
                if (index != nextIndex)
                {
                    // End of current.
                    (char endChar, int endIndex) = ParseEnd(ref template, tokens, end1, end2, index, nextIndex);
                    if (endIndex >= 0) return (endChar, tokens);

                    tokens.Add(new UriToken(
                        UriTokenType.Literal,
                        null,
                        Substring(template, index, nextIndex, TokenStart),
                        null));
                    index = nextIndex;
                }

                if (index >= template.Length) break;
                index++;

                var tokenEnd = IndexOf(template, TokenEnd, index);

                string name = null;
                string format = null;
                string @default = null;
                byte state = 0;

                // Start of format.
                nextIndex = IndexOf(template, FormatStart, index);
                if (nextIndex >= 0 && nextIndex < tokenEnd)
                {
                    name = Substring(template, index, nextIndex, FormatStart);
                    state = 1;
                    index = nextIndex + 1;
                }

                // Start of default.
                nextIndex = IndexOf(template, DefaultStart, index);
                if (nextIndex >= 0 && nextIndex < tokenEnd)
                {
                    if (state == 0) name = Substring(template, index, nextIndex, DefaultStart);
                    else if (state == 1) format = Substring(template, index, nextIndex, DefaultStart);
                    state = 2;
                    index = nextIndex + 1;
                }

                // End of token.
                nextIndex = tokenEnd;
                if (nextIndex < 0) throw new FormatException(InvalidTokenError);

                if (state == 0) name = Substring(template, index, nextIndex, TokenEnd);
                else if (state == 1) format = Substring(template, index, nextIndex, TokenEnd);
                else if (state == 2) @default = Substring(template, index, nextIndex, TokenEnd);

                tokens.Add(new UriToken(
                    UriTokenType.Value,
                    name,
                    @default,
                    format));
                index = nextIndex + 1;
            }

            template = default;
            return (default, tokens);
        }

        private static (char, int) ParseEnd(ref ReadOnlySpan<char> template, List<UriToken> tokens, char end1, char end2, int index, int nextIndex)
        {
            // End of current.
            (char endChar, int endIndex) = template.IndexOf(end1, end2, index);
            if (endIndex >= 0 && endIndex <= nextIndex)
            {
                if (index != endIndex)
                {
                    tokens.Add(new UriToken(
                        UriTokenType.Literal,
                        null,
                        Substring(template, index, endIndex),
                        null));
                }

                template = template.Slice(endIndex + 1);
                return (endChar, endIndex);
            }
            return (default, -1);
        }

        private static int IndexOf(ReadOnlySpan<char> haystack, char needle, int startIndex)
        {
            var nextIndex = haystack.IndexOf(needle, startIndex);

            while (nextIndex >= 0 && nextIndex < haystack.Length - 1 && haystack[nextIndex + 1] == needle)
                nextIndex = haystack.IndexOf(needle, nextIndex + 2);

            return nextIndex;
        }

        private static string Substring(ReadOnlySpan<char> value, int startIndex, int exclusiveEndIndex)
        {
            --exclusiveEndIndex;
            if (exclusiveEndIndex >= value.Length) throw new ArgumentOutOfRangeException(nameof(exclusiveEndIndex));

            var sb = new StringBuilder(exclusiveEndIndex - startIndex + 1);
            for (var i = startIndex; i <= exclusiveEndIndex; i++)
            {
                var c = value[i];
                sb.Append(c);
            }

            return sb.ToString();
        }

        private static string Substring(ReadOnlySpan<char> value, int startIndex, int exclusiveEndIndex, char escape)
        {
            --exclusiveEndIndex;
            if (exclusiveEndIndex >= value.Length) throw new ArgumentOutOfRangeException(nameof(exclusiveEndIndex));

            var sb = new StringBuilder(exclusiveEndIndex - startIndex + 1);
            for (var i = startIndex; i <= exclusiveEndIndex; i++)
            {
                var c = value[i];
                if ((c == TokenStart || c == TokenEnd || c == escape) &&
                    (i == value.Length - 1 || value[++i] != c))
                    throw new FormatException(InvalidTokenError);
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
