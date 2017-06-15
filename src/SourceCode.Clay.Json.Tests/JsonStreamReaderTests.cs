using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class JsonStreamReaderTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "JsonStreamReader: Parse AAD document")]
        public static void Parsing_a_large_AAD_delta_document_should_succeed()
        {
            var json = File.ReadAllText("JsonStreamReaderAad.json");

            using (var sr = new StringReader(json))
            {
                var reader = new JsonStreamReader(sr);

                reader.ReadObject(name =>
                {
                    switch (name)
                    {
                        // eg: "https://graph.windows.net/<domain>.onmicrosoft.com/$metadata#directoryObjects/Microsoft.DirectoryServices.User"
                        case "odata.metadata":
                            reader.SkipString(); // Eat
                            break;

                        // Feed annotation that represents a URI to be called later after the polling interval has passed.
                        // eg: "https://graph.windows.net/<domain>.onmicrosoft.com/users?nextLink=..."
                        case "aad.nextLink":
                            var nextLink = reader.ReadString();
                            Assert.NotEmpty(nextLink);
                            break;

                        // Feed annotation that represents a URI to be called immediately for more changes.
                        // eg: "https://graph.windows.net/<domain>.onmicrosoft.com/users?deltaLink=..."
                        case "aad.deltaLink":
                            var deltaLink = reader.ReadString();
                            Assert.NotEmpty(deltaLink);
                            break;

                        // Value array
                        // "value": [...]
                        case "value":
                            var items = reader.ReadArray(() => ParseValue(reader));
                            Assert.True(items.Count == 191);
                            break;

                        // Custom
                        default:
                            // TODO: Ignore unknown properties, don't throw
                            throw new FormatException($"Invalid Json: Unknown property \"{name}\" at position {reader.Position}");
                    }
                });
            }
        }

        #region Helpers

        private static bool ParseValue(JsonStreamReader reader)
        {
            //Contract.Requires(reader != null);

            // "mail": "foo@bar.com",
            return reader.ReadObject
            (
                // PropertySwitch
                (name) =>
                {
                    switch (name) // value
                    {
                        // This seems to reliably be the first property, so can be used as a DISCRIMINATOR
                        case "odata.type":
                            {
                                var typ = reader.ReadString();
                                Assert.Contains(typ, new[]
                                {
                                    "Microsoft.DirectoryServices.User",
                                    "Microsoft.DirectoryServices.Group",
                                    "Microsoft.DirectoryServices.DirectoryLinkChange"
                                });
                            }
                            break;

                        // 'objectId'
                        case "objectId":
                        case "sourceObjectId":
                        case "targetObjectId":
                            {
                                var id = reader.ReadGuid();
                                Assert.True(id != Guid.Empty);
                            }
                            break;

                        // Property
                        default:
                            {
                                ParseProperty(reader, name);
                            }
                            break;

                        // Link
                        case "associationType":
                            {
                                var lnk = reader.ReadString();
                                Assert.Contains(lnk, new[] { "Member", "Manager" });
                            }
                            break;

                        case "sourceObjectType":
                        case "targetObjectType":
                            {
                                var typ = reader.ReadString();
                                Assert.Contains(typ, new[] { "User", "Group" });
                            }
                            break;

                        // Tombstone
                        case "accountEnabled": // User
                        case "aad.isDeleted": // Instance annotation that represents if the entry is deleted (User/Group/Link)
                        case "aad.isSoftDeleted": // Instance annotation that represents if the entry is soft-deleted (User/Group)
                            {
                                var enabled = reader.ReadBooleanNullable();
                                Assert.Contains(enabled, new bool?[] { true, false, null });
                            }
                            break;

                        case "deletionTimestamp":
                            {
                                var deleted = reader.ReadDateTimeNullable();
                                Assert.True(deleted == null || deleted > DateTime.MinValue);
                            }
                            break;

                        // Used when a user is deleted. The UPN value gets prefixed with the ObjectId Guid when a user is deleted
                        case "aad.originalUserPrincipalName":
                            {
                                var origUpn = reader.ReadString();
                                Assert.NotEmpty(origUpn);
                            }
                            break;

                        // This seems to follow 'aad.' properties, so cannot easily be used as a discriminator
                        case "objectType":
                        case "sourceObjectUri": // Ignore
                        case "targetObjectUri": // Ignore
                            {
                                reader.SkipString(); // Eat
                            }
                            break;
                    }
                },

                // ObjectFactory
                () => true
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseProperty(JsonStreamReader reader, string name)
        {
            //Contract.Requires(reader != null);

#pragma warning disable S1479 // "switch" statements should not have too many "case" clauses
            switch (name)
#pragma warning restore S1479 // "switch" statements should not have too many "case" clauses
            {
                // Bool
                case "dirSyncEnabled":
                case "mailEnabled":
                case "securityEnabled":
                    {
                        var tf = reader.ReadBooleanNullable();
                        Assert.Contains(tf, new bool?[] { true, false, null });
                        return;
                    }

                // String (common)
                case "city":
                case "country":
                case "creationType":
                case "department":
                case "passwordPolicies":
                case "preferredLanguage":
                case "physicalDeliveryOfficeName":
                case "postalCode":
                case "state":
                case "streetAddress":
                case "usageLocation":
                case "userType":
                    {
                        var str = reader.ReadString();
                        //Assert.NotEmpty(str);
                        return;
                    }

                // String (unique)
                case "displayName":
                case "mail":
                case "mailNickname":
                case "onPremisesSecurityIdentifier":
                case "description":
                case "facsimileTelephoneNumber":
                case "givenName":
                case "immutableId":
                case "jobTitle":
                case "mobile":
                case "sipProxyAddress":
                case "surname":
                case "telephoneNumber":
                case "userPrincipalName":
                case "proxyAddresses":
                case "otherMails":
                    {
                        var str = reader.ReadString();
                        Assert.NotEmpty(str);
                        return;
                    }

                // DateTime
                case "lastDirSyncTime":
                case "refreshTokensValidFromDateTime":
                    {
                        var dt = reader.ReadDateTimeNullable();
                        Assert.True(dt == null || dt > DateTime.MinValue);
                        return;
                    }

                // Custom
                default:
                    // TODO: Ignore unknown properties, don't throw
                    throw new FormatException($"Invalid Json: Unknown property \"{name}\" at position {reader.Position}");
            }
        }

        #endregion
    }
}
