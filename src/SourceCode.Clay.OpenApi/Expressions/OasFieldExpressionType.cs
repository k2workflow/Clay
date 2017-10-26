#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents the different expression types.
    /// </summary>
#pragma warning disable CA1028 // Enum Storage should be Int32

    public enum OasFieldExpressionType : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        /// <summary>
        /// The expression retrieves a value from a URL.
        /// </summary>
        Url = 0,

        /// <summary>
        /// The expression retrieves a value from the method.
        /// </summary>
        Method = 1,

        /// <summary>
        /// The expression retrieves a value from the status code.
        /// </summary>
        StatusCode = 2,

        /// <summary>
        /// The expression retrieves a value from the request.
        /// </summary>
        Request = 3,

        /// <summary>
        /// The expression retrieves a value from the response.
        /// </summary>
        Response = 4
    }
}
