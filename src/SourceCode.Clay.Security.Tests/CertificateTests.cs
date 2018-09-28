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
        private static readonly X509Certificate2 s_existingCertificate;

        static CertificateTests()
        {
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            try
            {
                // Choose any arbitrary certificate on the machine
                s_existingCertificate = certStore.Certificates[0];
            }
            finally
            {
                certStore.Close();
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_null()
        {
            const string thumbprint = null;

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.Equal(string.Empty, norm);

            Assert.Throws<ArgumentNullException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<ArgumentNullException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_empty()
        {
            const string thumbprint = "";

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.Equal(string.Empty, norm);

            Assert.Throws<ArgumentNullException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<ArgumentNullException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_nonexistent()
        {
            var thumbprint = "00000" + s_existingCertificate.Thumbprint.Substring(10) + "00000"; // Valid format but unlikely to exist

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.Equal(thumbprint.Length, norm.Length);
            Assert.Equal(CertificateLoader.Sha1Length, norm.Length);

            var found = CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _);
            Assert.False(found);

            Assert.Throws<InvalidOperationException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_only()
        {
            var thumbprint = new string('?', CertificateLoader.Sha1Length); // Special characters only

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.Equal(0, norm.Length);
            Assert.Throws<ArgumentNullException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, norm, false, out _));

            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<FormatException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(39)]
        public static void When_load_certificate_thumb_short_by_N(int n)
        {
            var thumbprint = s_existingCertificate.Thumbprint.Substring(n); // Too short

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.Equal(thumbprint.Length, norm.Length);
            Assert.NotEqual(CertificateLoader.Sha1Length, norm.Length);

            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<FormatException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public static void When_load_certificate_thumb_long_by_N(int n)
        {
            var thumbprint = s_existingCertificate.Thumbprint + new string('1', n); // Too long

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.NotEqual(thumbprint.Length, norm.Length);
            Assert.Equal(CertificateLoader.Sha1Length, norm.Length);

            var found = CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, norm, false, out _);
            Assert.True(found);

            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<FormatException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(39)]
        public static void When_load_certificate_thumb_noisy_short(int n)
        {
            var thumbprint = "\n" + s_existingCertificate.Thumbprint.Substring(n) + "\t"; // Too short after removing special chars

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.NotEqual(thumbprint.Length, norm.Length);
            Assert.NotEqual(CertificateLoader.Sha1Length, norm.Length);
            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, norm, false, out _));

            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<FormatException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_short_0()
        {
            const string thumbprint = "\r\n"; // 0 chars after removing special chars

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.NotEqual(thumbprint.Length, norm.Length);
            Assert.Equal(0, norm.Length);
            Assert.Throws<ArgumentNullException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, norm, false, out _));

            Assert.Throws<ArgumentNullException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<ArgumentNullException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public static void When_load_certificate_thumb_noisy_long(int n)
        {
            var thumbprint = "\n" + s_existingCertificate.Thumbprint + new string('1', n) + "\t"; // Too long after removing special chars

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.NotEqual(thumbprint.Length, norm.Length);
            Assert.Equal(CertificateLoader.Sha1Length, norm.Length);

            var found = CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, norm, false, out _);
            Assert.True(found);

            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<FormatException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_noisy_valid()
        {
            var thumbprint = "\n" + s_existingCertificate.Thumbprint + "\t"; // Valid after removing special chars

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.NotEqual(thumbprint.Length, norm.Length);
            Assert.Equal(CertificateLoader.Sha1Length, norm.Length);

            var found = CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, norm, false, out _);
            Assert.True(found);

            Assert.Throws<FormatException>(() => CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out _));
            Assert.Throws<FormatException>(() => CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_load_certificate_thumb_valid()
        {
            var thumbprint = s_existingCertificate.Thumbprint; // Valid in all respects (given that we already retrieved it locally)

            var norm = CertificateLoader.NormalizeThumbprint(thumbprint);
            Assert.Equal(CertificateLoader.Sha1Length, norm.Length);

            var found = CertificateLoader.TryLoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false, out X509Certificate2 actual);
            Assert.True(found);
            Assert.Equal(s_existingCertificate.SerialNumber, actual.SerialNumber);

            actual = CertificateLoader.LoadCertificate(StoreName.My, StoreLocation.CurrentUser, thumbprint, false);
            Assert.Equal(s_existingCertificate.SerialNumber, actual.SerialNumber);
        }
    }
}
