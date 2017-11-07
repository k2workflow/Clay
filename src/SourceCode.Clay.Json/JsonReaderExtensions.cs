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
        #region Methods

        /// <summary>
        /// Reads the current token value as a string, then converts it to a <see cref="Enum"/>.
        /// Returns null if the Json value is null, or the string value is <see langword="null"/> or <see cref="string.Empty"/>.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert to.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="ignoreCase">True to ignore case; False to regard case.</param>
        /// <returns>The parsed enum value or null.</returns>
        public static TEnum? ReadEnum<TEnum>(this JsonReader jr, bool ignoreCase)
            where TEnum : struct
        {
            var str = (string)jr.Value;
            if (string.IsNullOrEmpty(str) || str == JsonConstants.JsonNull)
                return null;

            var knd = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
            return knd;
        }

        #endregion
    }
}
