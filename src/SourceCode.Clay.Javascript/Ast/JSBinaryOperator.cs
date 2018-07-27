namespace SourceCode.Clay.Javascript.Ast
{
    public enum JSBinaryOperator
    {
        IdentityEquality,
        IdentityInequality,
        CoercedEquality,
        CoercedInequality,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        UnsignedLeftShift,
        SignedRightShift,
        UnsignedRightShift,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulus,
        BitwiseOr,
        BitwiseXor,
        BitwiseAnd,
        In,
        InstanceOf,

        Assign,
        AddAssign,
        SubtractAssign,
        MultiplyAssign,
        DivideAssign,
        ModulusAssign,
        UnsignedLeftShiftAssign,
        SignedRightShiftAssign,
        UnsignedRightShiftAssign,
        BitwiseOrAssign,
        BitwiseXorAssign,
        BitwiseAndAssign,

        LogicalAnd,
        LogicalOr
    }
}