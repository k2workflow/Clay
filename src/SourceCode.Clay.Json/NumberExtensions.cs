#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace SourceCode.Clay.Json
{
    public static class NumberExtensions
    {
        #region Helpers

        private static readonly Func<JsonPrimitive, object> GetValueFromPrimitive = CreateGetValueFromPrimitive();

        private static Func<JsonPrimitive, object> CreateGetValueFromPrimitive()
        {
            var prim = typeof(JsonPrimitive);
            var prop = prim.GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance);

            var param = Expression.Parameter(prim, "primitive");
            var expr = Expression.Property(param, prop);

            var func = Expression.Lambda<Func<JsonPrimitive, object>>(expr, param);
            return func.Compile();
        }

        #endregion

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

                default: return default;
            }
        }

        public static Number ToNumber(this JsonValue value)
        {
            if (value == null) return default;

            if (value.JsonType != JsonType.Number) throw new ArgumentOutOfRangeException(nameof(value));

            var primitive = (JsonPrimitive)value;

            var obj = GetValueFromPrimitive(primitive);
            if (obj is Decimal d)
                obj = (double)d;

            var num = Number.CreateFromObject(obj);
            return num;
        }

        #endregion
    }
}
