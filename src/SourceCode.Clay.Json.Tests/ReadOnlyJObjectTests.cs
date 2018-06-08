#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class ReadOnlyJObjectTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_ReadOnlyJObject_Parse))]
        public static void When_ReadOnlyJObject_Parse()
        {
            var arr = new JArray(new[] { new JValue("hi"), new JValue(456), new JValue(false), null });
            var exp = new JObject { ["Foo"] = arr, ["Bar"] = 123 };
            var expected = new ReadOnlyJObject(exp);

            var actual = new ReadOnlyJObject((JObject)JToken.Parse(expected.ToString()));
            Assert.Equal(expected, actual, ReadOnlyJObjectComparer.Default);
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
            Assert.True(json["int"].Type == JTokenType.Integer);
            Assert.True(json["string"].Type == JTokenType.String);
            Assert.True(json["object"].Type == JTokenType.Object);
            Assert.True(json["array"].Type == JTokenType.Array);
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
            Assert.Equal(json, clone, ReadOnlyJObjectComparer.Default);

            Assert.Equal(json.Count, clone.Count);
            Assert.Equal(json.ToString(), clone.ToString());

            var mutable = json.ToJObject();
            Assert.Equal(json.ToString(), mutable.ToString());

            Assert.True(json["bool"].Type == JTokenType.Boolean);
            Assert.True(json.TryGetValue("bool", out var jv) && (bool)jv);

            Assert.True(json["int"].Type == JTokenType.Integer);
            Assert.True(json.TryGetValue("int", out jv) && jv.Value<long>() == 123);

            Assert.True(json["string"].Type == JTokenType.String);
            Assert.True(json.TryGetValue("string", out jv) && jv.Value<string>() == "hello");

            Assert.True(json["object"].Type == JTokenType.Object);

            Assert.True(json["array"].Type == JTokenType.Array);

            Assert.False(json.TryGetValue("dne", out jv));
        }
    }
}
