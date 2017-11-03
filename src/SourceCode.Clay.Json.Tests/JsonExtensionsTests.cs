#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class JsonExtensionTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJObject_Null))]
        public static void When_ReadOnlyJObject_Null()
        {
            ReadOnlyJObject json = null;

            // Repros for: https://github.com/dotnet/corefx/issues/25005
            //var a = new JArray(new JToken[] { "abc", 123, null });
            //var b = new JArray(new JToken[] { "abc", 123 }); b.Add(null);
            //var x = new JArray(new JToken[] { "abc", 123 }); x.Add(new JValue((string)null));
            //var p = new JValue(Guid.NewGuid());
            //var t = p.Type;
            //var s = p.ToString();

            Assert.True(json == null);
            Assert.False(json != null);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJObject_Empty))]
        public static void When_ReadOnlyJObject_Empty()
        {
            var json = new ReadOnlyJObject();

            Assert.True(json != null);

            Assert.False(json.Equals(null));
            Assert.False(json == null);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_JObject_Parse))]
        public static void When_JObject_Parse()
        {
            var arr = new JArray(new[] { new JValue("hi"), new JValue(456), new JValue(false), null });
            var expected = new JObject { ["Foo"] = arr, ["Bar"] = 123 };

            var actual = expected.ToString().ParseJObject();
            Assert.Equal(expected.ToString(), actual.ToString());

            var ar1 = expected.GetArray("Foo");
            Assert.Equal(arr.Count, ar1.Count);

            var ar2 = ar1.ToString().ParseJArray();
            Assert.Equal(ar1.ToString(), ar2.ToString());

            var pr1 = expected.GetPrimitive("Bar");
            Assert.Equal(new JValue(123), pr1);

            var pr2 = pr1.ToString().ParseJValue();
            Assert.Equal(pr1.ToString(), pr2.ToString());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJObject_Parse))]
        public static void When_ReadOnlyJObject_Parse()
        {
            var arr = new JArray(new[] { new JValue("hi"), new JValue(456), new JValue(false), null });
            var exp = new JObject { ["Foo"] = arr, ["Bar"] = 123 };
            var expected = new ReadOnlyJObject(exp);

            var actual = new ReadOnlyJObject(expected.ToString().ParseJObject());
            Assert.Equal(expected, actual, JObjectComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_JObject_TryGetValue))]
        public static void When_JObject_TryGetValue()
        {
            var json = new JObject
            {
                ["bool"] = new JValue(true),
                ["int"] = new JValue(123),
                ["string"] = new JValue("hello"),
                ["object"] = new JObject(),
                ["array"] = new JArray()
            };

            Assert.False(json.Equals(null));
            Assert.True(json != null);

            var clone = (JObject)json.DeepClone();
            Assert.Equal(json.Count, clone.Count);
            Assert.Equal(json.ToString(), clone.ToString());

            Assert.True(json["bool"].Type == JTokenType.Boolean);
            Assert.True(json.TryGetValue("bool", JTokenType.Boolean, false, out var jv) && (bool)jv);

            Assert.True(json["int"].Type == JTokenType.Integer);
            Assert.True(json.TryGetValue("int", JTokenType.Integer, false, out jv) && jv.Value<int>() == 123);

            Assert.True(json["string"].Type == JTokenType.String);
            Assert.True(json.TryGetValue("string", JTokenType.String, false, out jv) && jv.Value<string>() == "hello");

            Assert.True(json["object"].Type == JTokenType.Object);
            Assert.True(json.TryGetObject("object", out var jo));

            Assert.True(json["array"].Type == JTokenType.Array);
            Assert.True(json.TryGetArray("array", out var ja));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJObject_TryGetValue))]
        public static void When_ReadOnlyJObject_TryGetValue()
        {
            var jobj = new JObject
            {
                ["bool"] = new JValue(true),
                ["int"] = new JValue(123),
                ["string"] = new JValue("hello"),
                ["object"] = new JObject { ["dt"] = "abc" },
                ["array"] = new JArray(new JToken[] { new JValue(123.456), new JObject { ["dto"] = 456 } })
            };
            var json = new ReadOnlyJObject(jobj);

            Assert.False(json.Equals(null));
            Assert.True(json != null);
            Assert.Equal(jobj.Count, json.Count);
            Assert.Equal(jobj.ToString(), json.ToString());

            var clone = json.DeepClone();
            Assert.Equal(json, clone, JObjectComparer.Default);

            Assert.Equal(json.Count, clone.Count);
            Assert.Equal(json.ToString(), clone.ToString());

            var mutable = json.ToJObject();
            Assert.Equal(json.ToString(), mutable.ToString());

            Assert.True(json["bool"].Type == JTokenType.Boolean);
            Assert.True(json.GetValue("bool", JTokenType.Boolean, false).Type == JTokenType.Boolean);
            Assert.True(json.TryGetValue("bool", out var jv) && (bool)jv);
            Assert.True(json.TryGetValue("bool", JTokenType.Boolean, false, out jv) && (bool)jv);

            Assert.True(json["int"].Type == JTokenType.Integer);
            Assert.True(json.GetValue("int", JTokenType.Integer, false).Type == JTokenType.Integer);
            Assert.True(json.TryGetValue("int", out jv) && jv.Value<int>() == 123);
            Assert.True(json.TryGetValue("int", JTokenType.Integer, false, out jv) && jv.Value<int>() == 123);

            Assert.True(json["string"].Type == JTokenType.String);
            Assert.True(json.GetValue("string", JTokenType.String, false).Type == JTokenType.String);
            Assert.True(json.TryGetValue("string", out jv) && jv.Value<string>() == "hello");
            Assert.True(json.TryGetValue("string", JTokenType.String, false, out jv) && jv.Value<string>() == "hello");

            Assert.True(json["object"].Type == JTokenType.Object);
            Assert.True(json.GetObject("object").Type == JTokenType.Object);
            Assert.True(json.TryGetObject("object", out var jo));

            Assert.True(json["array"].Type == JTokenType.Array);
            Assert.True(json.GetArray("array").Type == JTokenType.Array);
            Assert.True(json.TryGetArray("array", out var ja));

            Assert.False(json.TryGetArray("dne", out ja));
            Assert.False(json.TryGetObject("dne", out jo));
            Assert.False(json.TryGetValue("dne", out jv));
        }

        #endregion
    }
}
