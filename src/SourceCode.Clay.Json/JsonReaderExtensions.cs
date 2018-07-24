#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using System;

namespace SourceCode.Clay.Json
{
    public static partial class JsonReaderExtensions
    {
        /// <summary>
        /// Reads the current token value as a string, then converts it to a <see cref="Enum"/>.
        /// Returns null if the Json value is null, or the string value is <see langword="null"/> or <see cref="string.Empty"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert to.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="ignoreCase">True to ignore case; False to regard case.</param>
        /// <returns>The parsed enum value or null.</returns>
        public static TEnum? ParseEnum<TEnum>(this JsonReader jr, bool ignoreCase)
            where TEnum : struct
        {
            if (jr.TokenType == JsonToken.Null)
                return null;

            var str = (string)jr.Value;
            if (string.IsNullOrEmpty(str))
                return null;

            var enm = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
            return enm;
        }

        /// <summary>
        /// Reads the current token value as a string, then converts it to a nullable <see cref="Guid"/>.
        /// Returns null if the Json value is null, or the string value is <see langword="null"/> or <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The parsed guid value or null.</returns>
        public static Guid? ParseGuid(this JsonReader jr)
        {
            if (jr.TokenType == JsonToken.Null)
                return null;

            var str = (string)jr.Value;
            if (string.IsNullOrEmpty(str))
                return null;

            var guid = Guid.Parse(str);
            return guid;
        }

        /// <summary>
        /// Reads the current token value as a string, then converts it to a nullable <see cref="Guid"/>.
        /// Returns null if the Json value is null, or the string value is <see langword="null"/> or <see cref="string.Empty"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="format">Specifier indicating the exact format to use when interpreting the input.</param>
        /// <returns>The parsed guid value or null.</returns>
        public static Guid? ParseGuidExact(this JsonReader jr, string format)
        {
            if (jr.TokenType == JsonToken.Null)
                return null;

            var str = (string)jr.Value;
            if (string.IsNullOrEmpty(str))
                return null;

            var guid = Guid.ParseExact(str, format);
            return guid;
        }
    }
}
