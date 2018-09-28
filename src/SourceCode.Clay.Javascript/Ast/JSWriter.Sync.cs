using System;
using System.Globalization;

namespace SourceCode.Clay.Javascript.Ast
{
    partial class JSWriter
    {
        public override void Write(object value)
        {
            if (value is IJSNode node) WriteNode(node);
            else base.Write(value);
        }

        public virtual void WriteNode<T>(T node)
            where T : IJSNode
        {
            if (Equals(node, default(T))) // TODO: ReferenceEquals
            {
                if (typeof(T) == typeof(JSStatement))
                {
                    if (Minify) Write(";");
                    else WriteLine(";");
                }
                else
                {
                    WriteLiteral();
                }
                return;
            }

            switch (node.Type)
            {
                case JSNodeType.Identifier:
                    WriteNode((JSIdentifier)(IJSNode)node);
                    break;
                case JSNodeType.Literal:
                    WriteNode((JSLiteral)(IJSNode)node);
                    break;
                case JSNodeType.Program:
                    WriteNode((JSProgram)(IJSNode)node);
                    break;
                case JSNodeType.ExpressionStatement:
                    WriteNode((JSExpressionStatement)(IJSNode)node);
                    break;
                case JSNodeType.BlockStatement:
                    WriteBlock((JSBlockStatement)(IJSNode)node, false, true);
                    break;
                case JSNodeType.DebuggerStatement:
                    WriteNode((JSDebuggerStatement)(IJSNode)node);
                    break;
                case JSNodeType.WithStatement:
                    WriteNode((JSWithStatement)(IJSNode)node);
                    break;
                case JSNodeType.ReturnStatement:
                    WriteNode((JSReturnStatement)(IJSNode)node);
                    break;
                case JSNodeType.LabeledStatement:
                    WriteNode((JSLabeledStatement)(IJSNode)node);
                    break;
                case JSNodeType.BreakStatement:
                    WriteNode((JSBreakStatement)(IJSNode)node);
                    break;
                case JSNodeType.ContinueStatement:
                    WriteNode((JSContinueStatement)(IJSNode)node);
                    break;
                case JSNodeType.IfStatement:
                    WriteNode((JSIfStatement)(IJSNode)node);
                    break;
                case JSNodeType.SwitchStatement:
                    WriteNode((JSSwitchStatement)(IJSNode)node);
                    break;
                case JSNodeType.SwitchCase:
                    WriteNode((JSSwitchCase)(IJSNode)node);
                    break;
                case JSNodeType.ThrowStatement:
                    WriteNode((JSThrowStatement)(IJSNode)node);
                    break;
                case JSNodeType.TryStatement:
                    WriteNode((JSTryStatement)(IJSNode)node);
                    break;
                case JSNodeType.CatchClause:
                    WriteNode((JSCatchClause)(IJSNode)node);
                    break;
                case JSNodeType.WhileStatement:
                    WriteNode((JSWhileStatement)(IJSNode)node);
                    break;
                case JSNodeType.DoWhileStatement:
                    WriteNode((JSDoWhileStatement)(IJSNode)node);
                    break;
                case JSNodeType.ForStatement:
                    WriteNode((JSForStatement)(IJSNode)node);
                    break;
                case JSNodeType.ForInStatement:
                    WriteNode((JSForInStatement)(IJSNode)node);
                    break;
                case JSNodeType.FunctionDeclaration:
                    WriteNode((JSFunctionDeclaration)(IJSNode)node);
                    break;
                case JSNodeType.VariableDeclaration:
                    WriteNode((JSVariableDeclaration)(IJSNode)node);
                    break;
                case JSNodeType.VariableDeclarator:
                    WriteNode((JSVariableDeclarator)(IJSNode)node);
                    break;
                case JSNodeType.ThisExpression:
                    WriteNode((JSThisExpression)(IJSNode)node);
                    break;
                case JSNodeType.ArrayExpression:
                    WriteNode((JSArrayExpression)(IJSNode)node);
                    break;
                case JSNodeType.ObjectExpression:
                    WriteNode((JSObjectExpression)(IJSNode)node);
                    break;
                case JSNodeType.Property:
                    WriteNode((JSProperty)(IJSNode)node);
                    break;
                case JSNodeType.FunctionExpression:
                    WriteNode((JSFunctionExpression)(IJSNode)node);
                    break;
                case JSNodeType.UnaryExpression:
                    WriteNode((JSUnaryExpression)(IJSNode)node);
                    break;
                case JSNodeType.BinaryExpression:
                    WriteNode((JSBinaryExpression)(IJSNode)node);
                    break;
                case JSNodeType.MemberExpression:
                    WriteNode((JSMemberExpression)(IJSNode)node);
                    break;
                case JSNodeType.ConditionalExpression:
                    WriteNode((JSConditionalExpression)(IJSNode)node);
                    break;
                case JSNodeType.CallExpression:
                    WriteNode((JSCallExpression)(IJSNode)node);
                    break;
                case JSNodeType.NewExpression:
                    WriteNode((JSNewExpression)(IJSNode)node);
                    break;
                case JSNodeType.SequenceExpression:
                    WriteNode((JSSequenceExpression)(IJSNode)node);
                    break;
                default: throw new NotSupportedException($"The node type {node.Type} is not supported.");
            }
        }

