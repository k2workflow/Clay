#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi.Serialization
{
    partial class OasSerializer
    {
        #region Constants

        /// <summary>
        /// Represents constants for OpenAPI enum values.
        /// </summary>
        protected static class EnumConstants
        {
            #region Constants

            /// <summary>
            /// Gets a constant containing "matrix".
            /// </summary>
            public const string Matrix = "matrix";

            /// <summary>
            /// Gets a constant containing "label".
            /// </summary>
            public const string Label = "label";

            /// <summary>
            /// Gets a constant containing "form".
            /// </summary>
            public const string Form = "form";

            /// <summary>
            /// Gets a constant containing "simple".
            /// </summary>
            public const string Simple = "simple";

            /// <summary>
            /// Gets a constant containing "spaceDelimited".
            /// </summary>
            public const string SpaceDelimited = "spaceDelimited";

            /// <summary>
            /// Gets a constant containing "pipeDelimited".
            /// </summary>
            public const string PipeDelimited = "pipeDelimited";

            /// <summary>
            /// Gets a constant containing "deepObject".
            /// </summary>
            public const string DeepObject = "deepObject";

            /// <summary>
            /// Gets a constant containing "query".
            /// </summary>
            public const string Query = "query";

            /// <summary>
            /// Gets a constant containing "header".
            /// </summary>
            public const string Header = "header";

            /// <summary>
            /// Gets a constant containing "path".
            /// </summary>
            public const string Path = "path";

            /// <summary>
            /// Gets a constant containing "cookie".
            /// </summary>
            public const string Cookie = "cookie";

            /// <summary>
            /// Gets a constant containing "array".
            /// </summary>
            public const string Array = "array";

            /// <summary>
            /// Gets a constant containing "boolean".
            /// </summary>
            public const string Boolean = "boolean";

            /// <summary>
            /// Gets a constant containing "number".
            /// </summary>
            public const string Number = "number";

            /// <summary>
            /// Gets a constant containing "object".
            /// </summary>
            public const string Object = "object";

            /// <summary>
            /// Gets a constant containing "string".
            /// </summary>
            public const string String = "string";

            /// <summary>
            /// Gets a constant containing "integer".
            /// </summary>
            public const string Integer = "integer";

            /// <summary>
            /// Gets a constant containing "apiKey".
            /// </summary>
            public const string ApiKey = "apiKey";

            /// <summary>
            /// Gets a constant containing "http".
            /// </summary>
            public const string Http = "http";

            /// <summary>
            /// Gets a constant containing "oauth2".
            /// </summary>
            public const string OAuth2 = "oauth2";

            /// <summary>
            /// Gets a constant containing "openIdConnect".
            /// </summary>
            public const string OpenIdConnect = "openIdConnect";

            #endregion
        }

        /// <summary>
        /// Represents constants for OpenAPI property names.
        /// </summary>
        public static class PathConstants
        {
            #region Constants

            /// <summary>
            /// Gets a constant containing "$ref".
            /// </summary>
            internal const string Reference = "$ref";

            /// <summary>
            /// Gets a constant containing "name".
            /// </summary>
            internal const string Name = "name";

            /// <summary>
            /// Gets a constant containing "externalDocs".
            /// </summary>
            internal const string ExternalDocs = "externalDocs";

            /// <summary>
            /// Gets a constant containing "description".
            /// </summary>
            internal const string Description = "description";

            /// <summary>
            /// Gets a constant containing "type".
            /// </summary>
            internal const string Type = "type";

            /// <summary>
            /// Gets a constant containing "title".
            /// </summary>
            internal const string Title = "title";

            /// <summary>
            /// Gets a constant containing "format".
            /// </summary>
            internal const string Format = "format";

            /// <summary>
            /// Gets a constant containing "multipleOf".
            /// </summary>
            internal const string MultipleOf = "multipleOf";

            /// <summary>
            /// Gets a constant containing "maximum".
            /// </summary>
            internal const string Maximum = "maximum";

            /// <summary>
            /// Gets a constant containing "exclusiveMaximum".
            /// </summary>
            internal const string ExclusiveMaximum = "exclusiveMaximum";

            /// <summary>
            /// Gets a constant containing "minimum".
            /// </summary>
            internal const string Minimum = "minimum";

            /// <summary>
            /// Gets a constant containing "exclusiveMinimum".
            /// </summary>
            internal const string ExclusiveMinimum = "exclusiveMinimum";

            /// <summary>
            /// Gets a constant containing "minLength".
            /// </summary>
            internal const string MinLength = "minLength";

            /// <summary>
            /// Gets a constant containing "maxLength".
            /// </summary>
            internal const string MaxLength = "maxLength";

            /// <summary>
            /// Gets a constant containing "minProperties".
            /// </summary>
            internal const string MinProperties = "minProperties";

            /// <summary>
            /// Gets a constant containing "maxProperties".
            /// </summary>
            internal const string MaxProperties = "maxProperties";

            /// <summary>
            /// Gets a constant containing "uniqueItems".
            /// </summary>
            internal const string UniqueItems = "uniqueItems";

            /// <summary>
            /// Gets a constant containing "required".
            /// </summary>
            internal const string Required = "required";

            /// <summary>
            /// Gets a constant containing "enum".
            /// </summary>
            internal const string EnumValue = "enum";

            /// <summary>
            /// Gets a constant containing "allOf".
            /// </summary>
            internal const string AllOf = "allOf";

            /// <summary>
            /// Gets a constant containing "anyOf".
            /// </summary>
            internal const string AnyOf = "anyOf";

            /// <summary>
            /// Gets a constant containing "oneOf".
            /// </summary>
            internal const string OneOf = "oneOf";

            /// <summary>
            /// Gets a constant containing "not".
            /// </summary>
            internal const string Not = "not";

            /// <summary>
            /// Gets a constant containing "items".
            /// </summary>
            internal const string Items = "items";

            /// <summary>
            /// Gets a constant containing "properties".
            /// </summary>
            internal const string Properties = "properties";

            /// <summary>
            /// Gets a constant containing "additionalProperties".
            /// </summary>
            internal const string AdditionalProperties = "additionalProperties";

            /// <summary>
            /// Gets a constant containing "nullable".
            /// </summary>
            internal const string Nullable = "nullable";

            /// <summary>
            /// Gets a constant containing "deprecated".
            /// </summary>
            internal const string Deprecated = "deprecated";

            /// <summary>
            /// Gets a constant containing "pattern".
            /// </summary>
            internal const string Pattern = "pattern";

            /// <summary>
            /// Gets a constant containing "schema".
            /// </summary>
            internal const string Schema = "schema";

            /// <summary>
            /// Gets a constant containing "responses".
            /// </summary>
            internal const string Responses = "responses";

            /// <summary>
            /// Gets a constant containing "parameters".
            /// </summary>
            internal const string Parameters = "parameters";

            /// <summary>
            /// Gets a constant containing "examples".
            /// </summary>
            internal const string Examples = "examples";

            /// <summary>
            /// Gets a constant containing "requestBodies".
            /// </summary>
            internal const string RequestBodies = "requestBodies";

            /// <summary>
            /// Gets a constant containing "requestBody".
            /// </summary>
            internal const string RequestBody = "requestBody";

            /// <summary>
            /// Gets a constant containing "headers".
            /// </summary>
            internal const string Headers = "headers";

            /// <summary>
            /// Gets a constant containing "securityScheme".
            /// </summary>
            internal const string SecuritySchemes = "securitySchemes";

            /// <summary>
            /// Gets a constant containing "links".
            /// </summary>
            internal const string Links = "links";

            /// <summary>
            /// Gets a constant containing "callback".
            /// </summary>
            internal const string Callbacks = "callbacks";

            /// <summary>
            /// Gets a constant containing "url".
            /// </summary>
            internal const string Url = "url";

            /// <summary>
            /// Gets a constant containing "email".
            /// </summary>
            internal const string Email = "email";

            /// <summary>
            /// Gets a constant containing "server".
            /// </summary>
            internal const string Server = "server";

            /// <summary>
            /// Gets a constant containing "contentType".
            /// </summary>
            internal const string ContentType = "contentType";

            /// <summary>
            /// Gets a constant containing "style".
            /// </summary>
            internal const string Style = "style";

            /// <summary>
            /// Gets a constant containing "explode".
            /// </summary>
            internal const string Explode = "explode";

            /// <summary>
            /// Gets a constant containing "allowReserved".
            /// </summary>
            internal const string AllowReserved = "allowReserved";

            /// <summary>
            /// Gets a constant containing "summary".
            /// </summary>
            internal const string Summary = "summary";

            /// <summary>
            /// Gets a constant containing "value".
            /// </summary>
            internal const string Value = "value";

            /// <summary>
            /// Gets a constant containing "externalValue".
            /// </summary>
            internal const string ExternalValue = "externalValue";

            /// <summary>
            /// Gets a constant containing "termsOfService".
            /// </summary>
            internal const string TermsOfService = "termsOfService";

            /// <summary>
            /// Gets a constant containing "contact".
            /// </summary>
            internal const string Contact = "contact";

            /// <summary>
            /// Gets a constant containing "license".
            /// </summary>
            internal const string License = "license";

            /// <summary>
            /// Gets a constant containing "operationId".
            /// </summary>
            internal const string OperationId = "operationId";

            /// <summary>
            /// Gets a constant containing "operationRef".
            /// </summary>
            internal const string OperationRef = "operationRef";

            /// <summary>
            /// Gets a constant containing "encoding".
            /// </summary>
            internal const string Encoding = "encoding";

            /// <summary>
            /// Gets a constant containing "allowEmptyValue".
            /// </summary>
            internal const string AllowEmptyValue = "allowEmptyValue";

            /// <summary>
            /// Gets a constant containing "content".
            /// </summary>
            internal const string Content = "content";

            /// <summary>
            /// Gets a constant containing "in".
            /// </summary>
            internal const string In = "in";

            /// <summary>
            /// Gets a constant containing "delete".
            /// </summary>
            internal const string Delete = "delete";

            /// <summary>
            /// Gets a constant containing "get".
            /// </summary>
            internal const string Get = "get";

            /// <summary>
            /// Gets a constant containing "head".
            /// </summary>
            internal const string Head = "head";

            /// <summary>
            /// Gets a constant containing "options".
            /// </summary>
            internal const string Options = "options";

            /// <summary>
            /// Gets a constant containing "patch".
            /// </summary>
            internal const string Patch = "patch";

            /// <summary>
            /// Gets a constant containing "post".
            /// </summary>
            internal const string Post = "post";

            /// <summary>
            /// Gets a constant containing "put".
            /// </summary>
            internal const string Put = "put";

            /// <summary>
            /// Gets a constant containing "trace".
            /// </summary>
            internal const string Trace = "trace";

            /// <summary>
            /// Gets a constant containing "default".
            /// </summary>
            internal const string Default = "default";

            /// <summary>
            /// Gets a constant containing "enum".
            /// </summary>
            internal const string Enum = "enum";

            /// <summary>
            /// Gets a constant containing "variables".
            /// </summary>
            internal const string Variables = "variables";

            /// <summary>
            /// Gets a constant containing "scheme".
            /// </summary>
            internal const string Scheme = "scheme";

            /// <summary>
            /// Gets a constant containing "bearerFormat".
            /// </summary>
            internal const string BearerFormat = "bearerFormat";

            /// <summary>
            /// Gets a constant containing "flows".
            /// </summary>
            internal const string Flows = "flows";

            /// <summary>
            /// Gets a constant containing "implicit".
            /// </summary>
            internal const string Implicit = "implicit";

            /// <summary>
            /// Gets a constant containing "password".
            /// </summary>
            internal const string Password = "pass" + "word";

            /// <summary>
            /// Gets a constant containing "clientCredentials".
            /// </summary>
            internal const string ClientCredentials = "clientCredentials";

            /// <summary>
            /// Gets a constant containing "authorizationCode".
            /// </summary>
            internal const string AuthorizationCode = "authorizationCode";

            /// <summary>
            /// Gets a constant containing "authorizationUrl".
            /// </summary>
            internal const string AuthorizationUrl = "authorizationUrl";

            /// <summary>
            /// Gets a constant containing "tokenUrl".
            /// </summary>
            internal const string TokenUrl = "tokenUrl";

            /// <summary>
            /// Gets a constant containing "refreshUrl".
            /// </summary>
            internal const string RefreshUrl = "refreshUrl";

            /// <summary>
            /// Gets a constant containing "scopes".
            /// </summary>
            internal const string Scopes = "scopes";

            /// <summary>
            /// Gets a constant containing "openIdConnectUrl".
            /// </summary>
            internal const string OpenIdConnectUrl = "openIdConnectUrl";

            /// <summary>
            /// Gets a constant containing "tags".
            /// </summary>
            public const string Tags = "tags";

            /// <summary>
            /// Gets a constant containing "openapi".
            /// </summary>
            public const string OpenApi = "openapi";

            /// <summary>
            /// Gets a constant containing "info".
            /// </summary>
            public const string Info = "info";

            /// <summary>
            /// Gets a constant containing "servers".
            /// </summary>
            public const string Servers = "servers";

            /// <summary>
            /// Gets a constant containing "paths".
            /// </summary>
            public const string Paths = "paths";

            /// <summary>
            /// Gets a constant containing "security".
            /// </summary>
            public const string Security = "security";

            /// <summary>
            /// Gets a constant containing "version".
            /// </summary>
            public const string Version = "version";

            /// <summary>
            /// Gets a constant containing "schemas".
            /// </summary>
            public const string Schemas = "schemas";

            /// <summary>
            /// Gets a constant containing "components".
            /// </summary>
            public const string Components = "components";

            #endregion
        }

        #endregion
    }
}
