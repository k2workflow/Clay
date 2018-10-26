#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Buffers;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// A <see cref="MemoryPool{T}"/> specialized for use in an arena context.
    /// It should be used in short-lived scoped scenarios and disposed of
    /// as soon as possible.
    /// </summary>
    public sealed class ArenaMemoryPool<T> : MemoryPool<T>
    {
        private readonly IList<IMemoryOwner<T>> _rentals;

        public override int MaxBufferSize => Shared.MaxBufferSize;

        public int Count => _rentals.Count;

        public ArenaMemoryPool()
        {
            _rentals = new List<IMemoryOwner<T>>();
        }

        public override IMemoryOwner<T> Rent(int minimumBufferSize = -1)
        {
            IMemoryOwner<T> rented = Shared.Rent(minimumBufferSize);

            _rentals.Add(rented);

            return rented;
        }

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    IList<IMemoryOwner<T>> rentals = _rentals;

                    if (rentals != null)
                        for (int i = 0; i < rentals.Count; i++)
                            rentals[i].Dispose();
                }

                _rentals.Clear();
                _disposed = true;
            }
        }
    }
}
