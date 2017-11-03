#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json;
using SourceCode.Clay.Json.Validation;
using SourceCode.Clay.OpenApi.Expressions;
using SourceCode.Clay.OpenApi.Tests.Mock;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests
{
    public static class SerializeTests
    {
        #region Helpers

        // Actually check that the values are correct when using this.

#if DEBUG

        private static string GenerateExpectedCode(JToken value)
        {
            using (var sw = new StringWriter())
            using (var iw = new IndentedTextWriter(sw))
            {
                GenerateExpectedCode(value, iw);
                return sw.ToString();
            }
        }

        private static void GenerateExpectedCode(JToken value, IndentedTextWriter writer)
        {
            if (value == null)
            {
                writer.Write("null");
            }
            else if (value is JValue)
            {
                writer.Write(value.ToString());
            }
            else if (value is JArray a)
            {
                if (a.Count == 0)
                    writer.Write("new JArray()");
                else
                {
                    writer.WriteLine("new JArray() {");
                    writer.Indent++;

                    for (var i = 0; i < a.Count; i++)
                    {
                        GenerateExpectedCode(a[i], writer);
                        if (i == a.Count - 1) writer.WriteLine();
                        else writer.WriteLine(",");
                    }

                    writer.Indent--;
                    writer.Write("}");
                }
            }
            else if (value is JObject o)
            {
                if (o.Count == 0)
                    writer.Write("new JObject()");
                else
                {
                    writer.WriteLine("new JObject() {");
                    writer.Indent++;

                    var first = true;
                    foreach (var item in o)
                    {
                        if (!first) writer.WriteLine(",");
                        first = false;

                        writer.Write("[ ");
                        GenerateExpectedCode(item.Key, writer);
                        writer.Write(" ] = ");

                        GenerateExpectedCode(item.Value, writer);
                    }

                    writer.WriteLine();
                    writer.Indent--;
                    writer.Write("}");
                }
            }
        }

#endif

        #endregion

        #region Methods

        [Fact(DisplayName = nameof(OpenApiSerializer_Serialize))]
        public static void OpenApiSerializer_Serialize()
        {
            var sut = new MockOasSerializer();

            #region Graph

            var actualGraphBuilder = new OasDocumentBuilder()
            {
                Components = new OasComponentsBuilder()
                {
                    Callbacks =
                    {
                        [ "callback1" ] = new OasCallbackBuilder()
                        {
                            [ OasExpression.Parse("http://foo{$statusCode}") ] = new OasPathBuilder()
                            {
                                Description = "path1",
                                Delete = new MockOasOperationBuilder()
                                {
                                    OperationId = 1,
                                    Callbacks =
                                    {
                                        [ "callback1" ] = "#/components/callbacks/callback1"
                                    },
                                    Description = "Delete operation",
                                    ExternalDocumentation = new OasExternalDocumentationBuilder()
                                    {
                                        Description = "External docs 1",
                                        Url = new Uri("http://example.org")
                                    },
                                    OperationIdentifier = "Operation 1",
                                    Options = OasOperationOptions.Deprecated,
                                    Parameters =
                                    {
                                        [ new OasParameterKey("p1") ] = "#/components/parameters/parameter1"
                                    },
                                    RequestBody = "#/components/requestBodies/requestBody1",
                                    Responses =
                                    {
                                        [OasResponseKey.Default] = "#/components/responses/response1"
                                    },
                                    Summary = "Delete operation summary"
                                },
                                Get = new OasOperationBuilder()
                                {
                                    Description = "Get"
                                },
                                Head = new OasOperationBuilder()
                                {
                                    Description = "Head"
                                },
                                Options = new OasOperationBuilder()
                                {
                                    Description = "Options"
                                },
                                Parameters =
                                {
                                    [ new OasParameterKey("p1") ] = "#/components/parameters/parameter1"
                                },
                                Patch = new OasOperationBuilder()
                                {
                                    Description = "Patch"
                                },
                                Post = new OasOperationBuilder()
                                {
                                    Description = "Post"
                                },
                                Put = new OasOperationBuilder()
                                {
                                    Description = "Put"
                                },
                                Summary = "Summary",
                                Trace = new OasOperationBuilder()
                                {
                                    Description = "Trace"
                                }
                            }
                        }
                    },
                    Examples =
                    {
                        [ "example1" ] = new OasExampleBuilder()
                        {
                            Description = "Description",
                            ExternalValue = new Uri("http://example.org/example"),
                            Summary = "Summary"
                        }
                    },
                    Headers =
                    {
                        [ "header1" ] = new OasParameterBodyBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json") ] = new OasMediaTypeBuilder()
                                {
                                    Encoding =
                                    {
                                        [ "header1" ] = new OasPropertyEncodingBuilder()
                                        {
                                            ContentType = new ContentType("application/json"),
                                            Headers =
                                            {
                                                [ "header1" ] = "#/components/headers/header1"
                                            },
                                            Options = OasPropertyEncodingOptions.AllowReserved | OasPropertyEncodingOptions.Explode,
                                            Style = OasParameterStyle.DeepObject
                                        }
                                    },
                                    Examples =
                                    {
                                        [ "Main Example" ] = "#/components/examples/example1"
                                    },
                                    Schema = "#/components/schemas/schema1"
                                }
                            }
                        }
                    },
                    Links =
                    {
                        [ "link1" ] = new OasLinkBuilder()
                        {
                            Description = "Description",
                            OperationIdentifier = "operation1",
                            OperationReference = "http://example.org/#/operation1",
                            Server = new OasServerBuilder()
                            {
                                Description = "Description",
                                Url = new Uri("http://example.org"),
                                Variables =
                                {
                                    [ "variable1" ] = new OasServerVariableBuilder()
                                    {
                                        Default = "Value",
                                        Description = "Description",
                                        Enum =
                                        {
                                            "Value", "Value1"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Parameters =
                    {
                        [ "parameter1" ] = new OasParameterBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json")] = new OasMediaTypeBuilder()
                                {
                                    Schema = "#/components/schemas/schema1"
                                }
                            },
                            Description = "Description",
                            Examples =
                            {
                                [ new ContentType("application/json") ] = "#/components/examples/example1"
                            },
                            Location = OasParameterLocation.Header,
                            Name = "parameter1",
                            Options = OasParameterOptions.AllowEmptyValue | OasParameterOptions.AllowReserved | OasParameterOptions.Deprecated | OasParameterOptions.Explode | OasParameterOptions.Required,
                            Schema = "#/components/schemas/schema1",
                            Style = OasParameterStyle.Matrix
                        }
                    },
                    RequestBodies =
                    {
                        [ "requestbody1" ] = new OasRequestBodyBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json" )] = new OasMediaTypeBuilder()
                                {
                                    Schema = "#/components/schemas/schema1"
                                }
                            },
                            Description = "Description",
                            Options = OasRequestBodyOptions.Required
                        }
                    },
                    Responses =
                    {
                        [ "response1" ] = new OasResponseBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json") ] = new OasMediaTypeBuilder()
                                {
                                    Schema = "#/components/schemas/schema1"
                                }
                            },
                            Description = "Description",
                            Headers =
                            {
                                [ "header1" ] = "#/components/headers/header1"
                            },
                            Links =
                            {
                                [ "link1" ] = "#/components/links/link1"
                            }
                        }
                    },
                    Schemas =
                    {
                        [ "schema1" ] = new OasSchemaBuilder()
                        {
                            AdditionalProperties =
                            {
                                [ "prop1" ] = "#/components/schemas/schema1"
                            },
                            Description = "Description",
                            ExternalDocumentation = new OasExternalDocumentationBuilder()
                            {
                                Description = "Description",
                                Url = new Uri("http://example.org/docs")
                            },
                            Format = "Format",
                            Items = "#/components/schemas/schema1",
                            ItemsRange = new CountConstraint(100, 200),
                            LengthRange = new CountConstraint(200, 300),
                            NumberRange = new NumberConstraint(300, 400, RangeOptions.Exclusive),
                            Options = OasSchemaOptions.Deprecated | OasSchemaOptions.Nullable | OasSchemaOptions.Required | OasSchemaOptions.UniqueItems,
                            Pattern = "[a-z]",
                            Properties =
                            {
                                [ "prop1" ] = "#/components/schemas/schema1"
                            },
                            PropertiesRange = new CountConstraint(100, 200),
                            Title = "Schema1",
                            JsonType = OasSchemaType.Object
                        }
                    },
                    SecuritySchemes =
                    {
                        [ "sec1" ] = new OasHttpSecuritySchemeBuilder()
                        {
                            BearerFormat = "Bearer",
                            Description = "Description",
                            Scheme = "Schema"
                        },
                        [ "sec2" ] = new OasApiKeySecuritySchemeBuilder()
                        {
                            Description = "Description",
                            Location = OasParameterLocation.Cookie,
                            Name = "Name"
                        },
                        [ "sec3" ] = new OasOidcSecuritySchemeBuilder()
                        {
                            Description = "Description",
                            Url = new Uri("http://example.org/openid")
                        },
                        [ "sec4" ] = new OasOAuth2SecuritySchemeBuilder()
                        {
                            AuthorizationCodeFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/auth/auth"),
                                RefreshUrl = new Uri("http://example.org/auth/refresh"),
                                TokenUrl = new Uri("http://example.org/auth/token"),
                                Scopes =
                                {
                                    [ "user:details" ] = "Get the user details"
                                }
                            },
                            ClientCredentialsFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/cli/auth")
                            },
                            Description = "Description",
                            ImplicitFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/imp/auth")
                            },
                            PasswordFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/pwd/auth")
                            }
                        }
                    }
                },
                ExternalDocumentation = new OasExternalDocumentationBuilder()
                {
                    Description = "Description",
                    Url = new Uri("http://example.org/docs")
                },
                Info = new OasInformationBuilder()
                {
                    Contact = new OasContactBuilder()
                    {
                        Email = new MailAddress("jonathan@example.org"),
                        Name = "Jonathan",
                        Url = new Uri("http://example.org/jonathan")
                    },
                    Description = "Description",
                    License = new OasLicenseBuilder()
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    TermsOfService = new Uri("http://example.org/tos"),
                    Title = "Title",
                    Version = new SemanticVersion(1, 2, 3, "pre1", "build1")
                },
                Version = new SemanticVersion(3, 0, 1),
                Paths =
                {
                    [ "path1" ] = new OasPathBuilder()
                    {
                        Description = "Description"
                    }
                },
                Security =
                {
                    "#/components/security/sec1"
                }
            };

            var actualJson = sut.Serialize(actualGraphBuilder.Build());

            #endregion

            #region JSON

            var expectedJson = new JObject()
            {
                ["components"] = new JObject()
                {
                    ["callbacks"] = new JObject()
                    {
                        ["callback1"] = new JObject()
                        {
                            ["http://foo{$statusCode}"] = new JObject()
                            {
                                ["delete"] = new JObject()
                                {
                                    ["callbacks"] = new JObject()
                                    {
                                        ["callback1"] = new JObject()
                                        {
                                            ["$ref"] = "#/components/callbacks/callback1"
                                        }
                                    },
                                    ["deprecated"] = true,
                                    ["description"] = "Delete operation",
                                    ["externalDocs"] = new JObject()
                                    {
                                        ["description"] = "External docs 1",
                                        ["url"] = "http://example.org/"
                                    },
                                    ["operationId"] = "Operation 1",
                                    ["parameters"] = new JArray()
                                    {
                                        new JObject()
                                        {
                                            [ "$ref" ] = "#/components/parameters/parameter1"
                                        }
                                    },
                                    ["requestBody"] = new JObject()
                                    {
                                        ["$ref"] = "#/components/requestBodies/requestBody1"
                                    },
                                    ["responses"] = new JObject()
                                    {
                                        ["default"] = new JObject()
                                        {
                                            ["$ref"] = "#/components/responses/response1"
                                        }
                                    },
                                    ["summary"] = "Delete operation summary",
                                    ["x-k2-operation-id"] = "0000000000000001"
                                },
                                ["description"] = "path1",
                                ["get"] = new JObject()
                                {
                                    ["description"] = "Get"
                                },
                                ["head"] = new JObject()
                                {
                                    ["description"] = "Head"
                                },
                                ["options"] = new JObject()
                                {
                                    ["description"] = "Options"
                                },
                                ["parameters"] = new JArray()
                                {
                                    new JObject()
                                    {
                                        [ "$ref" ] = "#/components/parameters/parameter1"
                                    }
                                },
                                ["patch"] = new JObject()
                                {
                                    ["description"] = "Patch"
                                },
                                ["post"] = new JObject()
                                {
                                    ["description"] = "Post"
                                },
                                ["put"] = new JObject()
                                {
                                    ["description"] = "Put"
                                },
                                ["summary"] = "Summary",
                                ["trace"] = new JObject()
                                {
                                    ["description"] = "Trace"
                                }
                            }
                        }
                    },
                    ["examples"] = new JObject()
                    {
                        ["example1"] = new JObject()
                        {
                            ["description"] = "Description",
                            ["externalValue"] = "http://example.org/example",
                            ["summary"] = "Summary"
                        }
                    },
                    ["headers"] = new JObject()
                    {
                        ["header1"] = new JObject()
                        {
                            ["content"] = new JObject()
                            {
                                ["application/json"] = new JObject()
                                {
                                    ["encoding"] = new JObject()
                                    {
                                        ["header1"] = new JObject()
                                        {
                                            ["allowReserved"] = true,
                                            ["contentType"] = "application/json",
                                            ["explode"] = true,
                                            ["headers"] = new JObject()
                                            {
                                                ["header1"] = new JObject()
                                                {
                                                    ["$ref"] = "#/components/headers/header1"
                                                }
                                            },
                                            ["style"] = "deepObject"
                                        }
                                    },
                                    ["examples"] = new JObject()
                                    {
                                        ["Main Example"] = new JObject()
                                        {
                                            ["$ref"] = "#/components/examples/example1"
                                        }
                                    },
                                    ["schema"] = new JObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            }
                        }
                    },
                    ["links"] = new JObject()
                    {
                        ["link1"] = new JObject()
                        {
                            ["description"] = "Description",
                            ["operationId"] = "operation1",
                            ["operationRef"] = "http://example.org/#/operation1",
                            ["server"] = new JObject()
                            {
                                ["description"] = "Description",
                                ["url"] = "http://example.org/",
                                ["variables"] = new JObject()
                                {
                                    ["variable1"] = new JObject()
                                    {
                                        ["default"] = "Value",
                                        ["description"] = "Description",
                                        ["enum"] = new JArray()
                                        {
                                            "Value",
                                            "Value1"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    ["parameters"] = new JObject()
                    {
                        ["parameter1"] = new JObject()
                        {
                            ["allowEmptyValue"] = true,
                            ["allowReserved"] = true,
                            ["content"] = new JObject()
                            {
                                ["application/json"] = new JObject()
                                {
                                    ["schema"] = new JObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            },
                            ["deprecated"] = true,
                            ["description"] = "Description",
                            ["examples"] = new JObject()
                            {
                                ["application/json"] = new JObject()
                                {
                                    ["$ref"] = "#/components/examples/example1"
                                }
                            },
                            ["explode"] = true,
                            ["in"] = "header",
                            ["name"] = "parameter1",
                            ["required"] = true,
                            ["schema"] = new JObject()
                            {
                                ["$ref"] = "#/components/schemas/schema1"
                            },
                            ["style"] = "matrix"
                        }
                    },
                    ["requestBodies"] = new JObject()
                    {
                        ["requestbody1"] = new JObject()
                        {
                            ["content"] = new JObject()
                            {
                                ["application/json"] = new JObject()
                                {
                                    ["schema"] = new JObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            },
                            ["description"] = "Description",
                            ["required"] = true
                        }
                    },
                    ["responses"] = new JObject()
                    {
                        ["response1"] = new JObject()
                        {
                            ["content"] = new JObject()
                            {
                                ["application/json"] = new JObject()
                                {
                                    ["schema"] = new JObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            },
                            ["description"] = "Description",
                            ["headers"] = new JObject()
                            {
                                ["header1"] = new JObject()
                                {
                                    ["$ref"] = "#/components/headers/header1"
                                }
                            },
                            ["links"] = new JObject()
                            {
                                ["link1"] = new JObject()
                                {
                                    ["$ref"] = "#/components/links/link1"
                                }
                            }
                        }
                    },
                    ["schemas"] = new JObject()
                    {
                        ["schema1"] = new JObject()
                        {
                            ["additionalProperties"] = new JObject()
                            {
                                ["prop1"] = new JObject()
                                {
                                    ["$ref"] = "#/components/schemas/schema1"
                                }
                            },
                            ["deprecated"] = true,
                            ["description"] = "Description",
                            ["exclusiveMaximum"] = true,
                            ["exclusiveMinimum"] = true,
                            ["externalDocs"] = new JObject()
                            {
                                ["description"] = "Description",
                                ["url"] = "http://example.org/docs"
                            },
                            ["format"] = "Format",
                            ["items"] = new JObject()
                            {
                                ["$ref"] = "#/components/schemas/schema1"
                            },
                            ["maxLength"] = 300,
                            ["maxProperties"] = 200,
                            ["maximum"] = 400,
                            ["minLength"] = 200,
                            ["minProperties"] = 100,
                            ["minimum"] = 300,
                            ["nullable"] = true,
                            ["pattern"] = "[a-z]",
                            ["properties"] = new JObject()
                            {
                                ["prop1"] = new JObject()
                                {
                                    ["$ref"] = "#/components/schemas/schema1"
                                }
                            },
                            ["required"] = true,
                            ["title"] = "Schema1",
                            ["type"] = "object",
                            ["uniqueItems"] = true
                        }
                    },
                    ["securitySchemes"] = new JObject()
                    {
                        ["sec1"] = new JObject()
                        {
                            ["bearerFormat"] = "Bearer",
                            ["description"] = "Description",
                            ["scheme"] = "Schema",
                            ["type"] = "http"
                        },
                        ["sec2"] = new JObject()
                        {
                            ["description"] = "Description",
                            ["in"] = "cookie",
                            ["name"] = "Name",
                            ["type"] = "apiKey"
                        },
                        ["sec3"] = new JObject()
                        {
                            ["description"] = "Description",
                            ["openIdConnectUrl"] = "http://example.org/openid",
                            ["type"] = "openIdConnect"
                        },
                        ["sec4"] = new JObject()
                        {
                            ["description"] = "Description",
                            ["flows"] = new JObject()
                            {
                                ["authorizationCode"] = new JObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/auth/auth",
                                    ["refreshUrl"] = "http://example.org/auth/refresh",
                                    ["scopes"] = new JObject()
                                    {
                                        ["user:details"] = "Get the user details"
                                    },
                                    ["tokenUrl"] = "http://example.org/auth/token"
                                },
                                ["clientCredentials"] = new JObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/cli/auth",
                                    ["tokenUrl"] = null
                                },
                                ["implicit"] = new JObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/imp/auth",
                                    ["tokenUrl"] = null
                                },
                                ["password"] = new JObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/pwd/auth",
                                    ["tokenUrl"] = null
                                }
                            },
                            ["type"] = "oauth2"
                        }
                    }
                },
                ["externalDocs"] = new JObject()
                {
                    ["description"] = "Description",
                    ["url"] = "http://example.org/docs"
                },
                ["info"] = new JObject()
                {
                    ["contact"] = new JObject()
                    {
                        ["email"] = "jonathan@example.org",
                        ["name"] = "Jonathan",
                        ["url"] = "http://example.org/jonathan"
                    },
                    ["description"] = "Description",
                    ["license"] = new JObject()
                    {
                        ["name"] = "MIT",
                        ["url"] = "https://opensource.org/licenses/MIT"
                    },
                    ["termsOfService"] = "http://example.org/tos",
                    ["title"] = "Title",
                    ["version"] = "1.2.3-pre1+build1"
                },
                ["openapi"] = "3.0.1",
                ["paths"] = new JObject()
                {
                    ["path1"] = new JObject()
                    {
                        ["description"] = "Description"
                    }
                },
                ["security"] = new JArray()
                {
                    new JObject()
                    {
                        [ "$ref" ] = "#/components/security/sec1"
                    }
                }
            };

            #endregion

            Assert.Equal(expectedJson, actualJson, JObjectComparer.Default);
        }

        #endregion
    }
}
