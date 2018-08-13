using System;

namespace SourceCode.Clay.Javascript.Ast
{
    public struct JSRegex : IEquatable<JSRegex>
    {
        public string Pattern { get; }

        public JSRegexOptions Options { get; }

        public JSRegex(string pattern, JSRegexOptions options)
        {
            Pattern = pattern;
            Options = options;
        }

        public JSRegex(string pattern)
            : this(pattern, JSRegexOptions.None)
        {
        }

        public override bool Equals(object obj) => obj is JSRegex o && Equals(o);

        public bool Equals(JSRegex other)
            => Pattern == other.Pattern
            && Options == other.Options;

        public override int GetHashCode() => 0;

        public static bool operator ==(JSRegex regex1, JSRegex regex2) => regex1.Equals(regex2);

        public static bool operator !=(JSRegex regex1, JSRegex regex2) => !(regex1 == regex2);
    }
}