        protected virtual void WriteNode(JSSequenceExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            for (var i = 0; i < node.Expressions.Count; i++)
            {
                if (i != 0) Write(Minify ? "," : ", ");
                WriteNode(node.Expressions[i]);
            }
        }

        protected virtual void WriteNode(JSNewExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("new ");
            WriteNode((JSCallExpression)node);
        }

        protected virtual void WriteNode(JSCallExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            if (!(node.Callee is null))
            {
                var isFunction = node.Callee is IJSFunction;
                if (isFunction) Write("(");
                WriteNode(node.Callee);
                if (isFunction) Write(")");
            }
            Write("(");
            for (var i = 0; i < node.Arguments.Count; i++)
            {
                if (i != 0) Write(Minify ? "," : ", ");
                WriteNode(node.Arguments[i]);
            }
            Write(")");
        }

        protected virtual void WriteNode(JSConditionalExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            WriteNode(node.Test);
            Write(Minify ? "?" : " ? ");
            WriteNode(node.Consequent);
            Write(Minify ? ":" : " : ");
            WriteNode(node.Alternate);
        }

        protected virtual void WriteNode(JSMemberExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            WriteNode(node.Object);
            for (var i = 0; i < node.Indices.Count; i++)
            {
                Write(node.IsComputed ? "[" : ".");
                WriteNode(node.Indices[i]);
                if (node.IsComputed) Write("]");
            }
        }

        protected virtual void WriteNode(JSBinaryExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("(");
            WriteNode(node.Left);
            for (var i = 0; i < node.Right.Count; i++)
            {
                WriteNode(node.Operator);
                WriteNode(node.Right[i]);
            }
            Write(")");
        }

