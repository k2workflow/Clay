#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace SourceCode.Clay.Json
{
    public static partial class JsonReaderExtensions
    {
        /// <summary>
        /// Reads the current token value as a string, then converts it to a <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert to.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="ignoreCase">True to ignore case; False to regard case.</param>
        /// <returns>The parsed enum value or null.</returns>
        public static TEnum AsEnum<TEnum>(this JsonReader jr, bool ignoreCase)
            where TEnum : struct, Enum
        {
            string str = (string)jr.Value;
            TEnum enm = Enum.Parse<TEnum>(str, ignoreCase);
            return enm;
        }

        /// <summary>
        /// Reads the current token value as a string, then converts it to a nullable <see cref="Enum"/>.
        /// Returns <see langword="null"/> if the Json value is null, or the string value is null or empty.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert to.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="ignoreCase">True to ignore case; False to regard case.</param>
        /// <returns>The parsed enum value or null.</returns>
        public static TEnum? AsEnumNullable<TEnum>(this JsonReader jr, bool ignoreCase)
            where TEnum : struct, Enum
        {
            if (jr.TokenType == JsonToken.Null)
                return null;

            string str = (string)jr.Value;
            if (str.Length == 0) // We already checked for null
                return null;

            TEnum enm = Enum.Parse<TEnum>(str, ignoreCase);
            return enm;
        }

        /// <summary>
        /// Reads the current token value as a string, then converts it to a <see cref="Guid"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The parsed guid value.</returns>
        public static Guid AsGuid(this JsonReader jr)
        {
            string str = (string)jr.Value;
            var guid = Guid.Parse(str);
            return guid;
        }

        /// <summary>
        /// Reads the current token value as a string, then converts it to a nullable <see cref="Guid"/>.
        /// Returns <see langword="null"/> if the Json value is null, or the string value is null or empty.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The parsed guid value or null.</returns>
        public static Guid? AsGuidNullable(this JsonReader jr)
        {
            if (jr.TokenType == JsonToken.Null)
                return null;

            string str = (string)jr.Value;
            if (str.Length == 0) // We already checked for null
                return null;

            var guid = Guid.Parse(str);
            return guid;
        }

        /// <summary>
        /// Reads the current token value as a string, then converts it to a nullable <see cref="Guid"/>.
        /// Returns <see langword="null"/> if the Json value is null, or the string value is null or empty.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="format">Specifier indicating the exact format to use when interpreting the input.</param>
        /// <returns>The parsed guid value or null.</returns>
        public static Guid? AsGuidExact(this JsonReader jr, string format)
        {
            if (jr.TokenType == JsonToken.Null)
                return null;

            string str = (string)jr.Value;
            if (str.Length == 0) // We already checked for null
                return null;

            var guid = Guid.ParseExact(str, format);
            return guid;
        }

        /// <summary>
        /// Reads the current token value as a <see cref="SByte"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte AsSByte(this JsonReader jr)
            => (sbyte)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="SByte"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte? AsSByteNullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (sbyte?)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte AsByte(this JsonReader jr)
            => (byte)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="Byte"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        public static byte? AsByteNullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (byte?)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as an <see cref="Int16"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short AsInt16(this JsonReader jr)
            => (short)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="Int16"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short? AsInt16Nullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (short?)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a <see cref="UInt16"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort AsUInt16(this JsonReader jr)
            => (ushort)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="UInt16"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort? AsUInt16Nullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (ushort?)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as an <see cref="Int32"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsInt32(this JsonReader jr)
            => (int)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="Int32"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? AsInt32Nullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (int?)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as an <see cref="UInt32"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint AsUInt32(this JsonReader jr)
            => (uint)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="UInt32"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint? AsUInt32Nullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (uint?)(long)jr.Value;

        /// <summary>
        /// Reads the current token value as an <see cref="Int64"/>.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long AsInt64(this JsonReader jr)
            => (long)jr.Value;

        /// <summary>
        /// Reads the current token value as a nullable <see cref="Int64"/>.
        /// Returns <see langword="null"/> if the Json value is null.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The integer value or null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long? AsInt64Nullable(this JsonReader jr)
            => jr.TokenType == JsonToken.Null ? null : (long?)(long)jr.Value;
    }
}
