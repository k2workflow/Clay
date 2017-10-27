#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Json;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class JsonExtensionTests
    {
        #region Methods

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

            var clone = json.Clone();
            Assert.Equal(json.ToString(), clone.ToString());

            Assert.True(json.TryGetValue("bool", JsonType.Boolean, false, out var jv) && (bool)jv);
            Assert.True(json.TryGetValue("int", JsonType.Number, false, out jv) && jv == 123);
            Assert.True(json.TryGetValue("string", JsonType.String, false, out jv) && jv == "hello");
            Assert.True(json.TryGetObject("object", out var jo));
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
                ["object"] = new JsonObject(),
                ["array"] = new JsonArray()
            };
            var json = new ReadOnlyJsonObject(jobj);

            Assert.True(json != null);

            var clone = json.Clone();
            Assert.Equal(json, clone);
            Assert.Equal(json.ToString(), clone.ToString());

            var mutable = json.ToJsonObject();
            Assert.Equal(json.ToString(), mutable.ToString());

            Assert.True(json.TryGetValue("bool", JsonType.Boolean, false, out var jv) && (bool)jv);
            Assert.True(json.TryGetValue("int", JsonType.Number, false, out jv) && jv == 123);
            Assert.True(json.TryGetValue("string", JsonType.String, false, out jv) && jv == "hello");
            Assert.True(json.TryGetObject("object", out var jo));
            Assert.True(json.TryGetArray("array", out var ja));
        }

        #endregion
    }
}