        protected virtual void WriteNode(JSBinaryOperator @operator)
        {
            switch (@operator)
            {
                case JSBinaryOperator.IdentityEquality:
                    Write(Minify ? "===" : " === ");
                    break;
                case JSBinaryOperator.IdentityInequality:
                    Write(Minify ? "!==" : " !== ");
                    break;
                case JSBinaryOperator.CoercedEquality:
                    Write(Minify ? "==" : " == ");
                    break;
                case JSBinaryOperator.CoercedInequality:
                    Write(Minify ? "!=" : " != ");
                    break;
                case JSBinaryOperator.LessThan:
                    Write(Minify ? "<" : " < ");
                    break;
                case JSBinaryOperator.LessThanOrEqual:
                    Write(Minify ? "<=" : " <= ");
                    break;
                case JSBinaryOperator.GreaterThan:
                    Write(Minify ? ">" : " > ");
                    break;
                case JSBinaryOperator.GreaterThanOrEqual:
                    Write(Minify ? ">=" : " >= ");
                    break;
                case JSBinaryOperator.UnsignedLeftShift:
                    Write(Minify ? "<<" : " << ");
                    break;
                case JSBinaryOperator.SignedRightShift:
                    Write(Minify ? ">>" : " >> ");
                    break;
                case JSBinaryOperator.UnsignedRightShift:
                    Write(Minify ? ">>>" : " >>> ");
                    break;
                case JSBinaryOperator.Add:
                    Write(Minify ? "+" : " + ");
                    break;
                case JSBinaryOperator.Subtract:
                    Write(Minify ? "-" : " - ");
                    break;
                case JSBinaryOperator.Multiply:
                    Write(Minify ? "*" : " * ");
                    break;
                case JSBinaryOperator.Divide:
                    Write(Minify ? "/" : " / ");
                    break;
                case JSBinaryOperator.Modulus:
                    Write(Minify ? "%" : " % ");
                    break;
                case JSBinaryOperator.BitwiseOr:
                    Write(Minify ? "|" : " | ");
                    break;
                case JSBinaryOperator.BitwiseXor:
                    Write(Minify ? "^" : " ^ ");
                    break;
                case JSBinaryOperator.BitwiseAnd:
                    Write(Minify ? "&" : " & ");
                    break;
                case JSBinaryOperator.In:
                    Write(" in ");
                    break;
                case JSBinaryOperator.InstanceOf:
                    Write(" instanceof ");
                    break;
                case JSBinaryOperator.Assign:
                    Write(Minify ? "=" : " = ");
                    break;
                case JSBinaryOperator.AddAssign:
                    Write(Minify ? "+=" : " += ");
                    break;
                case JSBinaryOperator.SubtractAssign:
                    Write(Minify ? "-=" : " -= ");
                    break;
                case JSBinaryOperator.MultiplyAssign:
                    Write(Minify ? "*=" : " *= ");
                    break;
                case JSBinaryOperator.DivideAssign:
                    Write(Minify ? "/=" : " /= ");
                    break;
                case JSBinaryOperator.ModulusAssign:
                    Write(Minify ? "%=" : " %= ");
                    break;
                case JSBinaryOperator.UnsignedLeftShiftAssign:
                    Write(Minify ? "<<=" : " <<= ");
                    break;
                case JSBinaryOperator.SignedRightShiftAssign:
                    Write(Minify ? ">>=" : " >>= ");
                    break;
                case JSBinaryOperator.UnsignedRightShiftAssign:
                    Write(Minify ? ">>>=" : " >>>= ");
                    break;
                case JSBinaryOperator.BitwiseOrAssign:
                    Write(Minify ? "|=" : " |= ");
                    break;
                case JSBinaryOperator.BitwiseXorAssign:
                    Write(Minify ? "^=" : " ^= ");
                    break;
                case JSBinaryOperator.BitwiseAndAssign:
                    Write(Minify ? "&=" : " &= ");
                    break;
                case JSBinaryOperator.LogicalAnd:
                    Write(Minify ? "&&" : " && ");
                    break;
                case JSBinaryOperator.LogicalOr:
                    Write(Minify ? "||" : " || ");
                    break;
                default: throw new NotSupportedException($"The binary operator {@operator} is not supported.");
            }
        }

        protected virtual void WriteNode(JSUnaryExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            switch (node.Operator)
            {
                case JSUnaryOperator.Negative:
                    Write("-");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.Positive:
                    Write("+");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.LogicalNot:
                    Write("!");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.BitwiseNot:
                    Write("~");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.TypeOf:
                    Write("typeof ");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.Void:
                    Write("void ");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.Delete:
                    Write("delete ");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.PreIncrement:
                    Write("++");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.PreDecrement:
                    Write("--");
                    WriteNode(node.Expression);
                    break;
                case JSUnaryOperator.PostIncrement:
                    WriteNode(node.Expression);
                    Write("++");
                    break;
                case JSUnaryOperator.PostDecrement:
                    WriteNode(node.Expression);
                    Write("--");
                    break;
                default: throw new ArgumentOutOfRangeException($"The unary operator {node.Operator} is not supported.");
            }
        }

