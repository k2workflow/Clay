// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace SourceCode.Clay.Algorithms
{
    /// <summary>
    /// 
    /// </summary>
    internal class HuffmanDecodingException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HuffmanDecodingException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public HuffmanDecodingException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public HuffmanDecodingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}