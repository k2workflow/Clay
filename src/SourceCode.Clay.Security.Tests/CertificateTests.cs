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
        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_null()
        {
            const string nullThumbprint = null;

            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.TryLoadCertificate(nullThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.LoadCertificate(nullThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_empty()
        {
            const string emptyThumbprint = "";

            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.TryLoadCertificate(emptyThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.LoadCertificate(emptyThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_nonexistent()
        {
            const string nonExistentThumbprint = "1230000000000000000000000000000000000000"; // Valid format but does not exist

            var clean = CertificateExtensions.Clean(nonExistentThumbprint);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(nonExistentThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(nonExistentThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_invalid()
        {
            const string invalidThumbprint = "________________________________________"; // Correct length but bad format

            var clean = CertificateExtensions.Clean(invalidThumbprint);
            Assert.Equal(invalidThumbprint.Length, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(invalidThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(invalidThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_only()
        {
            const string noisyOnlyThumbprint = "????????????????????????????????????????"; // Special characters only

            var clean = CertificateExtensions.Clean(noisyOnlyThumbprint);
            Assert.Equal(0, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(noisyOnlyThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(noisyOnlyThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_short()
        {
            const string shortThumbprint = "1230000000000000000000000000000000000"; // Too short

            var clean = CertificateExtensions.Clean(shortThumbprint);
            Assert.Equal(shortThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(shortThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(shortThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_long_by_1()
        {
            const string longThumbprint = "1230000000000000000000000000000000000000" + "1"; // Too long

            var clean = CertificateExtensions.Clean(longThumbprint);
            Assert.Equal(longThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(longThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(longThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_long_within_limit()
        {
            var longThumbprint = "1230000000000000000000000000000000000000" + new string('1', CertificateExtensions.JunkLen); // Too long

            var clean = CertificateExtensions.Clean(longThumbprint);
            Assert.Equal(longThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(longThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(longThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_long_over_limit()
        {
            var longThumbprint = "1230000000000000000000000000000000000000" + new string('1', CertificateExtensions.JunkLen + 1); // Too long

            var clean = CertificateExtensions.Clean(longThumbprint);
            Assert.Equal(longThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.TryLoadCertificate(longThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(longThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_short()
        {
            const string noisyShortThumbprint = "\n1230000000000000000000000000???0000000000\t"; // Too short after removing special chars

            var clean = CertificateExtensions.Clean(noisyShortThumbprint);
            Assert.NotEqual(noisyShortThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(noisyShortThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(noisyShortThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_short_0()
        {
            const string noisy0Thumbprint = "?"; // 0 chars after removing special chars

            var clean = CertificateExtensions.Clean(noisy0Thumbprint);
            Assert.NotEqual(noisy0Thumbprint.Length, clean.Length);
            Assert.Equal(0, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(noisy0Thumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(noisy0Thumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_short_1()
        {
            const string noisy1Thumbprint = "0?"; // Only 1 char after removing special chars

            var clean = CertificateExtensions.Clean(noisy1Thumbprint);
            Assert.NotEqual(noisy1Thumbprint.Length, clean.Length);
            Assert.Equal(1, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(noisy1Thumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(noisy1Thumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_long()
        {
            const string noisyLongThumbprint = "\n1230000000000000000000000000?00000000000012\t"; // Too long after removing special chars

            var clean = CertificateExtensions.Clean(noisyLongThumbprint);
            Assert.NotEqual(noisyLongThumbprint.Length, clean.Length);
            Assert.NotEqual(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(noisyLongThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(noisyLongThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_valid()
        {
            const string noisyValidThumbprint = "\n1230000000000000000000000000?000000000000\t"; // Valid format after removing special chars

            var clean = CertificateExtensions.Clean(noisyValidThumbprint);
            Assert.NotEqual(noisyValidThumbprint.Length, clean.Length);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(noisyValidThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(noisyValidThumbprint, StoreName.My, false));
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

            var thumbprint = expected.Thumbprint; // Valid in all respects (given that we already retrieved it locally)
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