        protected virtual void WriteNode(JSFunctionExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("function(");
            for (var i = 0; i < node.Parameters.Count; i++)
            {
                if (i != 0) Write(Minify ? "," : ", ");
                WriteNode(node.Parameters[i]);
            }
            Write(Minify ? ")" : ") ");
            WriteBlock(node, false, false);
        }

        protected virtual void WriteNode(JSProperty node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            switch (node.Key)
            {
                case var d when d.IsItem1: WriteNode(d.Item1); break;
                case var d when d.IsItem2: WriteNode(d.Item2); break;
                default: WriteLiteral(); break;
            }

            Write(Minify ? ":" : ": ");
            WriteNode(node.Value);
        }

        protected virtual void WriteNode(JSObjectExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            if (node.Properties.Count == 0)
            {
                Write("{}");
                return;
            }

            if (Minify) Write("{");
            else
            {
                WriteLine("{");
                Indent++;
            }

            for (var i = 0; i < node.Properties.Count; i++)
            {
                if (i != 0)
                {
                    if (Minify) Write(",");
                    else WriteLine(",");
                }
                WriteNode(node.Properties[i]);
            }

            if (Minify) Write("}");
            else
            {
                WriteLine();
                Indent--;
                Write("}");
            }
        }

        protected virtual void WriteNode(JSArrayExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            if (node.Elements.Count == 0)
            {
                Write("[]");
                return;
            }

            if (Minify) Write("[");
            else
            {
                WriteLine("[");
                Indent++;
            }

            for (var i = 0; i < node.Elements.Count; i++)
            {
                if (i != 0)
                {
                    if (Minify) Write(",");
                    else WriteLine(",");
                }
                WriteNode(node.Elements[i]);
            }

            if (Minify) Write("]");
            else
            {
                Indent--;
                WriteLine();
                Write("]");
            }
        }

        protected virtual void WriteNode(JSThisExpression node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("this");
        }

        protected virtual void WriteNode(JSVariableDeclarator node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            WriteNode(node.Identifier);
            if (!(node.Initializer is null))
            {
                Write(Minify ? "=" : " = ");
                WriteNode(node.Initializer);
            }
        }

        protected virtual void WriteNode(JSVariableDeclaration node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            WriteNode(node.Kind);
            if (!Minify) Indent++;
            for (var i = 0; i < node.Declarations.Count; i++)
            {
                if (i != 0)
                {
                    if (Minify) Write(",");
                    else WriteLine(",");
                }
                WriteNode(node.Declarations[i]);
            }
            if (Minify) Write(";");
            else
            {
                Indent--;
                WriteLine(";");
            }
        }

        protected virtual void WriteNode(JSVariableDeclarationKind kind)
        {
            Write("var ");
        }

        protected virtual void WriteNode(JSFunctionDeclaration node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("function");
            if (!(node.Identifier is null))
            {
                Write(" ");
                WriteNode(node.Identifier);
            }
            Write("(");
            for (var i = 0; i < node.Parameters.Count; i++)
            {
                if (i != 0) Write(Minify ? "," : ", ");
                WriteNode(node.Parameters[i]);
            }
            Write(Minify ? ")" : ") ");
            WriteBlock(node, false, true);
        }

        protected virtual void WriteNode(JSForInStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(Minify ? "for(" : "for (");

            switch (node.Left)
            {
                case var d when d.IsItem1:
                    WriteNode(d.Item1.Kind);
                    if (d.Item1.Declarations.Count > 0 && !(d.Item1.Declarations[0] is null))
                        WriteNode(d.Item1.Declarations[0]);
                    break;

                case var d when d.IsItem2:
                    WriteNode(d.Item2);
                    break;

                default:
                    WriteLiteral();
                    break;
            }

            Write(" in ");
            WriteNode(node.Right);
            Write(")");
            WriteBody(node, false, true);
        }

