#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Threading;

namespace SourceCode.Clay.Distributed
{
    /// <summary>
    /// Represents a <see cref="IDistributedIdFactory"/> that creates IDs containing
    /// 40 bits of time entropy, 10 bits of sequence entropy and 14 bits of machine entropy.
    /// </summary>
    public sealed class DistributedIdFactory : IDistributedIdFactory
    {
        // 40 bits time (10ms = 348.7 years)
        // 10 bits sequence
        // 14 bits machine

        private const byte TimestampFrequency = 1000 / 10;

        private const ulong MachineMask = 0b0011_1111_1111_1111;
        private const ulong SequenceMask = 0b0011_1111_1111;
        private const ulong TimestampMask = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111;

        private const byte MachineShift = 0;
        private const byte SequenceShift = 14;
        private const byte TimestampShift = 24;

        private static readonly ulong s_divisor = Stopwatch.IsHighResolution && (ulong)Stopwatch.Frequency > TimestampFrequency
            ? (ulong)Stopwatch.Frequency / TimestampFrequency
            : 0;

        /// <summary>
        /// Gets the epoch for the time entropy.
        /// </summary>
        public DateTime Epoch { get; }

        /// <summary>
        /// Gets the current machine identifier.
        /// </summary>
        public ushort MachineId { get; }

        private ulong Timestamp => ((ulong)Stopwatch.GetTimestamp() / s_divisor) - _offset;

        private readonly ulong _offset;
        private readonly object _lock;
        private ulong _lastTimestamp;
        private ushort _lastSequence;

        /// <summary>
        /// Creates a new instance of the <see cref="DistributedIdFactory"/> class.
        /// </summary>
        /// <param name="epoch">The epoch for the time entropy.</param>
        /// <param name="machineId">The identifier of the current machine. The value cannot be larger than 16383.</param>
        public DistributedIdFactory(DateTime epoch, ushort machineId)
        {
            if (s_divisor == 0)
                throw new PlatformNotSupportedException($"A system timer with at least {1000 / TimestampFrequency}ms frequency is required.");
            if ((machineId & ~MachineMask) != 0)
                throw new ArgumentOutOfRangeException(nameof(machineId), machineId, "The machine ID cannot be larger than 16383.");
            if (epoch.Kind != DateTimeKind.Utc)
                throw new ArgumentOutOfRangeException(nameof(epoch), epoch, "The epoch must be a UTC DateTime.");

            Epoch = epoch;

            var epochDifference = ((ulong)DateTime.UtcNow.Ticks - (ulong)epoch.Ticks) / s_divisor;
            var stopwatchDifference = (ulong)Stopwatch.GetTimestamp() / s_divisor;
            _offset = stopwatchDifference - epochDifference;

            if ((_offset & ~TimestampMask) != 0)
                throw new ArgumentOutOfRangeException(nameof(epoch), epoch, "The epoch is in the future.");

            MachineId = machineId;
            _lastTimestamp = Timestamp;
            _lastSequence = 0;
            _lock = new object();
        }

        /// <summary>
        /// Testing constructor.
        /// </summary>
        /// <param name="offset">The value for the offset field.</param>
        /// <param name="machineId">The machine ID.</param>
        internal DistributedIdFactory(ulong offset, ushort machineId)
        {
            MachineId = machineId;
            _offset = offset;
            _lastTimestamp = Timestamp;
            _lastSequence = 0;
            _lock = new object();
        }

        /// <inheritdoc />
        public DistributedId Create()
        {
            ulong timestamp;
            ulong sequence;

            while (true)
            {
                lock (_lock)
                {
                    timestamp = Timestamp;
                    if (timestamp != _lastTimestamp)
                    {
                        _lastTimestamp = timestamp;
                        _lastSequence = 0;
                    }
                    sequence = ++_lastSequence;
                }

                if ((timestamp & ~TimestampMask) != 0)
                    throw new InvalidOperationException("All distributed identifiers have been exhausted for the epoch.");

                // Wait for the next slice.
                if ((sequence & ~SequenceMask) == 0) break;
                else Thread.Yield();
            }

            var id = ((ulong)MachineId << MachineShift) |
                (sequence << SequenceShift) |
                (timestamp << TimestampShift);

            return new DistributedId(id);
        }
    }
}
