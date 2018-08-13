using System;
using System.IO;
using Xunit;
using static System.FormattableString;
using static SourceCode.Clay.Javascript.Ast.JSFluent;

namespace SourceCode.Clay.Javascript.Ast
{
    public partial class JSWriterTests
    {
        private string Write<T>(T node, bool minify)
            where T : IJSNode
        {
            using (var sw = new StringWriter())
            using (var jw = new JSWriter(sw, "\t"))
            {
                jw.NewLine = "\n";
                jw.Minify = minify;
                jw.WriteNode(node);
                jw.Write(string.Empty);
                jw.Flush();
                return sw.ToString();
            }
        }

        private string WriteAsync<T>(T node, bool minify)
            where T : IJSNode
        {
            using (var sw = new StringWriter())
            using (var jw = new JSWriter(sw, "\t"))
            {
                jw.NewLine = "\n";
                jw.Minify = minify;
                jw.WriteNodeAsync(node).AsTask().Wait();
                jw.Write(string.Empty);
                jw.Flush();
                return sw.ToString();
            }
        }

        private void Test<T>(T node, string minified, string normal = null)
            where T : IJSNode
        {
            if (normal is null) normal = minified;
            var actualMinified = "sync:" + Write(node, true);
            var actualNormal = "sync:" + Write(node, false);
            Assert.Equal("sync:" + minified, actualMinified);
            Assert.Equal("sync:" + normal, actualNormal);

            actualMinified = "async:" + WriteAsync(node, true);
            actualNormal = "async:" + WriteAsync(node, false);
            Assert.Equal("async:" + minified, actualMinified);
            Assert.Equal("async:" + normal, actualNormal);
        }

        [Fact]
        public void JSWriter_Write_ArrayExpression()
        {
            var ast = JSArray(JSLiteral(1), JSLiteral(2));
            Test(ast, "[1,2]", "[\n\t1,\n\t2\n]");

            ast.Elements.Clear();
            Test(ast, "[]");
        }

        public static readonly object[][] JSWriter_Write_BinaryExpression_Data = new[]
        {
            new object[]{ JSLiteral(1).JSAdd(JSLiteral(2), JSLiteral(3)), "+" },
            new object[]{ JSLiteral(1).JSAddAssign(JSLiteral(2), JSLiteral(3)), "+=" },
            new object[]{ JSLiteral(1).JSAssign(JSLiteral(2), JSLiteral(3)), "=" },
            new object[]{ JSLiteral(1).JSBitwiseAnd(JSLiteral(2), JSLiteral(3)), "&" },
            new object[]{ JSLiteral(1).JSBitwiseAndAssign(JSLiteral(2), JSLiteral(3)), "&=" },
            new object[]{ JSLiteral(1).JSBitwiseOr(JSLiteral(2), JSLiteral(3)), "|" },
            new object[]{ JSLiteral(1).JSBitwiseOrAssign(JSLiteral(2), JSLiteral(3)), "|=" },
            new object[]{ JSLiteral(1).JSBitwiseXor(JSLiteral(2), JSLiteral(3)), "^" },
            new object[]{ JSLiteral(1).JSBitwiseXorAssign(JSLiteral(2), JSLiteral(3)), "^=" },
            new object[]{ JSLiteral(1).JSCoercedEquality(JSLiteral(2), JSLiteral(3)), "==" },
            new object[]{ JSLiteral(1).JSCoercedInequality(JSLiteral(2), JSLiteral(3)), "!=" },
            new object[]{ JSLiteral(1).JSDivide(JSLiteral(2), JSLiteral(3)), "/" },
            new object[]{ JSLiteral(1).JSDivideAssign(JSLiteral(2), JSLiteral(3)), "/=" },
            new object[]{ JSLiteral(1).JSGreaterThan(JSLiteral(2), JSLiteral(3)), ">" },
            new object[]{ JSLiteral(1).JSGreaterThanOrEqual(JSLiteral(2), JSLiteral(3)), ">=" },
            new object[]{ JSLiteral(1).JSIdentityEquality(JSLiteral(2), JSLiteral(3)), "===" },
            new object[]{ JSLiteral(1).JSIdentityInequality(JSLiteral(2), JSLiteral(3)), "!==" },
            new object[]{ JSLiteral(1).JSIn(JSLiteral(2), JSLiteral(3)), " in " },
            new object[]{ JSLiteral(1).JSInstanceOf(JSLiteral(2), JSLiteral(3)), " instanceof " },
            new object[]{ JSLiteral(1).JSLessThan(JSLiteral(2), JSLiteral(3)), "<" },
            new object[]{ JSLiteral(1).JSLessThanOrEqual(JSLiteral(2), JSLiteral(3)), "<=" },
            new object[]{ JSLiteral(1).JSLogicalAnd(JSLiteral(2), JSLiteral(3)), "&&" },
            new object[]{ JSLiteral(1).JSLogicalOr(JSLiteral(2), JSLiteral(3)), "||" },
            new object[]{ JSLiteral(1).JSModulus(JSLiteral(2), JSLiteral(3)), "%" },
            new object[]{ JSLiteral(1).JSModulusAssign(JSLiteral(2), JSLiteral(3)), "%=" },
            new object[]{ JSLiteral(1).JSMultiply(JSLiteral(2), JSLiteral(3)), "*" },
            new object[]{ JSLiteral(1).JSMultiplyAssign(JSLiteral(2), JSLiteral(3)), "*=" },
            new object[]{ JSLiteral(1).JSSignedRightShift(JSLiteral(2), JSLiteral(3)), ">>" },
            new object[]{ JSLiteral(1).JSSignedRightShiftAssign(JSLiteral(2), JSLiteral(3)), ">>=" },
            new object[]{ JSLiteral(1).JSSubtract(JSLiteral(2), JSLiteral(3)), "-" },
            new object[]{ JSLiteral(1).JSSubtractAssign(JSLiteral(2), JSLiteral(3)), "-=" },
            new object[]{ JSLiteral(1).JSUnsignedLeftShift(JSLiteral(2), JSLiteral(3)), "<<" },
            new object[]{ JSLiteral(1).JSUnsignedLeftShiftAssign(JSLiteral(2), JSLiteral(3)), "<<=" },
            new object[]{ JSLiteral(1).JSUnsignedRightShift(JSLiteral(2), JSLiteral(3)), ">>>" },
            new object[]{ JSLiteral(1).JSUnsignedRightShiftAssign(JSLiteral(2), JSLiteral(3)), ">>>=" }
        };