        protected virtual bool WriteBody(JSBodyStatement node, bool spacer, bool newLine)
        {
            if (node.Body is JSBlockStatement block)
            {
                if (Minify && block.Body.Count == 1)
                {
                    WriteNode(block.Body[0]);
                    return false;
                }
                WriteBlock(block, spacer || !Minify, newLine);
                return true;
            }
            else if (!(node.Body is null))
            {
                if (Minify && spacer) Write(" ");
                else if (!Minify)
                {
                    WriteLine();
                    Indent++;
                }

                WriteNode(node.Body);

                if (!Minify) Indent--;
            }
            return false;
        }

        protected virtual void WriteNode(JSForStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(Minify ? "for(" : "for (");

            switch (node.Initializer)
            {
                case var d when d.IsItem1:
                    WriteNode(d.Item1.Kind);
                    if (d.Item1.Declarations.Count > 0 && !(d.Item1.Declarations[0] is null))
                        WriteNode(d.Item1.Declarations[0]);
                    break;
                case var d when d.IsItem2:
                    WriteNode(d.Item2);
                    break;
            }

            Write(Minify ? ";" : "; ");
            if (!(node.Test is null)) WriteNode(node.Test);
            Write(Minify ? ";" : "; ");
            if (!(node.Test is null)) WriteNode(node.Update);
            Write(")");
            WriteBody(node, false, true);
        }

        protected virtual void WriteNode(JSDoWhileStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            Write("do");
            var isBlock = WriteBody(node, true, false);
            if (Minify) Write("while(");
            else if (isBlock) Write(" while (");
            else Write("while (");
            if (!(node.Test is null)) WriteNode(node.Test);
            if (Minify) Write(");");
            else WriteLine(");");
        }

        protected virtual void WriteNode(JSWhileStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(Minify ? "while(" : "while (");
            WriteNode(node.Test);
            Write(")");
            WriteBody(node, false, true);
        }

        protected virtual void WriteNode(JSCatchClause node) => WriteNode(node, true);

        protected virtual void WriteNode(JSCatchClause node, bool newLine)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(Minify ? "catch(" : "catch (");
            WriteNode(node.Parameter);
            Write(")");
            WriteBlock(node, true, newLine);
        }

        protected virtual void WriteNode(JSTryStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("try");
            WriteBlock(node, true, node.Handler is null && node.Finalizer is null);
            if (!(node.Handler is null))
            {
                if (!Minify) Write(" ");
                WriteNode(node.Handler, node.Finalizer is null);
            }
            if (node.Finalizer.Body.Count > 0)
            {
                Write(Minify ? "finally" : " finally ");
                WriteNode(node.Finalizer);
            }
        }

        protected virtual void WriteNode(JSThrowStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("throw ");
            WriteNode(node.Expression);
            if (Minify) Write(";");
            else WriteLine(";");
        }

        protected virtual void WriteNode(JSSwitchCase node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            if (!(node.Test is null))
            {
                Write("case ");
                WriteNode(node.Test);
            }
            else
                Write("default");

            if (Minify) Write(":");
            else
            {
                WriteLine(":");
                Indent++;
            }

            for (var i = 0; i < node.Body.Count; i++)
            {
                WriteNode(node.Body[i]);
            }

            if (!Minify) Indent--;
        }

        protected virtual void WriteNode(JSSwitchStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(Minify ? "switch(" : "switch (");
            WriteNode(node.Discriminant);
            Write(Minify ? "){" : ") {");
            if (!Minify)
            {
                WriteLine();
                Indent++;
            }
            for (var i = 0; i < node.Cases.Count; i++)
            {
                JSSwitchCase @case = node.Cases[i];
                WriteNode(@case);
            }
            if (Minify) Write("}");
            else
            {
                Indent--;
                WriteLine("}");
            }
        }

