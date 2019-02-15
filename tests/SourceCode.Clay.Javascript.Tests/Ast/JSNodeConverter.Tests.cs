using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Xunit;
using static SourceCode.Clay.Javascript.Ast.JSFluent;

namespace SourceCode.Clay.Javascript.Ast
{
    public static class JSNodeConverterTests
    {
        private static T Read<T>(JToken token)
        {
            var sut = new JSNodeConverter();
            var serializer = new JsonSerializer();
            serializer.Converters.Add(sut);
            using (var reader = token.CreateReader())
            {
                var result = serializer.Deserialize<T>(reader);

                // Test serialize.
                using (var tw = new StringWriter())
                using (var writer = new JsonTextWriter(tw))
                {
                    serializer.Serialize(writer, result);
                    writer.Flush();
                    var checkToken = JToken.Parse(tw.ToString());
                    Assert.Equal(token, checkToken, new JTokenEqualityComparer());
                }

                // Test in-place deserialize.
                using (var reader2 = token.CreateReader())
                {
                    reader2.Read();
                    result = (T)sut.ReadJson(reader2, typeof(T), result, serializer);
                    return result;
                }
            }
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ArrayExpression()
        {
            var json = new JObject()
            {
                ["type"] = "ArrayExpression",
                ["elements"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 1
                    },
                    new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 2
                    }
                }
            };

