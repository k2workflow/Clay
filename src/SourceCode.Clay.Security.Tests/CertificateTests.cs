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
        private const string NonExistentThumbprint = "1230000000000000000000000000000000000000";
        private const string InvalidThumbprint = "________________________________________";
        private const string TrashThumbprint = "????????????????????????????????????????";
        private const string ShortThumbprint = "1230000000000000000000000000000000000";
        private const string LongThumbprint = "1230000000000000000000000000000000000000123";
        private const string SpecialThumbprint = "\n1230000000000000000000000000?000000000000\t";

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.TryLoadCertificate(NullThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.LoadCertificate(NullThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_is_empty()
        {
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.TryLoadCertificate(EmptyThumbprint, out var cert, StoreName.My, false));
            Assert.Throws<ArgumentNullException>(() => StoreLocation.CurrentUser.LoadCertificate(EmptyThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_does_non_existent()
        {
            var clean = CertificateExtensions.Clean(NonExistentThumbprint);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(NonExistentThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(NonExistentThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_invalid()
        {
            var clean = CertificateExtensions.Clean(InvalidThumbprint);
            Assert.Equal(InvalidThumbprint.Length, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(InvalidThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(InvalidThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_trash()
        {
            var clean = CertificateExtensions.Clean(TrashThumbprint);
            Assert.Equal(0, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(TrashThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(InvalidThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_short()
        {
            var clean = CertificateExtensions.Clean(ShortThumbprint);
            Assert.Equal(ShortThumbprint.Length, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(ShortThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(ShortThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_long()
        {
            var clean = CertificateExtensions.Clean(LongThumbprint);
            Assert.Equal(LongThumbprint.Length, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(LongThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(LongThumbprint, StoreName.My, false));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_certificate_thumb_special()
        {
            var clean = CertificateExtensions.Clean(SpecialThumbprint);
            Assert.Equal(CertificateExtensions.Sha1HexLen, clean.Length);

            var found = StoreLocation.CurrentUser.TryLoadCertificate(SpecialThumbprint, out var cert, StoreName.My, false);
            Assert.False(found);

            Assert.Throws<ArgumentException>(() => StoreLocation.CurrentUser.LoadCertificate(SpecialThumbprint, StoreName.My, false));
        }
    }
}