        protected virtual void WriteNode(JSIfStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            if (Minify &&
                node.Body is JSExpressionStatement consequent &&
                node.Alternate is JSExpressionStatement alternate)
            {
                WriteNode(node.Test);
                Write("?");
                WriteNode(consequent.Expression);
                Write(":");
                WriteNode(alternate.Expression);
                Write(";");
                return;
            }

            Write(Minify ? "if(" : "if (");
            WriteNode(node.Test);
            Write(")");

            var isBlock = WriteBody(node, false, node.Alternate is null);

            if (!(node.Alternate is null))
            {
                if (isBlock && !Minify) Write(" ");
                Write("else");
                if (node.Alternate is JSBlockStatement block)
                {
                    if (Minify && block.Body.Count == 1)
                    {
                        Write(" ");
                        WriteNode(block.Body[0]);
                        return;
                    }

                    if (!Minify) Write(" ");
                    WriteNode(node.Alternate);
                    return;
                }

                if (Minify) Write(" ");
                else
                {
                    WriteLine();
                    Indent++;
                }

                WriteNode(node.Alternate);
                if (!Minify) Indent--;
            }
        }

        protected virtual void WriteNode(JSContinueStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(node.Label is null ? "continue" : "continue ");
            if (!(node.Label is null)) WriteNode(node.Label);
            if (Minify) Write(";");
            else WriteLine(";");
        }

        protected virtual void WriteNode(JSBreakStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(node.Label is null ? "break" : "break ");
            if (!(node.Label is null)) WriteNode(node.Label);
            if (Minify) Write(";");
            else WriteLine(";");
        }

        protected virtual void WriteNode(JSLabeledStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            if (!(node.Label is null)) WriteNode(node.Label);
            if (Minify) Write(":");
            else WriteLine(":");
            WriteNode(node.Body);
        }

        protected virtual void WriteNode(JSReturnStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write("return");
            if (!(node.Expression is null))
            {
                Write(" ");
                WriteNode(node.Expression);
            }
            if (Minify) Write(";");
            else WriteLine(";");
        }

        protected virtual void WriteNode(JSWithStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(Minify ? "with(" : "with (");
            WriteNode(node.Object);
            Write(")");
            WriteBody(node, false, true);
        }

        protected virtual void WriteNode(JSDebuggerStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            if (Minify) Write("debugger;");
            else WriteLine("debugger;");
        }

        protected virtual void WriteNode(JSExpressionStatement node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            if (!(node.Expression is null)) WriteNode(node.Expression);
            if (Minify) Write(";");
            else WriteLine(";");
        }

        protected virtual void WriteBlock(IJSBlock node, bool spacer, bool newLine)
        {
            if (!Minify && spacer) Write(" ");

            if (node.Body.Count == 0)
            {
                if (!newLine || Minify) Write("{}");
                else WriteLine("{}");
                return;
            }

            if (Minify) Write("{");
            else
            {
                WriteLine("{");
                Indent++;
            }

            for (var i = 0; i < node.Body.Count; i++)
            {
                WriteNode(node.Body[i]);
            }

            if (!Minify) Indent--;
            if (!newLine || Minify) Write("}");
            else WriteLine("}");
        }

        protected virtual void WriteNode(IJSFunction node) => WriteNode(node, true);

        protected virtual void WriteNode(IJSFunction node, bool newLine)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            if (node.Body.Count == 0)
            {
                if (!newLine || Minify) Write("{}");
                else WriteLine("{}");
                return;
            }

            if (Minify) Write("{");
            else
            {
                WriteLine("{");
                Indent++;
            }

            for (var i = 0; i < node.Body.Count; i++)
            {
                WriteNode(node.Body[i]);
            }

