#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Buffers;

namespace SourceCode.Clay.Buffers
{
    // See [ArrayPool.cs](https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/System/Buffers/ArrayPool.cs)
    // See [MemoryPool.cs](https://github.com/dotnet/corefx/blob/c5cad0cdbee20f2c5f392f3e4b19c71a274b2f2e/src/System.Memory/src/System/Buffers/MemoryPool.cs)
    // See [Create usage guidelines for lifetime management of System.Memory<T>](https://github.com/dotnet/docs/issues/4823)
    // See [Memory<T> usage guidelines](https://gist.github.com/GrabYourPitchforks/4c3e1935fd4d9fa2831dbfcab35dffc6)
    // See [Memory<T> API documentation and samples](https://gist.github.com/GrabYourPitchforks/8efb15abbd90bc5b128f64981766e834)
    // See [Question: IMemoryOwner decorator & lifetime management](https://github.com/dotnet/corefx/issues/33372)

    public static class IMemoryOwnerExtensions
    {
        /// <summary>
        /// Wrap an existing <see cref="IMemoryOwner{T}"/> instance in a lightweight manner, but allow
        /// the <see cref="IMemoryOwner{T}.Memory"/> member to have a different length.
        /// </summary>
        /// <param name="owner">The original instance.</param>
        /// <param name="start">The starting offset of the slice.</param>
        /// <param name="length">The length of the slice.</param>
        public static IMemoryOwner<T> WrapSlice<T>(this IMemoryOwner<T> owner, int start, int length)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            // Fast path for no-op
            if (start == 0 && length == owner.Memory.Length)
                return owner;

            if ((uint)start >= (uint)owner.Memory.Length) throw new ArgumentOutOfRangeException(nameof(start));
            if ((uint)length > (uint)(owner.Memory.Length - start)) throw new ArgumentOutOfRangeException(nameof(length));

            return new SliceOwner<T>(owner, start, length);
        }

        /// <summary>
        /// Wrap an existing <see cref="IMemoryOwner{T}"/> instance in a lightweight manner, but allow
        /// the <see cref="IMemoryOwner{T}.Memory"/> member to have a different length.
        /// </summary>
        /// <param name="owner">The original instance.</param>
        /// <param name="start">The starting offset of the slice.</param>
        public static IMemoryOwner<T> WrapSlice<T>(this IMemoryOwner<T> owner, int start)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            // Fast path for no-op
            if (start == 0)
                return owner;

            if ((uint)start >= (uint)owner.Memory.Length) throw new ArgumentOutOfRangeException(nameof(start));

            return new SliceOwner<T>(owner, start);
        }

        private struct SliceOwner<T> : IMemoryOwner<T>
        {
            private IMemoryOwner<T> _owner;
            public Memory<T> Memory { get; private set; }

            public SliceOwner(IMemoryOwner<T> owner, int start, int length)
            {
                _owner = owner;
                Memory = _owner.Memory.Slice(start, length);
            }

            public SliceOwner(IMemoryOwner<T> owner, int start)
            {
                _owner = owner;
                Memory = _owner.Memory.Slice(start);
            }

            #region IDisposable

            public void Dispose()
            {
                if (_owner != null)
                {
                    _owner.Dispose();
                    _owner = null;
                }

                Memory = default;
            }

            #endregion
        }
    }
}