        [Theory]
        [MemberData(nameof(JSWriter_Write_BinaryExpression_Data))]
        public void JSWriter_Write_BinaryExpression(JSBinaryExpression ast, string infix)
        {
            Test(ast, Invariant($"(1{infix}2{infix}3)"), Invariant($"(1 {infix.Trim()} 2 {infix.Trim()} 3)"));
        }

        [Fact]
        public void JSWriter_Write_BlockStatement()
        {
            var ast = JSBlock(
                JSReturn(),
                JSReturn()
            );
            Test(ast, "{return;return;}", "{\n\treturn;\n\treturn;\n}\n");

            ast.Body.Clear();
            Test(ast, "{}", "{}\n");
        }

        [Fact]
        public void JSWriter_Write_BreakStatement()
        {
            var ast = JSBreak();
            Test(ast, "break;", "break;\n");
        }

        [Fact]
        public void JSWriter_Write_CallExpression()
        {
            var ast = JSLiteral(1).JSCall(JSLiteral(2), JSLiteral(3));

            Test(ast, "1(2,3)", "1(2, 3)");

            ast.Arguments.Clear();
            Test(ast, "1()", "1()");

            ast.Callee = JSFunction().Add(JSReturn());
            Test(ast, "(function(){return;})()", "(function() {\n\treturn;\n})()");
        }

        [Fact]
        public void JSWriter_Write_CatchClause()
        {
            var ast = JSCatch("e",
                JSReturn(),
                JSReturn()
            );
            Test(ast, "catch(e){return;return;}", "catch (e) {\n\treturn;\n\treturn;\n}\n");
        }

        [Fact]
        public void JSWriter_Write_ConditionalExpression()
        {
            var ast = JSLiteral(1).JSConditional(JSLiteral(2), JSLiteral(3));
            Test(ast, "1?2:3", "1 ? 2 : 3");
        }

        [Fact]
        public void JSWriter_Write_ContinueStatement()
        {
            var ast = JSContinue("a");
            Test(ast, "continue a;", "continue a;\n");

            ast.Label = null;
            Test(ast, "continue;", "continue;\n");
        }

        [Fact]
        public void JSWriter_Write_DebuggerStatement()
        {
            var ast = JSDebugger();
            Test(ast, "debugger;", "debugger;\n");
        }

        [Fact]
        public void JSWriter_Write_Directive()
        {
            var ast = JSDirective("use strict");
            Test(ast, "'use strict';", "'use strict';\n");
        }