            if (!Minify) Indent--;
            if (!newLine || Minify) Write("}");
            else WriteLine("}");
        }

        protected virtual void WriteNode(JSProgram node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            for (var i = 0; i < node.Body.Count; i++)
            {
                JSStatement body = node.Body[i];
                WriteNode(body);
            }
        }

        protected virtual void WriteNode(JSIdentifier node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));
            Write(node.Name);
        }

        protected virtual void WriteNode(JSLiteral node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            if (node.Value is string s) WriteLiteral(s);
            else if (node.Value is bool b) WriteLiteral(b);
            else if (node.Value is null) WriteLiteral();
            else if (node.Value is JSRegex r) WriteLiteral(r);
            else if (node.Value is byte uint8) WriteLiteral(uint8);
            else if (node.Value is sbyte int8) WriteLiteral(int8);
            else if (node.Value is ushort uint16) WriteLiteral(uint16);
            else if (node.Value is short int16) WriteLiteral(int16);
            else if (node.Value is uint uint32) WriteLiteral(uint32);
            else if (node.Value is int int32) WriteLiteral(int32);
            else if (node.Value is ulong uint64) WriteLiteral(uint64);
            else if (node.Value is long int64) WriteLiteral(int64);
            else if (node.Value is float float32) WriteLiteral(float32);
            else if (node.Value is double float64) WriteLiteral(float64);
            else if (node.Value is decimal decimal128) WriteLiteral(decimal128);
            else if (node.Value is IFormattable formattable) WriteLiteral(formattable);
            else WriteLiteral(node.Value);
        }

        protected virtual void WriteLiteral(object value) => Write(value);

        protected virtual void WriteLiteral(IFormattable value) => Write(value.ToString(null, CultureInfo.InvariantCulture));

        protected virtual void WriteLiteral(decimal value) => Write(value);

        protected virtual void WriteLiteral(double value) => Write(value);

        protected virtual void WriteLiteral(float value) => Write(value);

        protected virtual void WriteLiteral(long value) => Write(value);

        protected virtual void WriteLiteral(ulong value) => Write(value);

        protected virtual void WriteLiteral(int value) => Write(value);

        protected virtual void WriteLiteral(uint value) => Write(value);

        protected virtual void WriteLiteral(short value) => Write(value);

        protected virtual void WriteLiteral(ushort value) => Write(value);

        protected virtual void WriteLiteral(sbyte value) => Write(value);

        protected virtual void WriteLiteral(byte value) => Write(value);

        protected virtual void WriteLiteral(bool value) => Write(value ? "true" : "false");

        protected virtual void WriteLiteral() => Write("null");

        protected virtual void WriteLiteral(JSRegex value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value));

            Write("/");
            for (var i = 0; i < value.Pattern.Length; i++)
            {
                var c = value.Pattern[i];
                if (c == '/') Write("\\/");
                else Write(c);
            }
            Write("/");
            if (value.Options.HasFlag(JSRegexOptions.Global)) Write("g");
            if (value.Options.HasFlag(JSRegexOptions.IgnoreCase)) Write("i");
            if (value.Options.HasFlag(JSRegexOptions.Multiline)) Write("m");
            if (value.Options.HasFlag(JSRegexOptions.Unicode)) Write("u");
            if (value.Options.HasFlag(JSRegexOptions.Sticky)) Write("y");
        }

        protected virtual void WriteLiteral(string value)
        {
            if (value is null)
            {
                WriteLiteral();
                return;
            }

            Write("'");
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                switch (c)
                {
                    case '\\': Write("\\\\"); break;
                    case '\'': Write("\\'"); break;
                    case '\n': Write("\\n"); break;
                    case '\r': Write("\\r"); break;
                    case '\t': Write("\\t"); break;
                    case '\b': Write("\\b"); break;
                    case '\f': Write("\\f"); break;
                    case '\v': Write("\\v"); break;
                    case '\0': Write("\\0"); break;
                    default:
                        if (c < ' ' || c > '~' && c < 256)
                        {
                            Write("\\x");
                            Write(((int)c).ToString("X2", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            Write(c);
                        }
                        break;
                }
            }
            Write("'");
        }
    }
}
