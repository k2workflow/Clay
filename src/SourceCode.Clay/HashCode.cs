#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs

    /// <summary>
    /// Represents a way to create high quality hashcodes.
    /// </summary>
    [DebuggerDisplay("{_value,nq}")]
    public struct HashCode
    {
        // Closely follows https://github.com/dotnet/corefx/issues/14354
        // TODO: Remove when that feature lands.

        #region Fields

        private ulong _value;
        private byte _index;

        #endregion

        #region Add

        /// <summary>
        /// Adds the specified hashcode into this <see cref="HashCode"/>.
        /// </summary>
        /// <param name="value">The hashcode to add.</param>
        /// <returns>The <see cref="HashCode"/> value with the additional hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public void Add(int value)
        {
            unchecked
            {
                _value = (_value * Prime(_index)) + (ulong)value;
                _index = (byte)(_index + 1);
            }
        }

        /// <summary>
        /// Adds the hashcode of the specified value into this <see cref="HashCode"/>.
        /// </summary>
        /// <param name="value">The value to hash and add.</param>
        /// <returns>The <see cref="HashCode"/> value with the additional hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public void Add<T>(T value) => Add(value, EqualityComparer<T>.Default);

        /// <summary>
        /// Adds the hashcode of the specified value into this <see cref="HashCode"/>.
        /// </summary>
        /// <param name="value">The value to hash and add.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>The <see cref="HashCode"/> value with the additional hash code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public void Add<T>(T value, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            Add(comparer.GetHashCode(value));
        }

        #endregion

        #region Static Methods

#       pragma warning disable S2436 // Classes and methods should not have too many generic parameters

        /// <summary>
        /// Creates a hash code from the specified object.
        /// </summary>
        /// <typeparam name="T1">The type of the object.</typeparam>
        /// <param name="value1">The object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1>(T1 value1)
        {
            var hc = new HashCode();
            hc.Add(value1);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <typeparam name="T6">The type of the sixth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <param name="value6">The sixth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            hc.Add(value6);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <typeparam name="T6">The type of the fifth object.</typeparam>
        /// <typeparam name="T7">The type of the seventh object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <param name="value6">The sixth object.</param>
        /// <param name="value7">The seventh object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            hc.Add(value6);
            hc.Add(value7);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <typeparam name="T6">The type of the fifth object.</typeparam>
        /// <typeparam name="T7">The type of the seventh object.</typeparam>
        /// <typeparam name="T8">The type of the eighth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <param name="value6">The sixth object.</param>
        /// <param name="value7">The seventh object.</param>
        /// <param name="value8">The eighth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            hc.Add(value6);
            hc.Add(value7);
            hc.Add(value8);
            return hc.ToHashCode();
        }