        [Fact]
        public void JSWriter_Write_DoWhileStatement()
        {
            var ast = JSDoWhile(JSLiteral(1),
                JSReturn());
            Test(ast, "do return;while(1);", "do\n\treturn;\nwhile (1);\n");

            ast.Body = JSBlock(
                JSReturn(),
                JSReturn()
            );
            Test(ast, "do{return;return;}while(1);", "do {\n\treturn;\n\treturn;\n} while (1);\n");
        }

        [Fact]
        public void JSWriter_Write_EmptyStatement()
        {
            Test<JSStatement>(null, ";", ";\n");
        }

        [Fact]
        public void JSWriter_Write_EmptyExpression()
        {
            Test<JSExpression>(null, "null");
        }

        [Fact]
        public void JSWriter_Write_ExpressionStatement()
        {
            var ast = new JSExpressionStatement(JSLiteral(1));
            Test(ast, "1;", "1;\n");
        }

        [Fact]
        public void JSWriter_Write_ForInStatement()
        {
            var ast = JSForIn(JSVar("a"), JSLiteral(1),
                JSReturn());
            Test(ast, "for(var a in 1)return;", "for (var a in 1)\n\treturn;\n");

            ast.Left = JSIdentifier("a");
            Test(ast, "for(a in 1)return;", "for (a in 1)\n\treturn;\n");

            ast.Body = JSBlock(
                JSReturn(),
                JSReturn()
            );
            Test(ast, "for(a in 1){return;return;}", "for (a in 1) {\n\treturn;\n\treturn;\n}\n");
        }

        [Fact]
        public void JSWriter_Write_ForStatement()
        {
            var ast = JSFor(JSVar("a"), JSLiteral(1), JSLiteral(2),
                JSReturn()
            );
            Test(ast, "for(var a;1;2)return;", "for (var a; 1; 2)\n\treturn;\n");

            ast.Initializer = JSIdentifier("a");
            Test(ast, "for(a;1;2)return;", "for (a; 1; 2)\n\treturn;\n");

            ast.Body = JSBlock(
                JSReturn(),
                JSReturn()
            );
            Test(ast, "for(a;1;2){return;return;}", "for (a; 1; 2) {\n\treturn;\n\treturn;\n}\n");
        }

        [Fact]
        public void JSWriter_Write_FunctionDeclaration()
        {
            var ast = JSFunctionDeclaration("a", "p1", "p2").Add(
                JSDirective("use strict"),
                JSReturn()
            );

            Test(ast, "function a(p1,p2){'use strict';return;}", "function a(p1, p2) {\n\t'use strict';\n\treturn;\n}\n");

            ast.Identifier = null;
            Test(ast, "function(p1,p2){'use strict';return;}", "function(p1, p2) {\n\t'use strict';\n\treturn;\n}\n");
        }

        [Fact]
        public void JSWriter_Write_FunctionExpression()
        {
            var ast = JSFunction("p1", "p2").Add(
                JSDirective("use strict"),
                JSReturn()
            );

            Test(ast, "function(p1,p2){'use strict';return;}", "function(p1, p2) {\n\t'use strict';\n\treturn;\n}");
        }

        [Fact]
        public void JSWriter_Write_Identifier()
        {
            var ast = JSIdentifier("a");
            Test(ast, "a", "a");
        }

        [Fact]
        public void JSWriter_Write_IfStatement()
        {
            var ast = JSIf(JSLiteral(1),
                JSReturn()).
                Else(
                JSContinue());
            Test(ast, "if(1)return;else continue;", "if (1)\n\treturn;\nelse\n\tcontinue;\n");

            ast.Body = JSBlock(
                JSReturn(),
                JSReturn()
            );

            Test(ast, "if(1){return;return;}else continue;", "if (1) {\n\treturn;\n\treturn;\n} else\n\tcontinue;\n");

            ast.Alternate = JSBlock(
                JSReturn(),
                JSReturn()
            );

            Test(ast, "if(1){return;return;}else{return;return;}", "if (1) {\n\treturn;\n\treturn;\n} else {\n\treturn;\n\treturn;\n}\n");

            // Block statement.

            ast.Alternate = JSBlock(
                JSReturn()
            );

            Test(ast, "if(1){return;return;}else return;", "if (1) {\n\treturn;\n\treturn;\n} else {\n\treturn;\n}\n");

            ast.Body = JSBlock(
                JSReturn()
            );

            Test(ast, "if(1)return;else return;", "if (1) {\n\treturn;\n} else {\n\treturn;\n}\n");

            // Expression statement

            ast.Body = new JSExpressionStatement(JSLiteral(2));
            ast.Alternate = new JSExpressionStatement(JSLiteral(3));

            Test(ast, "1?2:3;", "if (1)\n\t2;\nelse\n\t3;\n");

            // No else.

            ast.Alternate = null;

            Test(ast, "if(1)2;", "if (1)\n\t2;\n");
        }

