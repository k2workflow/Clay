#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests
{
    public static class DtoTests
    {
        #region Methods

        private static void TestDtoEquality<T>(Expression<Func<T>> constructor)
            where T : IEquatable<T>
        {
            Assert.Equal(ExpressionType.New, constructor.Body.NodeType);
            var newExpression = (NewExpression)constructor.Body;

            var p1 = Expression.Parameter(typeof(T), "p1");
            var p2 = Expression.Parameter(typeof(T), "p2");
            var eq = Expression.Lambda<Func<T, T, bool>>(Expression.Equal(p1, p2), p1, p2).Compile();
            var neq = Expression.Lambda<Func<T, T, bool>>(Expression.NotEqual(p1, p2), p1, p2).Compile();

            var ctor = newExpression.Constructor;
            var constructorArguments = newExpression.Arguments;
            var ctorParams = ctor.GetParameters();

            var param = new Expression[ctorParams.Length];
            var allValues = constructor.Compile()();
            var hs = new HashSet<T>();

            for (var i = -1; i < constructorArguments.Count; i++)
            {
                for (var j = 0; j < constructorArguments.Count; j++)
                {
                    param[j] = i != j
                        ? ExtractActualArg(constructorArguments[j])
                        : Expression.Default(ctorParams[j].ParameterType);
                }

                var nw = Expression.New(ctor, param);
                var lambda = Expression.Lambda<Func<T>>(nw).Compile();

                var current = lambda();
                hs.Add(current);
                if (i == -1)
                {
                    Assert.Equal(allValues, current);
                    Assert.NotEqual(default, current);

                    Assert.True(eq(allValues, current));
                    Assert.True(neq(default, current));

                    Assert.False(neq(allValues, current));
                    Assert.False(eq(default, current));
                }
                else
                {
                    Assert.Equal(current, current);
                    Assert.NotEqual(default, current);
                    Assert.NotEqual(allValues, current);

                    Assert.True(eq(current, current));
                    Assert.True(neq(default, current));
                    Assert.True(neq(allValues, current));

                    Assert.False(neq(current, current));
                    Assert.False(eq(default, current));
                    Assert.False(eq(allValues, current));
                }
            }

            Assert.Equal(constructorArguments.Count + 1, hs.Count);
        }

        private static Expression ExtractActualArg(Expression arg)
        {
            while (arg.CanReduce) arg = arg.Reduce();

            if (arg is LambdaExpression l) arg = l.Body;
            if (arg is UnaryExpression u && u.NodeType == ExpressionType.Convert && u.Type == typeof(object)) arg = u.Operand;

            return arg;
        }

        #endregion

        #region Equals

        [Fact(DisplayName = nameof(ApiSecurityScheme_Equals))]
        public static void ApiSecurityScheme_Equals()
        {
            TestDtoEquality(() => new ApiKeySecurityScheme(
                "Description",
                "Name",
                ParameterLocation.Header
            ));
        }

        [Fact(DisplayName = nameof(Callback_Equals))]
        public static void Callback_Equals()
        {
            var cb1 = new Callback();
            var cb2 = new Callback(new Dictionary<CompoundExpression, Referable<Path>>()
            {
                [CompoundExpression.Parse("http://test/{$statusCode}")] = new Referable<Path>("#/test"),
                [CompoundExpression.Parse("http://test/1/{$statusCode}")] = new Referable<Path>("#/test/1"),
            });
            var cb3 = new Callback(new Dictionary<CompoundExpression, Referable<Path>>()
            {
                [CompoundExpression.Parse("http://test/{$statusCode}")] = new Referable<Path>("#/test"),
                [CompoundExpression.Parse("http://test/1/{$statusCode}")] = new Referable<Path>("#/test/1"),
            });
            var cb4 = new Callback(new Dictionary<CompoundExpression, Referable<Path>>()
            {
                [CompoundExpression.Parse("http://test/1/{$statusCode}")] = new Referable<Path>("#/test/1"),
            });

            Assert.False(cb1 == null);
            Assert.True(cb2 == cb3);
            Assert.False(cb1 == cb2);
            Assert.False(cb3 == cb4);
        }

        [Fact(DisplayName = nameof(Contact_Equals))]
        public static void Contact_Equals()
        {
            TestDtoEquality(() => new Contact(
                "Jonathan",
                new Uri("http://example.org/jonathan"),
                new MailAddress("jonathan@example.org")
            ));
        }

        [Fact(DisplayName = nameof(Document_Equals))]
        public static void Document_Equals()
        {
            var paths = new Dictionary<string, Referable<Path>>()
            {
                ["/test"] = "#/components/path0"
            };

            var schemas = new Dictionary<string, Referable<Schema>>()
            {
                ["schema1"] = "http://example.org/schema1"
            };

            TestDtoEquality(() => new Document(
                new SemanticVersion(1, 0, 0),
                new Information("Example API", default, default, default, default, default),
                new[] { new Server(default, "Server 1", default) },
                paths,
                new Components(schemas, default, default, default, default, default, default, default, default),
                new[] { new Referable<SecurityScheme>(new HttpSecurityScheme("Http", default, default)) },
                new[] { new Tag("Tag1", default, default) },
                new ExternalDocumentation("External docs", default)
            ));
        }

        [Fact(DisplayName = nameof(Example_Equals))]
        public static void Example_Equals()
        {
            TestDtoEquality(() => new Example(
                "Summary",
                "Description",
                new Uri("http://example.org/docs")
            ));
        }

        [Fact(DisplayName = nameof(ExternalDocumentation_Equals))]
        public static void ExternalDocumentation_Equals()
        {
            TestDtoEquality(() => new ExternalDocumentation(
                "Description",
                new Uri("http://example.org/docs")
            ));
        }

        [Fact(DisplayName = nameof(HttpSecurityScheme_Equals))]
        public static void HttpSecurityScheme_Equals()
        {
            TestDtoEquality(() => new HttpSecurityScheme(
                "Description",
                "Scheme",
                "Bearer Format"
            ));
        }

        [Fact(DisplayName = nameof(Information_Equals))]
        public static void Information_Equals()
        {
            TestDtoEquality(() => new Information(
                "Title",
                "Description",
                new Uri("http://example.org/tos"),
                new Contact("Name", default, default),
                new License("MIT", default),
                new SemanticVersion(1, 0, 0)
            ));
        }

        [Fact(DisplayName = nameof(License_Equals))]
        public static void License_Equals()
        {
            TestDtoEquality(() => new License(
                "MIT",
                new Uri("https://opensource.org/licenses/MIT")
            ));
        }

        [Fact(DisplayName = nameof(Link_Equals))]
        public static void Link_Equals()
        {
            TestDtoEquality(() => new Link(
                "http://example.org/linkOp",
                "Operation ID",
                "Description",
                new Server(default, "Description", default)
            ));
        }

        [Fact(DisplayName = nameof(MediaType_Equals))]
        public static void MediaType_Equals()
        {
            var examples = new Dictionary<string, Referable<Example>>()
            {
                ["example1"] = "#/components/examples/example1"
            };

            var encoding = new Dictionary<string, PropertyEncoding>()
            {
                ["application/json"] = new PropertyEncoding(
                    new System.Net.Mime.ContentType("application/json"),
                    default,
                    default,
                    default
                )
            };

            TestDtoEquality(() => new MediaType(
                "#/components/schemas/schema1",
                examples,
                encoding
            ));
        }

        [Fact(DisplayName = nameof(OAuth2SecurityScheme_Equals))]
        public static void OAuth2SecurityScheme_Equals()
        {
            TestDtoEquality(() => new OAuth2SecurityScheme(
                "Description",
                new OAuthFlow(new Uri("http://example.org/implicit"), default, default, default),
                new OAuthFlow(new Uri("http://example.org/password"), default, default, default),
                new OAuthFlow(new Uri("http://example.org/credential"), default, default, default),
                new OAuthFlow(new Uri("http://example.org/authorization"), default, default, default)
            ));
        }

        [Fact(DisplayName = nameof(OAuth2Flow_Equals))]
        public static void OAuth2Flow_Equals()
        {
            var scopes = new Dictionary<string, string>()
            {
                ["user:name"] = "Get the user name"
            };

            TestDtoEquality(() => new OAuthFlow(
                new Uri("http://example.org/authorization"),
                new Uri("http://example.org/token"),
                new Uri("http://example.org/refresh"),
                scopes
            ));
        }

        [Fact(DisplayName = nameof(OpenIdConnectSecurityScheme_Equals))]
        public static void OpenIdConnectSecurityScheme_Equals()
        {
            TestDtoEquality(() => new OpenIdConnectSecurityScheme(
                "Description",
                new Uri("http://example.org/openid")
            ));
        }

        [Fact(DisplayName = nameof(Operation_Equals))]
        public static void Operation_Equals()
        {
            var param = new Dictionary<ParameterKey, Referable<ParameterBody>>()
            {
                [new ParameterKey("param1", ParameterLocation.Header)] = "#/components/parameters/param1"
            };

            var responses = new Dictionary<ResponseKey, Referable<Response>>()
            {
                [HttpStatusCode.InternalServerError] = "#/components/responses/error"
            };

            var callbacks = new Dictionary<string, Referable<Callback>>()
            {
                ["result"] = "#/components/callbacks/cb1"
            };

            TestDtoEquality(() => new Operation(
                new[] { "Tag1" },
                "Summary",
                "Description",
                new ExternalDocumentation("Description", default),
                "Operation 1",
                param,
                "#/components/requestBodies/rb1",
                responses,
                callbacks,
                OperationOptions.Deprecated,
                new[] { new HttpSecurityScheme("Description", default, default) },
                new[] { new Server(new Uri("http://example.org"), default, default) }
            ));
        }

        [Fact(DisplayName = nameof(Parameter_Equals))]
        public static void Parameter_Equals()
        {
            var examples = new Dictionary<ContentType, Referable<Example>>()
            {
                [new ContentType("application/json")] = "#/components/examples/example1"
            };

            var content = new Dictionary<ContentType, MediaType>()
            {
                [new ContentType("application/json")] = new MediaType(
                    "#/components/schemas/schema1",
                    default,
                    default
                )
            };

            TestDtoEquality(() => new Parameter(
                "Name",
                ParameterLocation.Header,
                "Description",
                ParameterOptions.AllowReserved,
                ParameterStyle.PipeDelimited,
                "#/components/schemas/schema1",
                examples,
                content
            ));
        }

        [Fact(DisplayName = nameof(ParameterBody_Equals))]
        public static void ParameterBody_Equals()
        {
            var examples = new Dictionary<ContentType, Referable<Example>>()
            {
                [new ContentType("application/json")] = "#/components/examples/example1"
            };

            var content = new Dictionary<ContentType, MediaType>()
            {
                [new ContentType("application/json")] = new MediaType(
                    "#/components/schemas/schema1",
                    default,
                    default
                )
            };

            TestDtoEquality(() => new ParameterBody(
                "Description",
                ParameterOptions.AllowReserved,
                ParameterStyle.PipeDelimited,
                "#/components/schemas/schema1",
                examples,
                content
            ));
        }

        [Fact(DisplayName = nameof(Path_Equals))]
        public static void Path_Equals()
        {
            var param = new Dictionary<ParameterKey, Referable<ParameterBody>>()
            {
                [new ParameterKey("Param1", default)] = "#/components/parameters/param1"
            };

            TestDtoEquality(() => new Path(
                "Summary",
                "Description",
                new Operation(default, "Get", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Put", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Post", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Delete", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Options", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Head", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Patch", default, default, default, default, default, default, default, default, default, default),
                new Operation(default, "Trace", default, default, default, default, default, default, default, default, default, default),
                new[] { new Server(new Uri("http://example.org"), default, default) },
                param
            ));
        }

        [Fact(DisplayName = nameof(PropertyEncoding_Equals))]
        public static void PropertyEncoding_Equals()
        {
            var headers = new Dictionary<string, Referable<ParameterBody>>()
            {
                ["header1"] = "#/components/parameters/param1"
            };

            TestDtoEquality(() => new PropertyEncoding(
                new ContentType("application/json"),
                headers,
                ParameterStyle.PipeDelimited,
                PropertyEncodingOptions.Explode
            ));
        }

        [Fact(DisplayName = nameof(RequestBody_Equals))]
        public static void RequestBody_Equals()
        {
            var content = new Dictionary<ContentType, MediaType>()
            {
                [new ContentType("application/json")] = new MediaType("#/components/schemas/schema1")
            };

            TestDtoEquality(() => new RequestBody(
                "Description",
                content,
                RequestBodyOptions.Required
            ));
        }

        [Fact(DisplayName = nameof(Response_Equals))]
        public static void Response_Equals()
        {
            var headers = new Dictionary<string, Referable<ParameterBody>>()
            {
                ["header1"] = "#/components/parameters/param1"
            };

            var content = new Dictionary<ContentType, MediaType>()
            {
                [new ContentType("application/json")] = new MediaType("#/components/schemas/schema1")
            };

            var links = new Dictionary<string, Referable<Link>>()
            {
                ["link"] = "#/components/links/link1"
            };

            TestDtoEquality(() => new Response(
                "Description",
                headers,
                content,
                links
            ));
        }

        [Fact(DisplayName = nameof(Schema_Equals))]
        public static void Schema_Equals()
        {
            var properties = new Dictionary<string, Referable<Schema>>()
            {
                ["prop1"] = "#/components/schemas/prop1"
            };

            var additionalProperties = new Dictionary<string, Referable<Schema>>()
            {
                ["prop1"] = "#/components/schemas/adprop1"
            };

            TestDtoEquality(() => new Schema(
                SchemaType.String,
                "uuid",
                "Unique Identifier",
                "Description",
                new NumberRange(1, 100),
                new CountRange(2, 200),
                new CountRange(3, 300),
                new CountRange(4, 400),
                SchemaOptions.Required,
                "[a-z]",
                new[] { new ScalarValue(true) },
                new Referable<Schema>[] { "#/components/schemas/allOf" },
                new Referable<Schema>[] { "#/components/schemas/oneOf" },
                new Referable<Schema>[] { "#/components/schemas/anyOf" },
                new Referable<Schema>[] { "#/components/schemas/not" },
                "#/components/schemas/items",
                properties,
                additionalProperties,
                new ExternalDocumentation("Description", default)
            ));
        }

        [Fact(DisplayName = nameof(Server_Equals))]
        public static void Server_Equals()
        {
            var variables = new Dictionary<string, ServerVariable>()
            {
                ["var1"] = new ServerVariable(description: "var1")
            };

            TestDtoEquality(() => new Server(
                new Uri("http://example.org"),
                "Description",
                variables
            ));
        }

        [Fact(DisplayName = nameof(ServerVariable_Equals))]
        public static void ServerVariable_Equals()
        {
            TestDtoEquality(() => new ServerVariable(
                new[] { "Val1" },
                "Val1",
                "Variable1"
            ));
        }

        [Fact(DisplayName = nameof(Tag_Equals))]
        public static void Tag_Equals()
        {
            TestDtoEquality(() => new Tag(
                "Name",
                "Description",
                new ExternalDocumentation("Description", default)
            ));
        }

        #endregion
    }
}
