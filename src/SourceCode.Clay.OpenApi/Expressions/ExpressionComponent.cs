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
    public abstract class ExpressionComponent : IEquatable<ExpressionComponent>
    {
        #region Properties

        /// <summary>
        /// Gets the component type.
        /// </summary>
        public abstract ExpressionComponentType ComponentType { get; }

        #endregion

        #region Ctor

#pragma warning disable S3442 // "abstract" classes should not have "public" constructors
        // Reasoning: external inheritance not supported.

        /// <summary>
        /// Creates a new instance of the <see cref="ExpressionComponent"/> class.
        /// </summary>
        internal ExpressionComponent()
        {
        }

#pragma warning restore S3442 // "abstract" classes should not have "public" constructors

        #endregion

        #region Equality

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="expression1">The expression1.</param>
        /// <param name="expression2">The expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ExpressionComponent expression1, ExpressionComponent expression2)
        {
            if (ReferenceEquals(expression1, expression2)) return true;
            if (expression1 is null || expression2 is null) return false;

            return expression1.Equals(expression2);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="expression1">The expression1.</param>
        /// <param name="expression2">The expression2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ExpressionComponent expression1, ExpressionComponent expression2) => !(expression1 == expression2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public abstract override bool Equals(object obj);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = (hc * 23) + ComponentType.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        bool IEquatable<ExpressionComponent>.Equals(ExpressionComponent other)
        {
            if (other is null) return false;
            if (ComponentType != other.ComponentType) return false;
            return Equals(other);
        }

        #endregion
    }
}
