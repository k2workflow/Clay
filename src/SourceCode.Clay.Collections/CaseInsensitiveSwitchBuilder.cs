#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// A dynamic switch for dictionaries with <see cref="System.String"/> keys.
    /// Uses an case-insensitive (invariant) comparison of key values.
    /// </summary>
    /// <typeparam name="TValue">The type of values.</typeparam>
    internal sealed class CaseInsensitiveSwitchBuilder<TValue> : BaseSwitchBuilder<string, TValue>
    {
        #region Methods

        /// <summary>
        /// Normalizes each switch key, so that comparisons are case-insensitive.
        /// </summary>
        /// <param name="key">The key value to be transformed.</param>
        /// <returns>
        /// The transformed key value.
        /// </returns>
        protected override string NormalizeKey(string key) => key.ToUpperInvariant();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseInsensitiveSwitchBuilder{TValue}"/> class.
        /// </summary>
        /// <param name="cases">The cases.</param>
        public CaseInsensitiveSwitchBuilder(IReadOnlyDictionary<string, TValue> cases)
            : base(cases)
        { }

        #endregion

        // Apparently UC (vs LC) is optimized in CLR
    }
}
