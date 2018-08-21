using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a discriminated union of two types.
    /// </summary>
    /// <typeparam name="TItem1">The first type.</typeparam>
    /// <typeparam name="TItem2">The second type.</typeparam>
    [DebuggerDisplay("{Which,nq} {Value}")]
    public readonly struct Discriminated<TItem1, TItem2> : IEquatable<Discriminated<TItem1, TItem2>>
    {
        private readonly byte _state;
        private readonly TItem1 _item1;
        private readonly TItem2 _item2;

        /// <summary>
        /// Determines whether the union has a value.
        /// </summary>
        public bool HasValue => _state != 0;

        /// <summary>
        /// Determines whether the first item is set.
        /// </summary>
        public bool IsItem1 => _state == 1;

        /// <summary>
        /// Determines whether the second item is set.
        /// </summary>
        public bool IsItem2 => _state == 2;

        /// <summary>
        /// Gets the first item.
        /// </summary>
        public TItem1 Item1 => _state == 1 ? _item1 : throw new InvalidOperationException("The first item is not set.");

        /// <summary>
        /// Gets the second item.
        /// </summary>
        public TItem2 Item2 => _state == 2 ? _item2 : throw new InvalidOperationException("The second item is not set.");

        #region DebuggerDisplay

        [ExcludeFromCodeCoverage]
        private string Which
        {
            get
            {
                switch (_state)
                {
                    case 0: return "Empty";
                    case 1: return nameof(Item1);
                    case 2: return nameof(Item2);
                }

                return "Invalid";
            }
        }

        [ExcludeFromCodeCoverage]
        private object Value
        {
            get
            {
                switch (_state)
                {
                    case 1: return _item1;
                    case 2: return _item2;
                }

                return null;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="Discriminated{TItem1, TItem2}"/> with a <typeparamref name="TItem1"/>.
        /// </summary>
        /// <param name="item1">The <typeparamref name="TItem1"/>.</param>
        public Discriminated(TItem1 item1)
        {
            _item1 = item1;
            _item2 = default;
            _state = 1;
        }

        /// <summary>
        /// Creates a new <see cref="Discriminated{TItem1, TItem2}"/> with a <typeparamref name="TItem2"/>.
        /// </summary>
        /// <param name="item2">The <typeparamref name="TItem2"/>.</param>
        public Discriminated(TItem2 item2)
        {
            _item1 = default;
            _item2 = item2;
            _state = 2;
        }

        /// <summary>
        /// Executes a delegate when one of the values is selected.
        /// </summary>
        /// <param name="empty">The delegate to execute if no value is selected.</param>
        /// <param name="item1">The delegate to execute if the first value is selected.</param>
        /// <param name="item2">The delegate to execute if the second value is selected.</param>
        public void Select(Action empty = null, Action<TItem1> item1 = null, Action<TItem2> item2 = null)
        {
            switch (_state)
            {
                case 0:
                    empty?.Invoke();
                    break;
                case 1:
                    item1?.Invoke(_item1);
                    break;
                case 2:
                    item2?.Invoke(_item2);
                    break;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Executes a delegate when one of the values is selected.
        /// </summary>
        /// <param name="empty">The delegate to execute if no value is selected.</param>
        /// <param name="item1">The delegate to execute if the first value is selected.</param>
        /// <param name="item2">The delegate to execute if the second value is selected.</param>
        public T Select<T>(Func<T> empty = null, Func<TItem1, T> item1 = null, Func<TItem2, T> item2 = null)
        {
            switch (_state)
            {
                case 0:
                    return empty is null ? default : empty();
                case 1:
                    return item1 is null ? default : item1(_item1);
                case 2:
                    return item2 is null ? default : item2(_item2);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Determines if this <see cref="Discriminated{TItem1, TItem2}"/> is equal to another.
        /// </summary>
        /// <param name="obj">The <see cref="Discriminated{TItem1, TItem2}"/>.</param>
        /// <returns>A value indicating whether the <see cref="Discriminated{TItem1, TItem2}"/> are equal.</returns>
        public override bool Equals(object obj) 
            => obj is Discriminated<TItem1, TItem2> other 
            && Equals(other);

        /// <summary>
        /// Determines if this <see cref="Discriminated{TItem1, TItem2}"/> is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="Discriminated{TItem1, TItem2}"/>.</param>
        /// <returns>A value indicating whether the <see cref="Discriminated{TItem1, TItem2}"/> are equal.</returns>
        public bool Equals(Discriminated<TItem1, TItem2> other)
            => _state == other._state &&
            (
                (_state == 1 && EqualityComparer<TItem1>.Default.Equals(_item1, other._item1)) ||
                (_state == 2 && EqualityComparer<TItem2>.Default.Equals(_item2, other._item2))
            );

        /// <summary>
        /// Gets the hash code for this <see cref="Discriminated{TItem1, TItem2}"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            switch (_state)
            {
                case 1: return EqualityComparer<TItem1>.Default.GetHashCode(_item1);
                case 2: return EqualityComparer<TItem2>.Default.GetHashCode(_item2);
            }

            return 0;
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Implicitly converts a <typeparamref name="TItem1"/> to a <see cref="Discriminated{TItem1, TItem2}"/>.
        /// </summary>
        /// <param name="item1">The <typeparamref name="TItem1"/>.</param>
        public static implicit operator Discriminated<TItem1, TItem2>(TItem1 item1)
            => new Discriminated<TItem1, TItem2>(item1);

        /// <summary>
        /// Implicitly converts a <typeparamref name="TItem2"/> to a <see cref="Discriminated{TItem1, TItem2}"/>.
        /// </summary>
        /// <param name="item2">The <typeparamref name="TItem2"/>.</param>
        public static implicit operator Discriminated<TItem1, TItem2>(TItem2 item2)
            => new Discriminated<TItem1, TItem2>(item2);
#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Determines whether two <see cref="Discriminated{TItem1, TItem2}"/> are equal.
        /// </summary>
        /// <param name="discriminated1">The first <see cref="Discriminated{TItem1, TItem2}"/>.</param>
        /// <param name="discriminated2">The second <see cref="Discriminated{TItem1, TItem2}"/>.</param>
        /// <returns>A value indicating whether the <see cref="Discriminated{TItem1, TItem2}"/> are equal.</returns>
        public static bool operator ==(Discriminated<TItem1, TItem2> discriminated1, Discriminated<TItem1, TItem2> discriminated2) 
            => discriminated1.Equals(discriminated2);

        /// <summary>
        /// Determines whether two <see cref="Discriminated{TItem1, TItem2}"/> are unequal.
        /// </summary>
        /// <param name="discriminated1">The first <see cref="Discriminated{TItem1, TItem2}"/>.</param>
        /// <param name="discriminated2">The second <see cref="Discriminated{TItem1, TItem2}"/>.</param>
        /// <returns>A value indicating whether the <see cref="Discriminated{TItem1, TItem2}"/> are unequal.</returns>
        public static bool operator !=(Discriminated<TItem1, TItem2> discriminated1, Discriminated<TItem1, TItem2> discriminated2) 
            => !(discriminated1 == discriminated2);
    }
}
