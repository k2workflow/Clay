#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Text;

namespace SourceCode.Clay.Data.SqlParser
{
    public struct SqlTokenInfo
    {
        #region Properties

        public SqlTokenKind Kind { get; }

        public string Value { get; }

        #endregion

        #region Constructors

        internal SqlTokenInfo(SqlTokenKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }

        internal SqlTokenInfo(SqlTokenKind kind, StringBuilder value)
        {
            Kind = kind;
            Value = value?.ToString();
        }

        internal SqlTokenInfo(SqlTokenKind kind, params char[] value)
        {
            Kind = kind;
            Value = value == null ? null : new string(value);
        }

        internal SqlTokenInfo(SqlTokenKind kind, char[] value, int offset, int count)
        {
            Kind = kind;
            Value = new string(value, offset, count);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            switch (Kind)
            {
                case SqlTokenKind.BlockComment:
                case SqlTokenKind.LineComment:
                case SqlTokenKind.Whitespace:
                    return $"{Kind}";

                case SqlTokenKind.Literal:
                case SqlTokenKind.QuotedString:
                case SqlTokenKind.SquareString:
                case SqlTokenKind.Symbol:
                    return $"{Kind} ({Value})";
            }

            return base.ToString();
        }

        #endregion
    }
}
