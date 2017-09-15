using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace SourceCode.Clay.Json.Validation
{
    public sealed class PatternValidator
    {
        #region Constants

        public const RegexOptions DefaultOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant;
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMilliseconds(500);

        #endregion

        #region Fields

        private readonly Lazy<Regex> _regex;

        #endregion

        #region Properties

        public string Pattern { get; }

        public bool Required { get; }

        #endregion

        #region Constructors

        public PatternValidator(string pattern, bool required, RegexOptions options, TimeSpan timeout)
        {
            if (!string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));

            Pattern = pattern;
            Required = required;

            _regex = new Lazy<Regex>(() => new Regex(pattern, options, timeout), LazyThreadSafetyMode.PublicationOnly);
        }

        public PatternValidator(string pattern, bool required)
            : this(pattern, required, DefaultOptions, DefaultTimeout)
        { }

        #endregion

        #region Methods

        public bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return !Required;

            var isMatch = _regex.Value.IsMatch(value);

            Clear(_regex.Value);

            return isMatch;
        }

        // Heap analysis shows Regex permanently holds onto last input string, which may be large
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static void Clear(Regex regex)
        {
            regex.IsMatch(string.Empty);
        }

        public override string ToString()
            => (Required ? "Required: " : string.Empty) + Pattern;

        #endregion
    }
}
