#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Json;

namespace SourceCode.Clay.Json
{
    public static class NumberExtensions
    {
        #region Methods

        public static JsonPrimitive ToJson(this Number number)
        {
            switch (number.ValueTypeCode)
            {
                case TypeCode.Byte: return new JsonPrimitive(number.Byte);
                case TypeCode.Double: return new JsonPrimitive(number.Double);
                case TypeCode.Int16: return new JsonPrimitive(number.Int16);
                case TypeCode.Int32: return new JsonPrimitive(number.Int32);
                case TypeCode.Int64: return new JsonPrimitive(number.Int64);
                case TypeCode.SByte: return new JsonPrimitive(number.SByte);
                case TypeCode.Single: return new JsonPrimitive(number.Single);
                case TypeCode.UInt16: return new JsonPrimitive(number.UInt16);
                case TypeCode.UInt32: return new JsonPrimitive(number.UInt32);
                case TypeCode.UInt64: return new JsonPrimitive(number.UInt64);
                case TypeCode.Decimal: return new JsonPrimitive(number.Decimal);

                default: return default;
            }
        }

        public static Number ToNumber(this JsonValue value)
        {
            if (value == null) return default;

            if (value.JsonType != JsonType.Number) throw new ArgumentOutOfRangeException(nameof(value));

            var primitive = (JsonPrimitive)value;

            var obj = JsonExtensions.GetValueFromPrimitive(primitive);

            var num = Number.CreateFromObject(obj);
            return num;
        }

        #endregion
    }
}
