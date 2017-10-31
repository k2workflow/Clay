#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace SourceCode.Clay.Json.Validation
{
    /// <summary>
    /// Represents a Json pattern constraint.
    /// </summary>
    public sealed class PatternConstraint
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

        /// <summary>
        /// Gets a value specifying whether a value is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Gets a value indicating whether any of the constraints are specified.
        /// </summary>
        public bool IsConstrained => !string.IsNullOrWhiteSpace(Pattern) || Required;

        #endregion

        #region Constructors

        public PatternConstraint(string pattern, bool required, RegexOptions options, TimeSpan timeout)
        {
            if (string.IsNullOrWhiteSpace(pattern)) throw new ArgumentNullException(nameof(pattern));

            Pattern = pattern;
            Required = required;
            _regex = new Lazy<Regex>(build, LazyThreadSafetyMode.PublicationOnly);

            // Local function
            Regex build() => new Regex(pattern, options, timeout);
        }

        public PatternConstraint(string pattern, bool required)
            : this(pattern, required, DefaultOptions, DefaultTimeout)
        { }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.NoOptimization)] // We ignore result but want the side-effects
        private static void Clear(Regex regex)
        {
            // Heap analysis shows Regex permanently holds onto last input string, which may be large

            regex.IsMatch(string.Empty);
        }

        public bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
                return !Required;

            var isMatch = _regex.Value.IsMatch(value);

            Clear(_regex.Value);

            return isMatch;
        }

        public override string ToString()
            => (Required ? "Required: " : string.Empty) + Pattern;

        #endregion
    }
}