#       pragma warning restore S2436 // Classes and methods should not have too many generic parameters

        #endregion

        #region Operators

        /// <summary>
        /// Converts this <see cref="HashCode"/> to a <see cref="int"/> hashcode.
        /// </summary>
        /// <returns>The <see cref="int"/> hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public int ToHashCode()
        {
            unchecked
            {
                // Decrease bit count

                var h = ((uint)(_value >> 32)) ^ (uint)_value;

                // Murmur3x32 final mix

                h ^= h >> 16;
                h *= 0x85ebca6b;
                h ^= h >> 13;
                h *= 0xc2b2ae35;
                h ^= h >> 16;

                return (int)h;
            }
        }

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        /// <summary>
        /// Converts this <see cref="HashCode"/> to a <see cref="int"/> hashcode.
        /// </summary>
        /// <returns>The <see cref="int"/> hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        [Obsolete("Use ToHashCode to retrieve the computed hash code.", error: true)]
        public override int GetHashCode() => ToHashCode();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        #endregion

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong Prime(byte index)
        {
            // Hope that the jitter is smart enough to inline this.
            // If it doesn't, it will still be an optimal jump table with
            // this many values.

            switch (index)
            {
                case 0: return 5653UL;
                case 1: return 5657UL;
                case 2: return 5659UL;
                case 3: return 5669UL;
                case 4: return 5683UL;
                case 5: return 5689UL;
                case 6: return 5693UL;
                case 7: return 5701UL;
                case 8: return 5711UL;
                case 9: return 5717UL;
                case 10: return 5737UL;
                case 11: return 5741UL;
                case 12: return 5743UL;
                case 13: return 5749UL;
                case 14: return 5779UL;
                case 15: return 5783UL;
                case 16: return 5791UL;
                case 17: return 5801UL;
                case 18: return 5807UL;
                case 19: return 5813UL;
                case 20: return 5821UL;
                case 21: return 5827UL;
                case 22: return 5839UL;
                case 23: return 5843UL;
                case 24: return 5849UL;
                case 25: return 5851UL;
                case 26: return 5857UL;
                case 27: return 5861UL;
                case 28: return 5867UL;
                case 29: return 5869UL;
                case 30: return 5879UL;
                case 31: return 5881UL;
                case 32: return 5897UL;
                case 33: return 5903UL;
                case 34: return 5923UL;
                case 35: return 5927UL;
                case 36: return 5939UL;
                case 37: return 5953UL;
                case 38: return 5981UL;
                case 39: return 5987UL;
                case 40: return 6007UL;
                case 41: return 6011UL;
                case 42: return 6029UL;
                case 43: return 6037UL;
                case 44: return 6043UL;
                case 45: return 6047UL;
                case 46: return 6053UL;
                case 47: return 6067UL;
                case 48: return 6073UL;
                case 49: return 6079UL;
                case 50: return 6089UL;
                case 51: return 6091UL;
                case 52: return 6101UL;
                case 53: return 6113UL;
                case 54: return 6121UL;
                case 55: return 6131UL;
                case 56: return 6133UL;
                case 57: return 6143UL;
                case 58: return 6151UL;
                case 59: return 6163UL;
                case 60: return 6173UL;
                case 61: return 6197UL;
                case 62: return 6199UL;
                case 63: return 6203UL;
                case 64: return 6211UL;
                case 65: return 6217UL;
                case 66: return 6221UL;
                case 67: return 6229UL;
                case 68: return 6247UL;
                case 69: return 6257UL;
                case 70: return 6263UL;
                case 71: return 6269UL;
                case 72: return 6271UL;
                case 73: return 6277UL;
                case 74: return 6287UL;
                case 75: return 6299UL;
                case 76: return 6301UL;
                case 77: return 6311UL;
                case 78: return 6317UL;
                case 79: return 6323UL;
                case 80: return 6329UL;
                case 81: return 6337UL;
                case 82: return 6343UL;
                case 83: return 6353UL;
                case 84: return 6359UL;
                case 85: return 6361UL;
                case 86: return 6367UL;
                case 87: return 6373UL;
                case 88: return 6379UL;
                case 89: return 6389UL;
                case 90: return 6397UL;
                case 91: return 6421UL;
                case 92: return 6427UL;
                case 93: return 6449UL;
                case 94: return 6451UL;
                case 95: return 6469UL;
                case 96: return 6473UL;
                case 97: return 6481UL;
                case 98: return 6491UL;
                case 99: return 6521UL;
                case 100: return 6529UL;
                case 101: return 6547UL;
                case 102: return 6551UL;
                case 103: return 6553UL;
                case 104: return 6563UL;
                case 105: return 6569UL;
                case 106: return 6571UL;
                case 107: return 6577UL;
                case 108: return 6581UL;
                case 109: return 6599UL;
                case 110: return 6607UL;
                case 111: return 6619UL;
                case 112: return 6637UL;
                case 113: return 6653UL;
                case 114: return 6659UL;
                case 115: return 6661UL;
                case 116: return 6673UL;
                case 117: return 6679UL;
                case 118: return 6689UL;
                case 119: return 6691UL;
                case 120: return 6701UL;
                case 121: return 6703UL;
                case 122: return 6709UL;
                case 123: return 6719UL;
                case 124: return 6733UL;
                case 125: return 6737UL;
                case 126: return 6761UL;
                case 127: return 6763UL;
                case 128: return 6779UL;
                case 129: return 6781UL;
                case 130: return 6791UL;
                case 131: return 6793UL;
                case 132: return 6803UL;
                case 133: return 6823UL;
                case 134: return 6827UL;
                case 135: return 6829UL;
                case 136: return 6833UL;
                case 137: return 6841UL;
                case 138: return 6857UL;
                case 139: return 6863UL;
                case 140: return 6869UL;
                case 141: return 6871UL;
                case 142: return 6883UL;
                case 143: return 6899UL;
                case 144: return 6907UL;
                case 145: return 6911UL;
                case 146: return 6917UL;
                case 147: return 6947UL;
                case 148: return 6949UL;
                case 149: return 6959UL;
                case 150: return 6961UL;
                case 151: return 6967UL;
                case 152: return 6971UL;
                case 153: return 6977UL;
                case 154: return 6983UL;
                case 155: return 6991UL;
                case 156: return 6997UL;
                case 157: return 7001UL;
                case 158: return 7013UL;
                case 159: return 7019UL;
                case 160: return 7027UL;
                case 161: return 7039UL;
                case 162: return 7043UL;
                case 163: return 7057UL;
                case 164: return 7069UL;
                case 165: return 7079UL;
                case 166: return 7103UL;
                case 167: return 7109UL;
                case 168: return 7121UL;
                case 169: return 7127UL;
                case 170: return 7129UL;
                case 171: return 7151UL;
                case 172: return 7159UL;
                case 173: return 7177UL;
                case 174: return 7187UL;
                case 175: return 7193UL;
                case 176: return 7207UL;
                case 177: return 7211UL;
                case 178: return 7213UL;
                case 179: return 7219UL;
                case 180: return 7229UL;
                case 181: return 7237UL;
                case 182: return 7243UL;
                case 183: return 7247UL;
                case 184: return 7253UL;
                case 185: return 7283UL;
                case 186: return 7297UL;
                case 187: return 7307UL;
                case 188: return 7309UL;
                case 189: return 7321UL;
                case 190: return 7331UL;
                case 191: return 7333UL;
                case 192: return 7349UL;
                case 193: return 7351UL;
                case 194: return 7369UL;
                case 195: return 7393UL;
                case 196: return 7411UL;
                case 197: return 7417UL;
                case 198: return 7433UL;
                case 199: return 7451UL;
                case 200: return 7457UL;
                case 201: return 7459UL;
                case 202: return 7477UL;
                case 203: return 7481UL;
                case 204: return 7487UL;
                case 205: return 7489UL;
                case 206: return 7499UL;
                case 207: return 7507UL;
                case 208: return 7517UL;
                case 209: return 7523UL;
                case 210: return 7529UL;
                case 211: return 7537UL;
                case 212: return 7541UL;
                case 213: return 7547UL;
                case 214: return 7549UL;
                case 215: return 7559UL;
                case 216: return 7561UL;
                case 217: return 7573UL;
                case 218: return 7577UL;
                case 219: return 7583UL;
                case 220: return 7589UL;
                case 221: return 7591UL;
                case 222: return 7603UL;
                case 223: return 7607UL;
                case 224: return 7621UL;
                case 225: return 7639UL;
                case 226: return 7643UL;
                case 227: return 7649UL;
                case 228: return 7669UL;
                case 229: return 7673UL;
                case 230: return 7681UL;
                case 231: return 7687UL;
                case 232: return 7691UL;
                case 233: return 7699UL;
                case 234: return 7703UL;
                case 235: return 7717UL;
                case 236: return 7723UL;
                case 237: return 7727UL;
                case 238: return 7741UL;
                case 239: return 7753UL;
                case 240: return 7757UL;
                case 241: return 7759UL;
                case 242: return 7789UL;
                case 243: return 7793UL;
                case 244: return 7817UL;
                case 245: return 7823UL;
                case 246: return 7829UL;
                case 247: return 7841UL;
                case 248: return 7853UL;
                case 249: return 7867UL;
                case 250: return 7873UL;
                case 251: return 7877UL;
                case 252: return 7879UL;
                case 253: return 7883UL;
                case 254: return 7901UL;
                case 255: return 7907UL;
                default: return 7919UL;
            }
        }

        #endregion
    }

#pragma warning restore S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
