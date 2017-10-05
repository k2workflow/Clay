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
        [Fact(DisplayName = nameof(When_TryGetValue))]
        public static void When_TryGetValue()
        {
            var json = new JsonObject
            {
                ["bool"] = new JsonPrimitive(true),
                ["int"] = new JsonPrimitive(123),
                ["string"] = new JsonPrimitive("hello"),
                ["object"] = new JsonObject(),
                ["array"] = new JsonArray()
            };

            Assert.True(json.TryGetValue("bool", JsonType.Boolean, false, out JsonValue jv) && (bool)jv);
            Assert.True(json.TryGetValue("int", JsonType.Number, false, out jv) && jv == 123);
            Assert.True(json.TryGetValue("string", JsonType.String, false, out jv) && jv == "hello");
            Assert.True(json.TryGetObject("object", out JsonObject jo));
            Assert.True(json.TryGetArray("array", out JsonArray ja));
        }

        #endregion
    }
}
