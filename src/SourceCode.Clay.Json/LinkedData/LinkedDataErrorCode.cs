#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.Json.LinkedData
{
    public enum LinkedDataErrorCode
    {
        None = 0,
        InvalidReversePropertyMap,
        CollidingKeywords,
        InvalidIdValue,
        InvalidTypeValue,
        InvalidValueObjectValue,
        InvalidLanguageTaggedString,
        InvalidIndexValue,
        ListOfLists,
        InvalidReversePropertyValue,
        RecursiveContextInclusion,
        LoadingRemoteContextFailed,
        InvalidRemoteContext,
        InvalidLocalContext,
        InvalidBaseValue,
        InvalidBaseIri,
        InvalidVocabMapping,
        InvalidDefaultLanguage,
        CyclicIriMapping,
        KeywordRedefinition,
        InvalidTermDefinition,
        InvalidVersionValue,
        InvalidTypeMapping,
        InvalidReverseProperty,
        InvalidIriMapping,
        InvalidKeywordAlias,
        InvalidContainerMapping,
        InvalidLanguageMapping,
        InvalidPrefixValue,
        InvalidGraph,
        InvalidLanguageMapValue,
        InvalidValueObject,
        InvalidLanguageTaggedValue,
        InvalidSetOrListObject,
        InvalidExpandContext,
    }
}
