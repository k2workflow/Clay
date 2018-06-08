#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents an Open API expression component.
    /// </summary>
    public abstract class OasExpressionComponent : IEquatable<OasExpressionComponent>
    {
        /// <summary>
        /// Gets the component type.
        /// </summary>
        public abstract OasExpressionComponentType ComponentType { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasExpressionComponent"/> class.
        /// </summary>
        internal OasExpressionComponent()
        { }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="expression1">The expression1.</param>
        /// <param name="expression2">The expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasExpressionComponent expression1, OasExpressionComponent expression2)
        {
            if (expression1 is null) return expression2 is null;
            return expression1.Equals(expression2);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="expression1">The expression1.</param>
        /// <param name="expression2">The expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasExpressionComponent expression1, OasExpressionComponent expression2) => !(expression1 == expression2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public abstract override bool Equals(object obj);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            ComponentType
        );

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        bool IEquatable<OasExpressionComponent>.Equals(OasExpressionComponent other)
        {
            if (other is null) return false;
            if (ComponentType != other.ComponentType) return false;
            return Equals(other);
        }
    }
}