        [Fact]
        public void JSWriter_Write_LabeledStatement()
        {
            var ast = JSLabel("a",
                JSReturn());
            Test(ast, "a:return;", "a:\nreturn;\n");
        }

        public static readonly object[][] JSWriter_Write_Literal_Data = new[]
        {
            new object[] {100.1m, "100.1"},
            new object[] {100.2, "100.2"},
            new object[] {100.3f, "100.3"},
            new object[] {-101L, "-101"},
            new object[] {102UL, "102"},
            new object[] {-103, "-103"},
            new object[] {104U, "104"},
            new object[] {(short)-105, "-105"},
            new object[] {(ushort)106, "106"},
            new object[] {(sbyte)-107, "-107"},
            new object[] {(byte)108, "108"},
            new object[] {true, "true"},
            new object[] {false, "false"},
            new object[] {null, "null"},
            new object[] {new JSRegex("a/b\\*", JSRegexOptions.Global | JSRegexOptions.IgnoreCase | JSRegexOptions.Multiline | JSRegexOptions.Sticky | JSRegexOptions.Unicode), "/a\\/b\\*/gimuy"},
            new object[] {new JSRegex("a/b\\*", JSRegexOptions.Global), "/a\\/b\\*/g"},
            new object[] {new JSRegex("a/b\\*", JSRegexOptions.IgnoreCase), "/a\\/b\\*/i"},
            new object[] {new JSRegex("a/b\\*", JSRegexOptions.Multiline), "/a\\/b\\*/m"},
            new object[] {new JSRegex("a/b\\*", JSRegexOptions.Unicode), "/a\\/b\\*/u"},
            new object[] {new JSRegex("a/b\\*", JSRegexOptions.Sticky), "/a\\/b\\*/y"},
            new object[] { "\\'\n\r\t\b\f\v\0\x16\x8f\x4e00", "'\\\\\\'\\n\\r\\t\\b\\f\\v\\0\\x16\\x8F\x4e00'" },
            new object[] {new TimeSpan(10, 0, 0), "10:00:00"}
        };

        [Theory]
        [MemberData(nameof(JSWriter_Write_Literal_Data))]
        public void JSWriter_Write_Literal(object value, string expected)
        {
            var ast = JSLiteral(value);
            Test(ast, expected);
        }

        [Fact]
        public void JSWriter_Write_MemberExpression()
        {
            var ast = JSLiteral(1).JSIndexer(JSLiteral(2), JSLiteral(3));
            Test(ast, "1[2][3]");

            ast = JSLiteral(1).JSMember("a", "b");
            Test(ast, "1.a.b");
        }

        [Fact]
        public void JSWriter_Write_NewExpression()
        {
            var ast = JSLiteral(1).JSNew(JSLiteral(2), JSLiteral(3));

            Test(ast, "new 1(2,3)", "new 1(2, 3)");

            ast.Arguments.Clear();
            Test(ast, "new 1()", "new 1()");

            ast.Callee = JSFunction().Add(JSReturn());
            Test(ast, "new (function(){return;})()", "new (function() {\n\treturn;\n})()");
        }

        [Fact]
        public void JSWriter_Write_ObjectExpression()
        {
            var ast = JSObject(
                JSLiteral("a").JSProperty(JSLiteral(1)),
                JSIdentifier("b").JSProperty(JSLiteral(2))
            );
            Test(ast, "{'a':1,b:2}", "{\n\t'a': 1,\n\tb: 2\n}");

            ast.Properties.Clear();
            Test(ast, "{}");
        }

        [Fact]
        public void JSWriter_Write_Program()
        {
            var ast = JSProgram(JSReturn(), JSReturn());
            Test(ast, "return;return;", "return;\nreturn;\n");
        }

        [Fact]
        public void JSWriter_Write_Property()
        {
            var ast = JSProperty(JSLiteral("a"), JSLiteral(1));
            Test(ast, "'a':1", "'a': 1");

            ast.Key = JSIdentifier("a");
            Test(ast, "a:1", "a: 1");
        }

