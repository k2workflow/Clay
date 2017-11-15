#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;

namespace SourceCode.Clay.Json
{
    public static class NumberExtensions
    {
        #region Methods

        public static JValue ToJson(Number number)
        {
            // We constrain to number types (not bool, guid, string, etc)
            switch (number.ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return new JValue(number.SByte);
                case TypeCode.Int16: return new JValue(number.Int16);
                case TypeCode.Int32: return new JValue(number.Int32);
                case TypeCode.Int64: return new JValue(number.Int64);

                // Unsigned
                case TypeCode.Byte: return new JValue(number.Byte);
                case TypeCode.UInt16: return new JValue(number.UInt16);
                case TypeCode.UInt32: return new JValue(number.UInt32);
                case TypeCode.UInt64: return new JValue(number.UInt64);

                // Numeric
                case TypeCode.Single: return new JValue(number.Single);
                case TypeCode.Double: return new JValue(number.Double);
                case TypeCode.Decimal: return new JValue(number.Decimal);

                default:
                    throw new InvalidCastException($"Unexpected {nameof(Number)} {nameof(Type)}");
            }
        }

        public static Number ToNumber(this JValue value)
        {
            if (value == null) return default;

            if (value.Type == JTokenType.Integer)
            {
                var val = value.Value<long>();
                var number = new Number(val);
                return number;
            }

            if (value.Type == JTokenType.Float)
            {
                var val = value.Value<double>();
                var number = new Number(val);
                return number;
            }

            throw new ArgumentOutOfRangeException(nameof(value));
        }

        #endregion
    }
}
