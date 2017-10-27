#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents the different type of expression components.
    /// </summary>
    public enum OasExpressionComponentType
    {
        /// <summary>
        /// A literal value.
        /// </summary>
        Literal = 0,

        /// <summary>
        /// A field getter.
        /// </summary>
        Field = 1
    }
}
