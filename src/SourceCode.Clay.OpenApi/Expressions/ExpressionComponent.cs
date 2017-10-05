using System;

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents an Open API expression component.
    /// </summary>
    public abstract class ExpressionComponent : IEquatable<ExpressionComponent>
    {
        /// <summary>
        /// Gets the component type.
        /// </summary>
        public abstract ExpressionComponentType ComponentType { get; }

        #region Ctor
#       pragma warning disable S3442 // "abstract" classes should not have "public" constructors
        // Reasoning: external inheritance not supported.

        /// <summary>
        /// Creates a new instance of the <see cref="ExpressionComponent"/> class.
        /// </summary>
        internal ExpressionComponent()
        {

        }
#       pragma warning restore S3442 // "abstract" classes should not have "public" constructors
        #endregion

        #region Equality

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
                hc = hc * 21 + ComponentType.GetHashCode();
                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        bool IEquatable<ExpressionComponent>.Equals(ExpressionComponent other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ComponentType != other.ComponentType) return false;
            return Equals(other);
        }

        public static bool operator ==(ExpressionComponent expression1, ExpressionComponent expression2)
        {
            if (ReferenceEquals(expression1, expression2)) return true;
            if (ReferenceEquals(expression1, null) || ReferenceEquals(expression2, null)) return false;

            return expression1.Equals(expression2);
        }

        public static bool operator !=(ExpressionComponent expression1, ExpressionComponent expression2) => !(expression1 == expression2); 

        #endregion
    }
}
