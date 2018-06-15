#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Json.LinkedData
{
    public static class LDKeywords
    {
        public const string Id = "@id";
        public const string Type = "@type";
        public const string Context = "@context";
        public const string Graph = "@graph";
        public const string Value = "@value";
        public const string Language = "@language";
        public const string Container = "@container";
        public const string List = "@list";
        public const string Set = "@set";
        public const string Reverse = "@reverse";
        public const string Index = "@index";
        public const string Base = "@base";
        public const string Vocab = "@vocab";
        public const string Version = "@version";
        public const string Nest = "@nest";
        public const string Prefix = "@prefix";

        private static readonly HashSet<string> _keywords = new HashSet<string>(StringComparer.Ordinal)
        {
            Id, Type, Context, Graph, Value, Language, Container, List, Set, Reverse, Index, Base, Vocab
        };

        public static bool IsKeyword(string value) => _keywords.Contains(value);
    }
}