            var array = Read<JSArrayExpression>(json);
            Assert.Collection(array.Elements,
                item => Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value)),
                item => Assert.Equal(2, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value))
            );
        }

        [Theory]
        [InlineData("=", JSBinaryOperator.Assign)]
        [InlineData("+=", JSBinaryOperator.AddAssign)]
        [InlineData("-=", JSBinaryOperator.SubtractAssign)]
        [InlineData("*=", JSBinaryOperator.MultiplyAssign)]
        [InlineData("/=", JSBinaryOperator.DivideAssign)]
        [InlineData("%=", JSBinaryOperator.ModulusAssign)]
        [InlineData("<<=", JSBinaryOperator.UnsignedLeftShiftAssign)]
        [InlineData(">>=", JSBinaryOperator.SignedRightShiftAssign)]
        [InlineData(">>>=", JSBinaryOperator.UnsignedRightShiftAssign)]
        [InlineData("|=", JSBinaryOperator.BitwiseOrAssign)]
        [InlineData("^=", JSBinaryOperator.BitwiseXorAssign)]
        [InlineData("&=", JSBinaryOperator.BitwiseAndAssign)]
        public static void JSNodeConverter_ReadJson_AssignExpression(string operatorString, JSBinaryOperator @operator)
        {
            var json = new JObject()
            {
                ["type"] = "AssignmentExpression",
                ["operator"] = operatorString,
                ["left"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                },
                ["right"] = new JObject()
                {
                    ["type"] = "AssignmentExpression",
                    ["operator"] = operatorString,
                    ["left"] = new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 2
                    },
                    ["right"] = new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 3
                    }
                }
            };

            var binary = Read<JSBinaryExpression>(json);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(binary.Left).Value));
            Assert.Equal(@operator, binary.Operator);

            Assert.Collection(binary.Right,
                item => Assert.Equal(2, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value)),
                item => Assert.Equal(3, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value))
            );
        }

        [Theory]
        [InlineData("==", JSBinaryOperator.CoercedEquality)]
        [InlineData("!=", JSBinaryOperator.CoercedInequality)]
        [InlineData("===", JSBinaryOperator.IdentityEquality)]
        [InlineData("!==", JSBinaryOperator.IdentityInequality)]
        [InlineData("<", JSBinaryOperator.LessThan)]
        [InlineData("<=", JSBinaryOperator.LessThanOrEqual)]
        [InlineData(">", JSBinaryOperator.GreaterThan)]
        [InlineData(">=", JSBinaryOperator.GreaterThanOrEqual)]
        [InlineData("<<", JSBinaryOperator.UnsignedLeftShift)]
        [InlineData(">>", JSBinaryOperator.SignedRightShift)]
        [InlineData(">>>", JSBinaryOperator.UnsignedRightShift)]
        [InlineData("+", JSBinaryOperator.Add)]
        [InlineData("-", JSBinaryOperator.Subtract)]
        [InlineData("*", JSBinaryOperator.Multiply)]
        [InlineData("/", JSBinaryOperator.Divide)]
        [InlineData("%", JSBinaryOperator.Modulus)]
        [InlineData("|", JSBinaryOperator.BitwiseOr)]
        [InlineData("^", JSBinaryOperator.BitwiseXor)]
        [InlineData("&", JSBinaryOperator.BitwiseAnd)]
        [InlineData("in", JSBinaryOperator.In)]
        [InlineData("instanceof", JSBinaryOperator.InstanceOf)]
        public static void JSNodeConverter_ReadJson_BinaryExpression(string operatorString, JSBinaryOperator @operator)
        {
            var json = new JObject()
            {
                ["type"] = "BinaryExpression",
                ["operator"] = operatorString,
                ["left"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                },
                ["right"] = new JObject()
                {
                    ["type"] = "BinaryExpression",
                    ["operator"] = operatorString,
                    ["left"] = new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 2
                    },
                    ["right"] = new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 3
                    }
                }
            };

            var binary = Read<JSBinaryExpression>(json);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(binary.Left).Value));
            Assert.Equal(@operator, binary.Operator);

            Assert.Collection(binary.Right,
                item => Assert.Equal(2, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value)),
                item => Assert.Equal(3, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value))
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_BlockStatement()
        {
            var json = new JObject()
            {
                ["type"] = "BlockStatement",
                ["body"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "EmptyStatement"
                    },
                    new JObject()
                    {
                        ["type"] = "ReturnStatement",
                        ["argument"] = null
                    }
                }
            };

            var block = Read<JSBlockStatement>(json);

            Assert.Collection(block.Body,
                item => Assert.Null(item),
                item => Assert.IsType<JSReturnStatement>(item)
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_BlockStatement_Existing()
        {
            var json = new JObject()
            {
                ["type"] = "BlockStatement",
                ["body"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "EmptyStatement"
                    },
                    new JObject()
                    {
                        ["type"] = "ReturnStatement",
                        ["argument"] = null
                    }
                }
            };

            var sut = new JSNodeConverter();
            var existing = JSBlock(
                JSReturn(),
                JSReturn(JSIdentifier("test")),
                JSContinue(),
                JSContinue()
            );

            using (var reader = json.CreateReader())
            {
                var block = (JSBlockStatement)sut.ReadJson(reader, typeof(JSBlockStatement), existing, new JsonSerializer());

                Assert.Collection(block.Body,
                    item => Assert.Null(item),
                    item => Assert.Null(Assert.IsType<JSReturnStatement>(item).Expression)
                );
            }
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_BreakStatement()
        {
            var json = new JObject()
            {
                ["type"] = "BreakStatement",
                ["label"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                }
            };

            var @break = Read<JSBreakStatement>(json);
            Assert.Equal("a", @break.Label?.Name);

            json = new JObject()
            {
                ["type"] = "BreakStatement",
                ["label"] = null
            };

            @break = Read<JSBreakStatement>(json);
            Assert.Null(@break.Label);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_CallExpression()
        {
            var json = new JObject()
            {
                ["type"] = "CallExpression",
                ["callee"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["arguments"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 1
                    },
                    new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 2
                    }
                }
            };

            var call = Read<JSCallExpression>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(call.Callee).Name);
            Assert.Collection(call.Arguments,
                item => Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value)),
                item => Assert.Equal(2, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value))
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_CatchClause()
        {
            var json = new JObject()
            {
                ["type"] = "CatchClause",
                ["param"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "EmptyStatement"
                        },
                        new JObject()
                        {
                            ["type"] = "ReturnStatement",
                            ["argument"] = null
                        }
                    }
                }
            };

            var @catch = Read<JSCatchClause>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@catch.Parameter).Name);
            Assert.Collection(@catch.Body,
                item => Assert.Null(item),
                item => Assert.IsType<JSReturnStatement>(item)
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ConditionalExpression()
        {
            var json = new JObject()
            {
                ["type"] = "ConditionalExpression",
                ["test"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["consequent"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                },
                ["alternate"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "c"
                }
            };

            var conditional = Read<JSConditionalExpression>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(conditional.Test).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(conditional.Consequent).Name);
            Assert.Equal("c", Assert.IsType<JSIdentifier>(conditional.Alternate).Name);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ContinueStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ContinueStatement",
                ["label"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                }
            };

            var @continue = Read<JSContinueStatement>(json);
            Assert.Equal("a", @continue.Label?.Name);

            json = new JObject()
            {
                ["type"] = "ContinueStatement",
                ["label"] = null
            };

            @continue = Read<JSContinueStatement>(json);
            Assert.Null(@continue.Label);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_DebuggerStatement()
        {
            var json = new JObject()
            {
                ["type"] = "DebuggerStatement"
            };

            var debugger = Read<JSDebuggerStatement>(json);
            Assert.NotNull(debugger);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_DirectiveStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ExpressionStatement",
                ["expression"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = "use strict"
                },
                ["directive"] = "use strict"
            };

            var directive = Read<JSExpressionStatement>(json);
            Assert.Equal("use strict", Assert.IsType<string>(Assert.IsType<JSLiteral>(directive.Expression).Value));
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_DoWhileStatement()
        {
            var json = new JObject()
            {
                ["type"] = "DoWhileStatement",
                ["test"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            var dowhile = Read<JSDoWhileStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(dowhile.Test).Name);
            Assert.IsType<JSReturnStatement>(dowhile.Body);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ExpressionStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ExpressionStatement",
                ["expression"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                }
            };

            var directive = Read<JSExpressionStatement>(json);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(directive.Expression).Value));
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ForInStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ForInStatement",
                ["left"] = new JObject()
                {
                    ["type"] = "VariableDeclaration",
                    ["kind"] = "var",
                    ["declarations"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "VariableDeclarator",
                            ["id"] = new JObject()
                            {
                                ["type"] = "Identifier",
                                ["name"] = "a"
                            },
                            ["init"] = null
                        }
                    }
                },
                ["right"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            var forin = Read<JSForInStatement>(json);
            var declaration = Assert.IsType<JSVariableDeclaration>(forin.Left);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(Assert.Single(declaration.Declarations).Identifier).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(forin.Right).Name);
            Assert.IsType<JSReturnStatement>(forin.Body);

            json = new JObject()
            {
                ["type"] = "ForInStatement",
                ["left"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["right"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            forin = Read<JSForInStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(forin.Left).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(forin.Right).Name);
            Assert.IsType<JSReturnStatement>(forin.Body);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ForStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ForStatement",
                ["init"] = new JObject()
                {
                    ["type"] = "VariableDeclaration",
                    ["kind"] = "var",
                    ["declarations"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "VariableDeclarator",
                            ["id"] = new JObject()
                            {
                                ["type"] = "Identifier",
                                ["name"] = "a"
                            },
                            ["init"] = null
                        }
                    }
                },
                ["test"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                },
                ["update"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "c"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            var @for = Read<JSForStatement>(json);
            var declaration = Assert.IsType<JSVariableDeclaration>(@for.Initializer);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(Assert.Single(declaration.Declarations).Identifier).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(@for.Test).Name);
            Assert.Equal("c", Assert.IsType<JSIdentifier>(@for.Update).Name);
            Assert.IsType<JSReturnStatement>(@for.Body);

            json = new JObject()
            {
                ["type"] = "ForStatement",
                ["init"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["test"] = null,
                ["update"] = null,
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            @for = Read<JSForStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@for.Initializer).Name);
            Assert.Null(@for.Test);
            Assert.Null(@for.Update);
            Assert.IsType<JSReturnStatement>(@for.Body);

            json = new JObject()
            {
                ["type"] = "ForStatement",
                ["init"] = null,
                ["test"] = null,
                ["update"] = null,
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            @for = Read<JSForStatement>(json);
            Assert.Null(@for.Initializer);
            Assert.Null(@for.Test);
            Assert.Null(@for.Update);
            Assert.IsType<JSReturnStatement>(@for.Body);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_FunctionDeclaration()
        {
            var json = new JObject()
            {
                ["type"] = "FunctionDeclaration",
                ["id"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["params"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "b"
                    },
                    new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "c"
                    }
                },
                ["body"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "ReturnStatement",
                            ["argument"] = null
                        }
                    }
                }
            };

            var function = Read<JSFunctionDeclaration>(json);
            Assert.Equal("a", function.Identifier?.Name);
            Assert.Collection(function.Parameters,
                item => Assert.Equal("b", Assert.IsType<JSIdentifier>(item).Name),
                item => Assert.Equal("c", Assert.IsType<JSIdentifier>(item).Name));
            Assert.IsType<JSReturnStatement>(Assert.Single(function.Body));
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_FunctionExpression()
        {
            var json = new JObject()
            {
                ["type"] = "FunctionExpression",
                ["id"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["params"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "b"
                    },
                    new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "c"
                    }
                },
                ["body"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "ReturnStatement",
                            ["argument"] = null
                        }
                    }
                }
            };

            var function = Read<JSFunctionExpression>(json);
            Assert.Equal("a", function.Identifier?.Name);
            Assert.Collection(function.Parameters,
                item => Assert.Equal("b", Assert.IsType<JSIdentifier>(item).Name),
                item => Assert.Equal("c", Assert.IsType<JSIdentifier>(item).Name));
            Assert.IsType<JSReturnStatement>(Assert.Single(function.Body));

            json["id"] = null;
            function = Read<JSFunctionExpression>(json);
            Assert.Null(function.Identifier);
            Assert.Collection(function.Parameters,
                item => Assert.Equal("b", Assert.IsType<JSIdentifier>(item).Name),
                item => Assert.Equal("c", Assert.IsType<JSIdentifier>(item).Name));
            Assert.IsType<JSReturnStatement>(Assert.Single(function.Body));
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_Identifier()
        {
            var json = new JObject()
            {
                ["type"] = "Identifier",
                ["name"] = "a"
            };

            var identifier = Read<JSIdentifier>(json);
            Assert.Equal("a", identifier.Name);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_IfStatement()
        {
            var json = new JObject()
            {
                ["type"] = "IfStatement",
                ["test"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["consequent"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                },
                ["alternate"] = new JObject()
                {
                    ["type"] = "ContinueStatement",
                    ["label"] = null
                }
            };

            var @if = Read<JSIfStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@if.Test).Name);
            Assert.IsType<JSReturnStatement>(@if.Body);
            Assert.IsType<JSContinueStatement>(@if.Alternate);

            json["alternate"] = new JObject()
            {
                ["type"] = "EmptyStatement"
            };
            @if = Read<JSIfStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@if.Test).Name);
            Assert.IsType<JSReturnStatement>(@if.Body);
            Assert.Null(@if.Alternate);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_LabeledStatement()
        {
            var json = new JObject()
            {
                ["type"] = "LabeledStatement",
                ["label"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            var label = Read<JSLabeledStatement>(json);
            Assert.Equal("a", label.Label?.Name);
            Assert.IsType<JSReturnStatement>(label.Body);
        }

        public static readonly object[][] JSNodeConverter_ReadJson_Literal_Data =
        {
            new object[] { new JObject() { ["type"] = "Literal", ["value"] = "a" }, "a" },
            new object[] { new JObject() { ["type"] = "Literal", ["value"] = true }, true },
            new object[] { new JObject() { ["type"] = "Literal", ["value"] = null }, null },
            new object[] { new JObject() { ["type"] = "Literal", ["value"] = 1 }, 1L },
            new object[] { new JObject() { ["type"] = "Literal", ["regex"] = new JObject()
            {
                ["pattern"] = "a/b\\*",
                ["flags"] = "gimuy"
            }}, new JSRegex("a/b\\*", JSRegexOptions.Global | JSRegexOptions.IgnoreCase | JSRegexOptions.Multiline | JSRegexOptions.Sticky | JSRegexOptions.Unicode) },
            new object[] { new JObject() { ["type"] = "Literal", ["regex"] = new JObject()
            {
                ["pattern"] = "a/b\\*",
                ["flags"] = "g"
            }}, new JSRegex("a/b\\*", JSRegexOptions.Global) },
            new object[] { new JObject() { ["type"] = "Literal", ["regex"] = new JObject()
            {
                ["pattern"] = "a/b\\*",
                ["flags"] = "i"
            }}, new JSRegex("a/b\\*", JSRegexOptions.IgnoreCase) },
            new object[] { new JObject() { ["type"] = "Literal", ["regex"] = new JObject()
            {
                ["pattern"] = "a/b\\*",
                ["flags"] = "m"
            }}, new JSRegex("a/b\\*", JSRegexOptions.Multiline) },
            new object[] { new JObject() { ["type"] = "Literal", ["regex"] = new JObject()
            {
                ["pattern"] = "a/b\\*",
                ["flags"] = "u"
            }}, new JSRegex("a/b\\*", JSRegexOptions.Unicode) },
            new object[] { new JObject() { ["type"] = "Literal", ["regex"] = new JObject()
            {
                ["pattern"] = "a/b\\*",
                ["flags"] = "y"
            }}, new JSRegex("a/b\\*", JSRegexOptions.Sticky) },
        };

        [Theory]
        [MemberData(nameof(JSNodeConverter_ReadJson_Literal_Data))]
        public static void JSNodeConverter_ReadJson_Literal(JObject json, object expected)
        {
            var literal = Read<JSLiteral>(json);
            Assert.Equal(expected, literal.Value);
        }

        [Theory]
        [InlineData("&&", JSBinaryOperator.LogicalAnd)]
        [InlineData("||", JSBinaryOperator.LogicalOr)]
        public static void JSNodeConverter_ReadJson_LogicalExpression(string operatorString, JSBinaryOperator @operator)
        {
            var json = new JObject()
            {
                ["type"] = "LogicalExpression",
                ["operator"] = operatorString,
                ["left"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                },
                ["right"] = new JObject()
                {
                    ["type"] = "LogicalExpression",
                    ["operator"] = operatorString,
                    ["left"] = new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 2
                    },
                    ["right"] = new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 3
                    }
                }
            };

            var binary = Read<JSBinaryExpression>(json);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(binary.Left).Value));
            Assert.Equal(@operator, binary.Operator);

            Assert.Collection(binary.Right,
                item => Assert.Equal(2, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value)),
                item => Assert.Equal(3, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value))
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_MemberExpression()
        {
            var json = new JObject()
            {
                ["type"] = "MemberExpression",
                ["computed"] = true,
                ["object"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["property"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                }
            };

            var member = Read<JSMemberExpression>(json);
            Assert.True(member.IsComputed);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(member.Object).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(Assert.Single(member.Indices)).Name);

            json["computed"] = false;
            member = Read<JSMemberExpression>(json);
            Assert.False(member.IsComputed);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(member.Object).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(Assert.Single(member.Indices)).Name);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_NewExpression()
        {
            var json = new JObject()
            {
                ["type"] = "NewExpression",
                ["callee"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["arguments"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 1
                    },
                    new JObject()
                    {
                        ["type"] = "Literal",
                        ["value"] = 2
                    }
                }
            };

            var call = Read<JSNewExpression>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(call.Callee).Name);
            Assert.Collection(call.Arguments,
                item => Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value)),
                item => Assert.Equal(2, Assert.IsType<long>(Assert.IsType<JSLiteral>(item).Value))
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ObjectExpression()
        {
            var json = new JObject()
            {
                ["type"] = "ObjectExpression",
                ["properties"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Property",
                        ["kind"] = "init",
                        ["key"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "a"
                        },
                        ["value"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "b"
                        }
                    },
                    new JObject()
                    {
                        ["type"] = "Property",
                        ["kind"] = "get",
                        ["key"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "c"
                        },
                        ["value"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "d"
                        }
                    },
                }
            };

            var @object = Read<JSObjectExpression>(json);
            Assert.Collection(@object.Properties,
                item =>
                {
                    Assert.Equal(JSPropertyKind.Initializer, item.Kind);
                    Assert.Equal("a", Assert.IsType<JSIdentifier>(item.Key).Name);
                    Assert.Equal("b", Assert.IsType<JSIdentifier>(item.Value).Name);
                },
                item =>
                {
                    Assert.Equal(JSPropertyKind.Get, item.Kind);
                    Assert.Equal("c", Assert.IsType<JSIdentifier>(item.Key).Name);
                    Assert.Equal("d", Assert.IsType<JSIdentifier>(item.Value).Name);
                }
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_Program()
        {
            var json = new JObject()
            {
                ["type"] = "Program",
                ["body"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "ReturnStatement",
                        ["argument"] = null
                    },
                    new JObject()
                    {
                        ["type"] = "ContinueStatement",
                        ["label"] = null
                    }
                }
            };

            var @object = Read<JSProgram>(json);
            Assert.Collection(@object.Body,
                item => Assert.IsType<JSReturnStatement>(item),
                item => Assert.IsType<JSContinueStatement>(item)
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_Property()
        {
            var json = new JObject()
            {
                ["type"] = "Property",
                ["kind"] = "init",
                ["key"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["value"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                }
            };

            var property = Read<JSProperty>(json);
            Assert.Equal(JSPropertyKind.Initializer, property.Kind);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(property.Key).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(property.Value).Name);

            json = new JObject()
            {
                ["type"] = "Property",
                ["kind"] = "get",
                ["key"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                },
                ["value"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                }
            };
            property = Read<JSProperty>(json);
            Assert.Equal(JSPropertyKind.Get, property.Kind);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(property.Key).Value));
            Assert.Equal("b", Assert.IsType<JSIdentifier>(property.Value).Name);

            json["kind"] = "set";
            property = Read<JSProperty>(json);
            Assert.Equal(JSPropertyKind.Set, property.Kind);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(property.Key).Value));
            Assert.Equal("b", Assert.IsType<JSIdentifier>(property.Value).Name);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ReturnStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ReturnStatement",
                ["argument"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                }
            };

            var @return = Read<JSReturnStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@return.Expression).Name);

            json["argument"] = null;
            @return = Read<JSReturnStatement>(json);
            Assert.Null(@return.Expression);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_SequenceExpression()
        {
            var json = new JObject()
            {
                ["type"] = "SequenceExpression",
                ["expressions"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "a"
                    },
                    new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "b"
                    },
                }
            };

            var @return = Read<JSSequenceExpression>(json);
            Assert.Collection(@return.Expressions,
                item => Assert.Equal("a", Assert.IsType<JSIdentifier>(item).Name),
                item => Assert.Equal("b", Assert.IsType<JSIdentifier>(item).Name)
            );
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_SwitchCase()
        {
            var json = new JObject()
            {
                ["type"] = "SwitchCase",
                ["test"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["consequent"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "ReturnStatement",
                        ["argument"] = null
                    }
                }
            };

            var @case = Read<JSSwitchCase>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@case.Test).Name);
            Assert.IsType<JSReturnStatement>(Assert.Single(@case.Body));

            json["test"] = null;
            @case = Read<JSSwitchCase>(json);
            Assert.Null(@case.Test);
            Assert.IsType<JSReturnStatement>(Assert.Single(@case.Body));
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_SwitchStatement()
        {
            var json = new JObject()
            {
                ["type"] = "SwitchStatement",
                ["discriminant"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["cases"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "SwitchCase",
                        ["test"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "a"
                        },
                        ["consequent"] = new JArray()
                        {
                            new JObject()
                            {
                                ["type"] = "ReturnStatement",
                                ["argument"] = null
                            }
                        }
                    }
                }
            };

            var @switch = Read<JSSwitchStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@switch.Discriminant).Name);
            Assert.Collection(@switch.Cases,
                @case =>
                {
                    Assert.Equal("a", Assert.IsType<JSIdentifier>(@case.Test).Name);
                    Assert.IsType<JSReturnStatement>(Assert.Single(@case.Body));
                });
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ThisExpression()
        {
            var json = new JObject()
            {
                ["type"] = "ThisExpression"
            };

            var @this = Read<JSThisExpression>(json);
            Assert.NotNull(@this);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_ThrowStatement()
        {
            var json = new JObject()
            {
                ["type"] = "ThrowStatement",
                ["argument"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                }
            };

            var @return = Read<JSThrowStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@return.Expression).Name);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_TryStatement()
        {
            var json = new JObject()
            {
                ["type"] = "TryStatement",
                ["block"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "BreakStatement",
                            ["label"] = null
                        }
                    }
                },
                ["handler"] = new JObject()
                {
                    ["type"] = "CatchClause",
                    ["param"] = new JObject()
                    {
                        ["type"] = "Identifier",
                        ["name"] = "a"
                    },
                    ["body"] = new JObject()
                    {
                        ["type"] = "BlockStatement",
                        ["body"] = new JArray()
                        {
                            new JObject()
                            {
                                ["type"] = "ReturnStatement",
                                ["argument"] = null
                            }
                        }
                    }
                },
                ["finalizer"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "ContinueStatement",
                            ["label"] = null
                        }
                    }
                }
            };

            var @try = Read<JSTryStatement>(json);
            Assert.IsType<JSBreakStatement>(Assert.Single(@try.Body));
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@try.Handler?.Parameter).Name);
            Assert.IsType<JSReturnStatement>(Assert.Single(@try.Handler?.Body));
            Assert.IsType<JSContinueStatement>(Assert.Single(@try.Finalizer.Body));

            json["finalizer"] = null;
            @try = Read<JSTryStatement>(json);
            Assert.IsType<JSBreakStatement>(Assert.Single(@try.Body));
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@try.Handler?.Parameter).Name);
            Assert.IsType<JSReturnStatement>(Assert.Single(@try.Handler?.Body));
            Assert.Empty(@try.Finalizer.Body);

            json = new JObject()
            {
                ["type"] = "TryStatement",
                ["block"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "BreakStatement",
                            ["label"] = null
                        }
                    }
                },
                ["handler"] = null,
                ["finalizer"] = new JObject()
                {
                    ["type"] = "BlockStatement",
                    ["body"] = new JArray()
                    {
                        new JObject()
                        {
                            ["type"] = "ContinueStatement",
                            ["label"] = null
                        }
                    }
                }
            };
            @try = Read<JSTryStatement>(json);
            Assert.IsType<JSBreakStatement>(Assert.Single(@try.Body));
            Assert.Null(@try.Handler);
            Assert.IsType<JSContinueStatement>(Assert.Single(@try.Finalizer.Body));
        }

        [Theory]
        [InlineData("+", JSUnaryOperator.Positive)]
        [InlineData("-", JSUnaryOperator.Negative)]
        [InlineData("!", JSUnaryOperator.LogicalNot)]
        [InlineData("~", JSUnaryOperator.BitwiseNot)]
        [InlineData("typeof", JSUnaryOperator.TypeOf)]
        [InlineData("void", JSUnaryOperator.Void)]
        [InlineData("delete", JSUnaryOperator.Delete)]
        public static void JSNodeConverter_ReadJson_UnaryExpression(string operatorString, JSUnaryOperator @operator)
        {
            var json = new JObject()
            {
                ["type"] = "UnaryExpression",
                ["operator"] = operatorString,
                ["prefix"] = true,
                ["argument"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                }
            };

            var unary = Read<JSUnaryExpression>(json);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(unary.Expression).Value));
            Assert.Equal(@operator, unary.Operator);
        }

        [Theory]
        [InlineData(true, "++", JSUnaryOperator.PreIncrement)]
        [InlineData(true, "--", JSUnaryOperator.PreDecrement)]
        [InlineData(false, "++", JSUnaryOperator.PostIncrement)]
        [InlineData(false, "--", JSUnaryOperator.PostDecrement)]
        public static void JSNodeConverter_ReadJson_UpdateExpression(bool prefix, string operatorString, JSUnaryOperator @operator)
        {
            var json = new JObject()
            {
                ["type"] = "UpdateExpression",
                ["operator"] = operatorString,
                ["prefix"] = prefix,
                ["argument"] = new JObject()
                {
                    ["type"] = "Literal",
                    ["value"] = 1
                }
            };

            var unary = Read<JSUnaryExpression>(json);
            Assert.Equal(1, Assert.IsType<long>(Assert.IsType<JSLiteral>(unary.Expression).Value));
            Assert.Equal(@operator, unary.Operator);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_VariableDeclaration()
        {
            var json = new JObject()
            {
                ["type"] = "VariableDeclaration",
                ["kind"] = "var",
                ["declarations"] = new JArray()
                {
                    new JObject()
                    {
                        ["type"] = "VariableDeclarator",
                        ["id"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "a"
                        },
                        ["init"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "b"
                        }
                    },
                    new JObject()
                    {
                        ["type"] = "VariableDeclarator",
                        ["id"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "c"
                        },
                        ["init"] = new JObject()
                        {
                            ["type"] = "Identifier",
                            ["name"] = "d"
                        }
                    }
                }
            };

            var declaration = Read<JSVariableDeclaration>(json);
            Assert.Equal(JSVariableDeclarationKind.Var, declaration.Kind);
            Assert.Collection(declaration.Declarations,
                item =>
                {
                    Assert.Equal("a", Assert.IsType<JSIdentifier>(item.Identifier).Name);
                    Assert.Equal("b", Assert.IsType<JSIdentifier>(item.Initializer).Name);
                },
                item =>
                {
                    Assert.Equal("c", Assert.IsType<JSIdentifier>(item.Identifier).Name);
                    Assert.Equal("d", Assert.IsType<JSIdentifier>(item.Initializer).Name);
                });
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_VariableDeclarator()
        {
            var json = new JObject()
            {
                ["type"] = "VariableDeclarator",
                ["id"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["init"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "b"
                }
            };

            var declarator = Read<JSVariableDeclarator>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(declarator.Identifier).Name);
            Assert.Equal("b", Assert.IsType<JSIdentifier>(declarator.Initializer).Name);

            json = new JObject()
            {
                ["type"] = "VariableDeclarator",
                ["id"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["init"] = null
            };

            declarator = Read<JSVariableDeclarator>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(declarator.Identifier).Name);
            Assert.Null(declarator.Initializer);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_WhileStatement()
        {
            var json = new JObject()
            {
                ["type"] = "WhileStatement",
                ["test"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            var @while = Read<JSWhileStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@while.Test).Name);
            Assert.IsType<JSReturnStatement>(@while.Body);
        }

        [Fact]
        public static void JSNodeConverter_ReadJson_WithStatement()
        {
            var json = new JObject()
            {
                ["type"] = "WithStatement",
                ["object"] = new JObject()
                {
                    ["type"] = "Identifier",
                    ["name"] = "a"
                },
                ["body"] = new JObject()
                {
                    ["type"] = "ReturnStatement",
                    ["argument"] = null
                }
            };

            var @while = Read<JSWithStatement>(json);
            Assert.Equal("a", Assert.IsType<JSIdentifier>(@while.Object).Name);
            Assert.IsType<JSReturnStatement>(@while.Body);
        }
    }
}
