#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;
using System.Json;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///
    /// </summary>
    internal interface IHasVendorExtensions
    {
        #region Properties

        /// <summary>
        /// Gets the collection of vendor extensions.
        /// </summary>
        IReadOnlyDictionary<string, JsonValue> VendorExtensions { get; }

        #endregion
    }
}
