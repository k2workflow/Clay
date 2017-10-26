#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;

namespace SourceCode.Clay.OpenApi.Serialization
{
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
    // Null is significant in JSON.

    partial class OasSerializer
    {
        #region Serialize

        /// <summary>
        /// Serializes a <see cref="OasResponse"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasResponse"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeResponse(OasResponse value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyConstants.Description, value.Description, true);
            SetJsonMap(json, PropertyConstants.Headers, value.Headers);
            SetJsonMap(json, PropertyConstants.Content, content);
            SetJsonMap(json, PropertyConstants.Links, value.Links);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasRequestBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasRequestBody"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeRequestBody(OasRequestBody value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonMap(json, PropertyConstants.Content, content);
            SetJsonFlag(json, PropertyConstants.Required, value.Options, OasRequestBodyOptions.Required);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasPropertyEncoding"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasPropertyEncoding"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializePropertyEncoding(OasPropertyEncoding value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.ContentType, value.ContentType);
            SetJsonMap(json, PropertyConstants.Headers, value.Headers);
            SetJsonValue(json, PropertyConstants.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PropertyConstants.Explode, value.Options, OasPropertyEncodingOptions.Explode);
            SetJsonFlag(json, PropertyConstants.AllowReserved, value.Options, OasPropertyEncodingOptions.AllowReserved);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasPath"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasPath"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializePath(OasPath value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Summary, value.Summary);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.Get, value.Get);
            SetJsonObject(json, PropertyConstants.Put, value.Put);
            SetJsonObject(json, PropertyConstants.Post, value.Post);
            SetJsonObject(json, PropertyConstants.Delete, value.Delete);
            SetJsonObject(json, PropertyConstants.Options, value.Options);
            SetJsonObject(json, PropertyConstants.Head, value.Head);
            SetJsonObject(json, PropertyConstants.Patch, value.Patch);
            SetJsonObject(json, PropertyConstants.Trace, value.Trace);
            SetJsonArray(json, PropertyConstants.Servers, value.Servers);
            SetJsonArray(json, PropertyConstants.Parameters, value.Parameters);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasParameterBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasParameterBody"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameterBody(OasParameterBody value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasParameterBody"/> value into an existing <see cref="JsonObject"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="OasParameterBody"/> value to serialize.</param>
        /// <param name="json">The <see cref="JsonObject"/> to populate.</param>
        protected virtual void SerializeParameterBody(OasParameterBody value, JsonObject json)
        {
            if (value == null) return;

            var examples = value.Examples
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonFlag(json, PropertyConstants.Required, value.Options, OasParameterOptions.Required);
            SetJsonFlag(json, PropertyConstants.Deprecated, value.Options, OasParameterOptions.Deprecated);
            SetJsonFlag(json, PropertyConstants.AllowEmptyValue, value.Options, OasParameterOptions.AllowEmptyValue);

            SetJsonValue(json, PropertyConstants.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PropertyConstants.Explode, value.Options, OasParameterOptions.Explode);
            SetJsonFlag(json, PropertyConstants.AllowReserved, value.Options, OasParameterOptions.AllowReserved);
            SetJsonObject(json, PropertyConstants.Schema, value.Schema);
            SetJsonMap(json, PropertyConstants.Examples, examples);

            SetJsonMap(json, PropertyConstants.Content, content);
        }

        /// <summary>
        /// Serializes a <see cref="OasParameter"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasParameter"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(OasParameter value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name, true);
            SetJsonValue(json, PropertyConstants.In, ToJsonValue(value.Location));
            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasParameter"/> value according to its components.
        /// </summary>
        /// <param name="key">The <see cref="OasParameterKey"/> component to serialize.</param>
        /// <param name="value">The referable <see cref="OasParameterBody"/> component to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(OasParameterKey key, OasReferable<OasParameterBody> value)
        {
            if (value.IsReference) return SerializeReferable(value);
            return SerializeParameter(key, value.Value);
        }

        /// <summary>
        /// Serializes a <see cref="OasParameter"/> value according to its components.
        /// </summary>
        /// <param name="key">The <see cref="OasParameterKey"/> component to serialize.</param>
        /// <param name="value">The <see cref="OasParameterBody"/> component to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(OasParameterKey key, OasParameterBody value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, key.Name, true);
            SetJsonValue(json, PropertyConstants.In, ToJsonValue(key.Location));
            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOperation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOperation"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOperation(OasOperation value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            var responses = value.Responses
                .Select(x => ValueTuple.Create(x.Key.ToString(), x.Value));

