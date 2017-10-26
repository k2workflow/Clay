#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents expression sources.
    /// </summary>
    public enum OasFieldExpressionSource
    {
        /// <summary>
        /// The expression is simple and has no source parameter.
        /// </summary>
        None = 0,

        /// <summary>
        /// The expression extracts a value from the header.
        /// </summary>
        Header = 1,

        /// <summary>
        /// The expression extracts a value from the query.
        /// </summary>
        Query = 2,

        /// <summary>
        /// The expression extracts a value from the path.
        /// </summary>
        Path = 3,

        /// <summary>
        /// The expression extracts a value from the body.
        /// </summary>
        Body = 4
    }
}
