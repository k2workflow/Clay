#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Json;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class JsonExtensionTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJsonObject_Null))]
        public static void When_ReadOnlyJsonObject_Null()
        {
            ReadOnlyJsonObject json = null;

            // Repros for: https://github.com/dotnet/corefx/issues/25005
            //var a = new JsonArray(new JsonValue[] { "abc", 123, null });
            //var b = new JsonArray(new JsonValue[] { "abc", 123 }); b.Add(null);
            //var x = new JsonArray(new JsonValue[] { "abc", 123 }); x.Add(new JsonPrimitive((string)null));
            //var p = new JsonPrimitive(Guid.NewGuid());
            //var t = p.JsonType;
            //var s = p.ToString();

            Assert.True(json == null);
            Assert.False(json != null);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJsonObject_Empty))]
        public static void When_ReadOnlyJsonObject_Empty()
        {
            var json = new ReadOnlyJsonObject();

            Assert.True(json != null);

            Assert.False(json.Equals(null));
            Assert.False(json == null);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_JsonObject_Parse))]
        public static void When_JsonObject_Parse()
        {
            var arr = new JsonArray(new[] { new JsonPrimitive("hi"), new JsonPrimitive(456), new JsonPrimitive(false), null });
            var expected = new JsonObject { ["Foo"] = arr, ["Bar"] = 123 };

            var actual = expected.ToString().ParseJsonObject();
            Assert.Equal(expected.ToString(), actual.ToString());

            var ar1 = expected.GetArray("Foo");
            Assert.Equal(arr.Count, ar1.Count);

            var ar2 = ar1.ToString().ParseJsonArray();
            Assert.Equal(ar1.ToString(), ar2.ToString());

            var pr1 = expected.GetPrimitive("Bar");
            Assert.Equal(new JsonPrimitive(123), pr1, JsonValueComparer.Default);

            var pr2 = pr1.ToString().ParseJsonPrimitive();
            Assert.Equal(pr1.ToString(), pr2.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJsonObject_Parse))]
        public static void When_ReadOnlyJsonObject_Parse()
        {
            var arr = new JsonArray(new[] { new JsonPrimitive("hi"), new JsonPrimitive(456), new JsonPrimitive(false), null });
            var exp = new JsonObject { ["Foo"] = arr, ["Bar"] = 123 };
            var expected = new ReadOnlyJsonObject(exp);

            var actual = new ReadOnlyJsonObject(expected.ToString().ParseJsonObject());
            Assert.Equal(expected, actual, JsonValueComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_JsonObject_TryGetValue))]
        public static void When_JsonObject_TryGetValue()
        {
            var json = new JsonObject
            {
                ["bool"] = new JsonPrimitive(true),
                ["int"] = new JsonPrimitive(123),
                ["string"] = new JsonPrimitive("hello"),
                ["object"] = new JsonObject(),
                ["array"] = new JsonArray()
            };

            Assert.False(json.Equals(null));
            Assert.True(json != null);

            var clone = json.Clone();
            Assert.Equal(json.Count, clone.Count);
            Assert.Equal(json.ToString(), clone.ToString());

            Assert.True(json.ContainsKey("bool"));
            Assert.True(json["bool"].JsonType == JsonType.Boolean);
            Assert.True(json.TryGetValue("bool", JsonType.Boolean, false, out var jv) && (bool)jv);

            Assert.True(json.ContainsKey("int"));
            Assert.True(json["int"].JsonType == JsonType.Number);
            Assert.True(json.TryGetValue("int", JsonType.Number, false, out jv) && jv == 123);

            Assert.True(json.ContainsKey("string"));
            Assert.True(json["string"].JsonType == JsonType.String);
            Assert.True(json.TryGetValue("string", JsonType.String, false, out jv) && jv == "hello");

            Assert.True(json.ContainsKey("object"));
            Assert.True(json["object"].JsonType == JsonType.Object);
            Assert.True(json.TryGetObject("object", out var jo));

            Assert.True(json.ContainsKey("array"));
            Assert.True(json["array"].JsonType == JsonType.Array);
            Assert.True(json.TryGetArray("array", out var ja));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJsonObject_TryGetValue))]
        public static void When_ReadOnlyJsonObject_TryGetValue()
        {
            var jobj = new JsonObject
            {
                ["bool"] = new JsonPrimitive(true),
                ["int"] = new JsonPrimitive(123),
                ["string"] = new JsonPrimitive("hello"),
                ["object"] = new JsonObject { ["dt"] = "abc" },
                ["array"] = new JsonArray(new JsonValue[] { new JsonPrimitive(123.456), new JsonObject { ["dto"] = 456 } })
            };
            var json = new ReadOnlyJsonObject(jobj);

            Assert.False(json.Equals(null));
            Assert.True(json != null);
            Assert.Equal(jobj.Count, json.Count);
            Assert.Equal(jobj.ToString(), json.ToString());

            var clone = json.Clone();
            Assert.Equal(json, clone, JsonValueComparer.Default);

            Assert.Equal(json.Count, clone.Count);
            Assert.Equal(json.Keys.Count(), clone.Keys.Count());
            Assert.Equal(json.Values.Count(), clone.Values.Count());
            Assert.Equal(json.ToString(), clone.ToString());

            var mutable = json.ToJsonObject();
            Assert.Equal(json.ToString(), mutable.ToString());

            Assert.True(json.ContainsKey("bool"));
            Assert.True(json["bool"].JsonType == JsonType.Boolean);
            Assert.True(json.GetValue("bool", JsonType.Boolean, false).JsonType == JsonType.Boolean);
            Assert.True(json.TryGetValue("bool", out var jv) && (bool)jv);
            Assert.True(json.TryGetValue("bool", JsonType.Boolean, false, out jv) && (bool)jv);

            Assert.True(json.ContainsKey("int"));
            Assert.True(json["int"].JsonType == JsonType.Number);
            Assert.True(json.GetValue("int", JsonType.Number, false).JsonType == JsonType.Number);
            Assert.True(json.TryGetValue("int", out jv) && jv == 123);
            Assert.True(json.TryGetValue("int", JsonType.Number, false, out jv) && jv == 123);

            Assert.True(json.ContainsKey("string"));
            Assert.True(json["string"].JsonType == JsonType.String);
            Assert.True(json.GetValue("string", JsonType.String, false).JsonType == JsonType.String);
            Assert.True(json.TryGetValue("string", out jv) && jv == "hello");
            Assert.True(json.TryGetValue("string", JsonType.String, false, out jv) && jv == "hello");

            Assert.True(json.ContainsKey("object"));
            Assert.True(json["object"].JsonType == JsonType.Object);
            Assert.True(json.GetObject("object").JsonType == JsonType.Object);
            Assert.True(json.TryGetObject("object", out var jo));

            Assert.True(json.ContainsKey("array"));
            Assert.True(json["array"].JsonType == JsonType.Array);
            Assert.True(json.GetArray("array").JsonType == JsonType.Array);
            Assert.True(json.TryGetArray("array", out var ja));

            Assert.False(json.TryGetArray("dne", out ja));
            Assert.False(json.TryGetObject("dne", out jo));
            Assert.False(json.TryGetValue("dne", out jv));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_JsonPrimitive_Equal))]
        public static void When_JsonPrimitive_Equal()
        {
            var primitives = new[]
            {
                new JsonPrimitive(false),
                new JsonPrimitive(true),

                new JsonPrimitive(new Uri("http://www.k2.com")),

                new JsonPrimitive(TimeSpan.MinValue),
                new JsonPrimitive(TimeSpan.Zero),
                new JsonPrimitive(TimeSpan.MaxValue),

                new JsonPrimitive(Guid.Empty),
                new JsonPrimitive(Guid.NewGuid()),

                new JsonPrimitive(DateTime.MinValue),
                new JsonPrimitive(DateTime.Now),
                new JsonPrimitive(DateTime.MaxValue),

                new JsonPrimitive(DateTimeOffset.MinValue),
                new JsonPrimitive(DateTimeOffset.Now),
                new JsonPrimitive(DateTimeOffset.MaxValue),

                new JsonPrimitive(byte.MinValue),
                new JsonPrimitive(byte.MaxValue),

                new JsonPrimitive(ushort.MinValue),
                new JsonPrimitive(ushort.MaxValue),

                new JsonPrimitive(ulong.MinValue),
                new JsonPrimitive(ulong.MaxValue),

                new JsonPrimitive(uint.MinValue),
                new JsonPrimitive(uint.MaxValue),

                new JsonPrimitive(sbyte.MinValue),
                new JsonPrimitive(sbyte.MaxValue),

                new JsonPrimitive(short.MinValue),
                new JsonPrimitive((short)0),
                new JsonPrimitive(short.MaxValue),

                new JsonPrimitive(long.MinValue),
                new JsonPrimitive((long)0),
                new JsonPrimitive(long.MaxValue),

                new JsonPrimitive(int.MinValue),
                new JsonPrimitive(0),
                new JsonPrimitive(int.MaxValue),

                new JsonPrimitive(double.MinValue),
                new JsonPrimitive((double)0),
                new JsonPrimitive(double.MaxValue),

                new JsonPrimitive(decimal.MinValue),
                new JsonPrimitive(decimal.Zero),
                new JsonPrimitive(decimal.MaxValue),

                new JsonPrimitive(float.MinValue),
                new JsonPrimitive((float)0),
                new JsonPrimitive(float.MaxValue),

                new JsonPrimitive(char.MinValue),
                new JsonPrimitive(char.MaxValue),

                new JsonPrimitive(string.Empty),
                new JsonPrimitive("hi")
            };

            for (var i = 0; i < primitives.Length; i++)
            {
                var ic = (JsonPrimitive)primitives[i].Clone();

                for (var j = 0; j < primitives.Length; j++)
                {
                    var jc = (JsonPrimitive)primitives[j].Clone();

                    if (i == j)
                    {
                        Assert.Equal(ic, jc, JsonValueComparer.Default);
                        Assert.Equal(primitives[i], primitives[j], JsonValueComparer.Default);
                    }
                    else
                    {
                        Assert.NotEqual(ic, jc, JsonValueComparer.Default);
                        Assert.NotEqual(primitives[i], primitives[j], JsonValueComparer.Default);
                    }
                }
            }
        }

        #endregion
    }
}