        [Fact]
        public void JSWriter_Write_ReturnStatement()
        {
            var ast = JSReturn(JSLiteral(1));
            Test(ast, "return 1;", "return 1;\n");

            ast.Expression = null;
            Test(ast, "return;", "return;\n");
        }

        [Fact]
        public void JSWriter_Write_SequenceExpression()
        {
            var ast = JSSequence(JSLiteral(1), JSLiteral(2));
            Test(ast, "1,2", "1, 2");
        }

        [Fact]
        public void JSWriter_Write_SwitchCase()
        {
            var ast = JSCase(JSLiteral(1),
                JSReturn());
            Test(ast, "case 1:return;", "case 1:\n\treturn;\n");

            ast = JSDefaultCase(JSReturn());
            Test(ast, "default:return;", "default:\n\treturn;\n");
        }

        [Fact]
        public void JSWriter_Write_SwitchStatement()
        {
            var ast = JSSwitch(JSLiteral(1),
                JSCase(JSLiteral(2),
                    JSReturn()),
                JSDefaultCase(
                    JSContinue())
            );
            Test(ast, "switch(1){case 2:return;default:continue;}", "switch (1) {\n\tcase 2:\n\t\treturn;\n\tdefault:\n\t\tcontinue;\n}\n");
        }

        [Fact]
        public void JSWriter_Write_ThisExpression()
        {
            var ast = JSThis();
            Test(ast, "this");
        }

        [Fact]
        public void JSWriter_Write_ThrowStatement()
        {
            var ast = JSThrow(JSLiteral(1));
            Test(ast, "throw 1;", "throw 1;\n");
        }

        [Fact]
        public void JSWriter_Write_TryStatement()
        {
            var ast = JSTry(
                JSReturn()
            ).Catch(JSIdentifier("e"),
                JSContinue()
            ).Finally(
                JSBreak()
            );
            Test(ast, "try{return;}catch(e){continue;}finally{break;}", "try {\n\treturn;\n} catch (e) {\n\tcontinue;\n} finally {\n\tbreak;\n}\n");
        }

        public static readonly object[][] JSWriter_Write_UnaryExpression_Data = new[]
        {
            new object[]{ JSValue(1).JSBitwiseNot(), "~1" },
            new object[]{ JSValue(1).JSDelete(), "delete 1" },
            new object[]{ JSValue(1).JSLogicalNot(), "!1" },
            new object[]{ JSValue(1).JSNegative(), "-1" },
            new object[]{ JSValue(1).JSPositive(), "+1" },
            new object[]{ JSValue(1).JSPostDecrement(), "1--" },
            new object[]{ JSValue(1).JSPostIncrement(), "1++" },
            new object[]{ JSValue(1).JSPreDecrement(), "--1" },
            new object[]{ JSValue(1).JSPreIncrement(), "++1" },
            new object[]{ JSValue(1).JSTypeOf(), "typeof 1" },
            new object[]{ JSValue(1).JSVoid(), "void 1" }
        };

        [Theory]
        [MemberData(nameof(JSWriter_Write_UnaryExpression_Data))]
        public void JSWriter_Write_UnaryExpression(JSUnaryExpression ast, string expected)
        {
            Test(ast, expected);
        }

        [Fact]
        public void JSWriter_Write_VariableDeclaration()
        {
            var ast = JSVarList()
                .Add("a")
                .Add("b", JSLiteral(1));
            Test(ast, "var a,b=1;", "var a,\n\tb = 1;\n");
        }

        [Fact]
        public void JSWriter_Write_VariableDeclarator()
        {
            var ast = new JSVariableDeclarator(new JSIdentifier("a"), new JSLiteral(1));
            Test(ast, "a=1", "a = 1");

            ast.Initializer = null;
            Test(ast, "a");
        }

        [Fact]
        public void JSWriter_Write_WhileStatement()
        {
            var ast = JSWhile(JSLiteral(1),
                JSReturn());
            Test(ast, "while(1)return;", "while (1)\n\treturn;\n");

            ast.Body = JSBlock(
                JSReturn(),
                JSReturn()
            );
            Test(ast, "while(1){return;return;}", "while (1) {\n\treturn;\n\treturn;\n}\n");
        }

        [Fact]
        public void JSWriter_Write_WithStatement()
        {
            var ast = JSWith(JSLiteral(1),
                JSReturn());
            Test(ast, "with(1)return;", "with (1)\n\treturn;\n");

            ast.Body = JSBlock(
                JSReturn(),
                JSReturn()
            );
            Test(ast, "with(1){return;return;}", "with (1) {\n\treturn;\n\treturn;\n}\n");
        }
    }
}
