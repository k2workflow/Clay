#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace SourceCode.Clay.OpenApi.Serialization
{
    internal static class NumberExtensions
    {
        #region Fields

        private static readonly Func<JsonPrimitive, object> GetValueFromPrimitive = CreateGetValueFromPrimitive();

        #endregion

        #region Methods

        private static Func<JsonPrimitive, object> CreateGetValueFromPrimitive()
        {
            var t = typeof(JsonPrimitive);
            var param = Expression.Parameter(t, "primitive");
            var prop = t.GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance);
            var expr = Expression.Property(param, prop);
            return Expression.Lambda<Func<JsonPrimitive, object>>(expr, param).Compile();
        }

        public static JsonValue ToValue(this Number number)
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
#               pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                default: return null;
#               pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }
        }

        public static Number ToNumber(this JsonValue value)
        {
            if (value == null) return default;
            if (value.JsonType != JsonType.Number) throw new ArgumentOutOfRangeException(nameof(value));
            var primitive = (JsonPrimitive)value;
            var val = GetValueFromPrimitive(primitive);
            if (val is Decimal d) val = (double)d;
            return Number.CreateFromObject(val);
        }

        #endregion
    }
}
