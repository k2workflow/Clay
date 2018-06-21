#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace SourceCode.Clay.Security.Tests
{
    public static class CertificateTests
    {
        private const string NullThumbprint = null;
        private const string EmptyThumbprint = "";

        private const string NonExistentThumbprint = "1230000000000000000000000000000000000000"; // Valid but does not exist
        private const string InvalidThumbprint = "________________________________________"; // Correct length but bad format

        private const string ShortThumbprint = "1230000000000000000000000000000000000"; // Too short
        private const string LongThumbprint = "1230000000000000000000000000000000000000123"; // Too long

        private const string NoisyOnlyThumbprint = "????????????????????????????????????????"; // Special characters only
        private const string NoisyValidThumbprint = "\n1230000000000000000000000000?000000000000\t"; // Valid, but has special chars
        private const string NoisyShortThumbprint = "\n1230000000000000000000000000???0000000000\t"; // Too short after removing special chars
        private const string NoisyLongThumbprint = "\n1230000000000000000000000000?00000000000012\t"; // Too long after removing special chars

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_null()
        {
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.TryLoadCertificate(NullThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.LoadCertificate(NullThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_empty()
        {
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.TryLoadCertificate(EmptyThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.LoadCertificate(EmptyThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_nonexistent()
        {
            var clean = CertificateExtensions.Clean(NonExistentThumbprint);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(NonExistentThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(NonExistentThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_invalid()
        {
            var clean = CertificateExtensions.Clean(InvalidThumbprint);
            Assert.Equal(InvalidThumbprint.Length, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(InvalidThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(InvalidThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_only()
        {
            var clean = CertificateExtensions.Clean(NoisyOnlyThumbprint);
            Assert.Equal(0, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(NoisyOnlyThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(NoisyOnlyThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_short()
        {
            var clean = CertificateExtensions.Clean(ShortThumbprint);
            Assert.Equal(ShortThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(ShortThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(ShortThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_long()
        {
            var clean = CertificateExtensions.Clean(LongThumbprint);
            Assert.Equal(LongThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(LongThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(LongThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_short()
        {
            var clean = CertificateExtensions.Clean(NoisyShortThumbprint);
            Assert.NotEqual(NoisyShortThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(NoisyShortThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(NoisyShortThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_long()
        {
            var clean = CertificateExtensions.Clean(NoisyLongThumbprint);
            Assert.NotEqual(NoisyLongThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(NoisyLongThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(NoisyLongThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_valid()
        {
            var clean = CertificateExtensions.Clean(NoisyValidThumbprint);
            Assert.NotEqual(NoisyValidThumbprint.Length, clean.Length);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(NoisyValidThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(NoisyValidThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_valid()
        {
            X509Certificate2 expected;

            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            try
            {
                // Choose any arbitrary certificate on the machine
                expected = certStore.Certificates[0];
            }
            finally
            {
                certStore.Close();
            }

            var thumbprint = expected.Thumbprint;
            var clean = CertificateExtensions.Clean(thumbprint);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(thumbprint, out var actual, StoreName.My, false);
            Assert.True(found);
            Assert.Equal(expected.SerialNumber, actual.SerialNumber);

            actual = StoreLocation.CurrentUser.LoadCertificate(thumbprint, StoreName.My, false);
            Assert.Equal(expected.SerialNumber, actual.SerialNumber);
        }
    }
}
