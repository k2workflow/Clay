#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// A dynamic switch for dictionaries with <see cref="System.String"/> keys.
    /// Uses a case-sensitive (invariant) comparison of key values.
    /// </summary>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValue">The type of values.</typeparam>
    internal sealed class StructSwitchBuilderImpl<TKey, TValue> : BaseSwitchBuilder<TKey, TValue>
        where TKey : struct, IEquatable<TKey>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructSwitchBuilderImpl{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="cases">The cases.</param>
        public StructSwitchBuilderImpl(IReadOnlyDictionary<TKey, TValue> cases)
            : base(cases)
        { }

        #endregion
    }
}