            SetJsonArray(json, PropertyConstants.Tags, value.Tags);
            SetJsonValue(json, PropertyConstants.Summary, value.Summary);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);
            SetJsonValue(json, PropertyConstants.OperationId, value.OperationIdentifier);
            SetJsonArray(json, PropertyConstants.Parameters, value.Parameters);
            SetJsonObject(json, PropertyConstants.RequestBody, value.RequestBody);
            SetJsonMap(json, PropertyConstants.Responses, responses);
            SetJsonMap(json, PropertyConstants.Callbacks, value.Callbacks);
            SetJsonFlag(json, PropertyConstants.Deprecated, value.Options, OasOperationOptions.Deprecated);
            SetJsonArray(json, PropertyConstants.Security, value.Security);
            SetJsonArray(json, PropertyConstants.Servers, value.Servers);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOidcSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOidcSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOpenIdConnectSecurityScheme(OasOidcSecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.OpenIdConnect);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.OpenIdConnectUrl, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOAuthFlow"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOAuthFlow"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOAuthFlow(OasOAuthFlow value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.AuthorizationUrl, value.AuthorizationUrl, true);
            SetJsonValue(json, PropertyConstants.TokenUrl, value.TokenUrl, true);
            SetJsonValue(json, PropertyConstants.RefreshUrl, value.RefreshUrl);
            SetJsonMap(json, PropertyConstants.Scopes, value.Scopes);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOAuth2SecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOAuth2SecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOAuth2SecurityScheme(OasOAuth2SecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.OAuth2);
            SetJsonValue(json, PropertyConstants.Description, value.Description);

            var flows = new JsonObject();

            SetJsonObject(flows, PropertyConstants.Implicit, value.ImplicitFlow);
            SetJsonObject(flows, PropertyConstants.Password, value.PasswordFlow);
            SetJsonObject(flows, PropertyConstants.ClientCredentials, value.ClientCredentialsFlow);
            SetJsonObject(flows, PropertyConstants.AuthorizationCode, value.AuthorizationCodeFlow);

            json.Add(PropertyConstants.Flows, flows);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasMediaType"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasMediaType"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeMediaType(OasMediaType value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonObject(json, PropertyConstants.Schema, value.Schema);
            SetJsonMap(json, PropertyConstants.Examples, value.Examples);
            SetJsonMap(json, PropertyConstants.Encoding, value.Encoding);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasLink"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasLink"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeLink(OasLink value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.OperationRef, SerializeReference(value.OperationReference));
            SetJsonValue(json, PropertyConstants.OperationId, value.OperationIdentifier);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.Server, value.Server);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasLicense"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasLicense"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeLicense(OasLicense value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.Url, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasInformation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasInformation"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeInformation(OasInformation value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Title, value.Title);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.TermsOfService, value.TermsOfService);
            SetJsonObject(json, PropertyConstants.Contact, value.Contact);
            SetJsonObject(json, PropertyConstants.License, value.License);
            SetJsonValue(json, PropertyConstants.Version, value.Version.ToString());

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasHttpSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasHttpSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeHttpSecurityScheme(OasHttpSecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.Http);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Scheme, value.Scheme, true);
            SetJsonValue(json, PropertyConstants.BearerFormat, value.BearerFormat);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasReference"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasReference"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeReference(OasReference value)
        {
            if (!value.HasValue) return null;
            return value.ToUri()?.ToString();
        }

        /// <summary>
        /// Serializes a <see cref="IOasReferable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="IOasReferable"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeReferable(IOasReferable value)
        {
            if (!value.HasValue) return null;
            if (value.IsReference)
            {
                var reference = Serialize(value.Reference);
                return new JsonObject()
                {
                    [PropertyConstants.Reference] = reference
                };
            }
            return SerializeUnknown(value.Value);
        }

        /// <summary>
        /// Serializes a <see cref="OasContact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasContact"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeComponents(OasComponents value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonMap(json, PropertyConstants.Schemas, value.Schemas);
            SetJsonMap(json, PropertyConstants.Responses, value.Responses);
            SetJsonMap(json, PropertyConstants.Parameters, value.Parameters);
            SetJsonMap(json, PropertyConstants.Examples, value.Examples);
            SetJsonMap(json, PropertyConstants.RequestBodies, value.RequestBodies);
            SetJsonMap(json, PropertyConstants.Headers, value.Headers);
            SetJsonMap(json, PropertyConstants.SecuritySchemes, value.SecuritySchemes);
            SetJsonMap(json, PropertyConstants.Links, value.Links);
            SetJsonMap(json, PropertyConstants.Callbacks, value.Callbacks);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasContact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasContact"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeContact(OasContact value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.Url, value.Url);
            SetJsonValue(json, PropertyConstants.Email, value.Email);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasExternalDocumentation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasExternalDocumentation"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeExternalDocumentation(OasExternalDocumentation value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Url, value.Url, true);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasExample"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasExample"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeExample(OasExample value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Summary, value.Summary);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.ExternalValue, value.ExternalValue);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasDocument"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasDocument"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeDocument(OasDocument value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.OpenApi, value.Version.ToString());
            SetJsonObject(json, PropertyConstants.Info, value.Info, true);
            SetJsonArray(json, PropertyConstants.Servers, value.Servers);
            SetJsonMap(json, PropertyConstants.Paths, value.Paths, true);
            SetJsonObject(json, PropertyConstants.Components, value.Components);
            SetJsonArray(json, PropertyConstants.Security, value.Security);
            SetJsonArray(json, PropertyConstants.Tags, value.Tags);
            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasCallback"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasCallback"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeCallback(OasCallback value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            foreach (var item in value)
            {
                var k = item.Key.ToString();
                var v = Serialize(item.Value);
                json.Add(k, v);
            }

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasApiKeySecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasApiKeySecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeApiKeySecurityScheme(OasApiKeySecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.ApiKey);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.In, ToJsonValue(value.Location));

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasTag"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasTag"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeTag(OasTag value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasServerVariable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasServerVariable"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeServerVariable(OasServerVariable value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonArray(json, PropertyConstants.Enum, value.Enum);
            SetJsonValue(json, PropertyConstants.Default, value.Default);
            SetJsonValue(json, PropertyConstants.Description, value.Description);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasServer"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasServer"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeServer(OasServer value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Url, value.Url);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonMap(json, PropertyConstants.Variables, value.Variables);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeSecurityScheme(OasSecurityScheme value) =>
            SerializeUnknown(value);

        /// <summary>
        /// Serializes a <see cref="OasSchema"/> value.
        /// </summary>
        /// <param name="value">The schema value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeSchema(OasSchema value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Title, value.Title);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Type, ToJsonValue(value.JsonType));
            SetJsonValue(json, PropertyConstants.Format, value.Format);

            SetJsonValue(json, PropertyConstants.Pattern, value.Pattern);
            SetJsonArray(json, PropertyConstants.EnumValue, value.Enum);

            SetJsonFlag(json, PropertyConstants.Deprecated, value.Options, OasSchemaOptions.Deprecated);
            SetJsonFlag(json, PropertyConstants.Nullable, value.Options, OasSchemaOptions.Nullable);
            SetJsonFlag(json, PropertyConstants.Required, value.Options, OasSchemaOptions.Required);
            SetJsonFlag(json, PropertyConstants.UniqueItems, value.Options, OasSchemaOptions.UniqueItems);

            SetJsonArray(json, PropertyConstants.AllOf, value.AllOf);
            SetJsonArray(json, PropertyConstants.AnyOf, value.AnyOf);
            SetJsonArray(json, PropertyConstants.OneOf, value.OneOf);
            SetJsonArray(json, PropertyConstants.Not, value.Not);

            SetJsonNumber(json, PropertyConstants.Minimum, value.NumberRange.Minimum);
            if (value.NumberRange.Minimum.HasValue)
                SetJsonFlag(json, PropertyConstants.ExclusiveMinimum, value.NumberRange.RangeOptions, OasRangeOptions.MinimumInclusive, invert: true);

            SetJsonNumber(json, PropertyConstants.Maximum, value.NumberRange.Maximum);
            if (value.NumberRange.Maximum.HasValue)
                SetJsonFlag(json, PropertyConstants.ExclusiveMaximum, value.NumberRange.RangeOptions, OasRangeOptions.MaximumInclusive, invert: true);

            SetJsonNumber(json, PropertyConstants.MultipleOf, value.NumberRange.MultipleOf);

            SetJsonValue(json, PropertyConstants.MinLength, value.LengthRange.Minimum);
            SetJsonValue(json, PropertyConstants.MaxLength, value.LengthRange.Maximum);

            SetJsonValue(json, PropertyConstants.MinProperties, value.PropertiesRange.Minimum);
            SetJsonValue(json, PropertyConstants.MaxProperties, value.PropertiesRange.Maximum);

            SetJsonObject(json, PropertyConstants.Items, value.Items);
            SetJsonMap(json, PropertyConstants.Properties, value.Properties);
            SetJsonMap(json, PropertyConstants.AdditionalProperties, value.AdditionalProperties);

            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>Serializes the specified OpenAPI object to a JSON value.</summary>
        /// <typeparam name="T">The type of the OpenAPI object.</typeparam>
        /// <param name="value">The instance of the OpenAPI object.</param>
        /// <returns>The serialized value.</returns>
        public virtual JsonValue Serialize<T>(T value)
        {
            if (ReferenceEquals(value, null)) return null;

            if (typeof(T) == typeof(string)) return (string)(object)value;

            if (value is IOasReferable referable) return SerializeReferable(referable);

            if (typeof(T) == typeof(KeyValuePair<OasParameterKey, OasParameterBody>))
            {
                var kvp = (KeyValuePair<OasParameterKey, OasParameterBody>)(object)value;
                return SerializeParameter(kvp.Key, kvp.Value);
            }

            if (typeof(T) == typeof(KeyValuePair<OasParameterKey, OasReferable<OasParameterBody>>))
            {
                var kvp = (KeyValuePair<OasParameterKey, OasReferable<OasParameterBody>>)(object)value;
                return SerializeParameter(kvp.Key, kvp.Value);
            }

            return SerializeUnknown(value);
        }

        #endregion
    }

#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
}
