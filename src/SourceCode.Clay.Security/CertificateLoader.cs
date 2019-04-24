#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SourceCode.Clay.Security
{
    /// <summary>
    /// Represents <see cref="X509Certificate2"/> utilities and extensions.
    /// </summary>
    public static class CertificateLoader
    {
        /// <summary>
        /// The number of hex characters required to represent a Sha1 thumbprint.
        /// </summary>
        public const byte Sha1Length = 20 * 2;

        /// <summary>
        /// Load a <see cref="X509Certificate2"/> given its store location and <paramref name="thumbprint"/>.
        /// </summary>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="thumbprint">The certificate thumbprint to find.</param>
        /// <param name="validOnly">If set to <c>false</c>, allows an invalid certificate to be loaded.</param>
        /// <returns>If found, the certificate with the specified details.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="thumbprint"/>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="storeName"/>
        /// or
        /// <paramref name="storeLocation"/>
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="thumbprint"/>
        /// </exception>
        /// <exception cref="InvalidOperationException">Certificate not found: {thumbprint} in {storeLocation}/{storeName}</exception>
        public static X509Certificate2 LoadCertificate(StoreName storeName, StoreLocation storeLocation, string thumbprint, bool validOnly)
        {
            if (TryLoadCertificate(storeName, storeLocation, thumbprint, validOnly, out X509Certificate2 certificate))
                return certificate;

            throw new InvalidOperationException($"Certificate not found: {thumbprint} in {storeLocation}/{storeName}");
        }

        /// <summary>
        /// Try load a <see cref="X509Certificate2"/> given its store location and thumbprint.
        /// </summary>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="thumbprint">The certificate thumbprint to find.</param>
        /// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false.</param>
        /// <param name="certificate">If found, the certificate with the specified details.</param>
        /// <returns>Returns true if the specified certificate was found; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="storeName"/>
        /// or
        /// <paramref name="storeLocation"/>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="thumbprint"/>
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="thumbprint"/>
        /// </exception>
        public static bool TryLoadCertificate(StoreName storeName, StoreLocation storeLocation, string thumbprint, bool validOnly, out X509Certificate2 certificate)
        {
            if (string.IsNullOrWhiteSpace(thumbprint)) throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint.Length != Sha1Length) throw new FormatException($"Specified thumbprint should be {Sha1Length} characters long.");
            if (!Enum.IsDefined(typeof(StoreName), storeName)) throw new ArgumentOutOfRangeException(nameof(storeName));
            if (!Enum.IsDefined(typeof(StoreLocation), storeLocation)) throw new ArgumentOutOfRangeException(nameof(storeLocation));
            if (!IsHex(thumbprint[0]) || !IsHex(thumbprint[thumbprint.Length - 1])) throw new FormatException($"Invalid character(s) detected in thumbprint.");

            using (var store = new X509Store(storeName, storeLocation))
            {
                X509Certificate2Collection storeCertificates = null;
                certificate = null;

                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    storeCertificates = store.Certificates;

                    X509Certificate2Collection found = storeCertificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly);

                    if (found.Count == 0)
                    {
                        return false;
                    }

                    certificate = found
                        .OfType<X509Certificate2>()
                        .OrderByDescending(cert => cert.NotAfter)
                        .FirstOrDefault();

                    return certificate != null;
                }
                finally
                {
                    DisposeCertificates(storeCertificates, certificate);
                }
            }
        }

        /// <summary>
        /// Load a <see cref="X509Certificate2"/> given its store location and <paramref name="subject"/>/<paramref name="issuer"/>.
        /// </summary>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="validOnly">If set to <c>false</c>, allows an invalid certificate to be loaded.</param>
        /// <returns>If found, the certificate with the specified details.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subject"/>
        /// or
        /// <paramref name="issuer"/>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="storeName"/>
        /// or
        /// <paramref name="storeLocation"/>
        /// </exception>
        /// <exception cref="InvalidOperationException">Certificate not found: {subject} ({issuer}) in {storeLocation}/{storeName}</exception>
        public static X509Certificate2 LoadCertificate(StoreName storeName, StoreLocation storeLocation, string subject, string issuer, bool validOnly)
        {
            if (TryLoadCertificate(storeName, storeLocation, subject, issuer, validOnly, out X509Certificate2 certificate))
                return certificate;

            throw new InvalidOperationException($"Certificate not found: {subject} ({issuer}) in {storeLocation}/{storeName}");
        }

        /// <summary>
        /// Try load a <see cref="X509Certificate2"/> given its store location and <paramref name="subject"/>/<paramref name="issuer"/>.
        /// </summary>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="issuer">The issuer.</param>
        /// <param name="validOnly">If set to <c>false</c>, allows an invalid certificate to be loaded.</param>
        /// <param name="certificate">If found, the certificate with the specified details.</param>
        /// <returns>Returns true if the specified certificate was found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="subject"/>
        /// or
        /// <paramref name="issuer"/>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="storeName"/>
        /// or
        /// <paramref name="storeLocation"/>
        /// </exception>
        public static bool TryLoadCertificate(StoreName storeName, StoreLocation storeLocation, string subject, string issuer, bool validOnly, out X509Certificate2 certificate)
        {
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrWhiteSpace(issuer)) throw new ArgumentNullException(nameof(issuer));
            if (!Enum.IsDefined(typeof(StoreName), storeName)) throw new ArgumentOutOfRangeException(nameof(storeName));
            if (!Enum.IsDefined(typeof(StoreLocation), storeLocation)) throw new ArgumentOutOfRangeException(nameof(storeLocation));

            using (var store = new X509Store(storeName, storeLocation))
            {
                X509Certificate2Collection storeCertificates = null;
                certificate = null;

                try
                {
                    store.Open(OpenFlags.ReadOnly);
                    storeCertificates = store.Certificates;

                    X509Certificate2Collection found = storeCertificates.Find(X509FindType.FindBySubjectDistinguishedName, subject, validOnly);

                    if (found.Count == 0)
                    {
                        return false;
                    }

                    certificate = found
                        .OfType<X509Certificate2>()
                        .Where(c => StringComparer.Ordinal.Equals(c.Issuer, issuer))
                        .OrderByDescending(cert => cert.NotAfter)
                        .FirstOrDefault();

                    return certificate != null;
                }
                finally
                {
                    DisposeCertificates(storeCertificates, certificate);
                }
            }
        }

        /// <summary>
        /// Try load a <see cref="X509Certificate2"/> given its <paramref name="fileName"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="fileName">The file path of the certificate.</param>
        /// <param name="password">The certificate password.</param>
        /// <param name="certificate">If found, the certificate with the specified details.</param>
        /// <returns>Returns true if the specified certificate was found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/>
        /// or
        /// <paramref name="password"/>
        /// </exception>
        public static bool TryLoadCertificate(string fileName, SecureString password, out X509Certificate2 certificate)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (password == null || !password.IsReadOnly()) throw new ArgumentNullException(nameof(password));

            try
            {
                certificate = new X509Certificate2(fileName, password);
                return true;
            }
            catch (CryptographicException)
            {
                certificate = null;
                return false;
            }
        }

        /// <summary>
        /// Trims all characters that are not hexadecimal.
        /// Truncates the resulting output to 40 characters if it is longer than that.
        /// (If the thumbprint was copied from MMC, it likely contains special characters)
        /// </summary>
        /// <param name="thumbprint">The raw thumbprint value to clean.</param>
        public static string NormalizeThumbprint(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
                return string.Empty;

            // Since the expected length is reasonable we use char[] instead of StringBuilder
            var ch = new char[Sha1Length];

            var i = 0;
            for (var j = 0; j < thumbprint.Length
                // We choose to ignore (vs throw) anything beyond the expected number of valid characters
                && i < Sha1Length; j++)
            {
                var c = thumbprint[j];

                if (!IsHex(c))
                    continue;

                ch[i++] = c;
            }

            // Return empty if all characters were invalid
            if (i == 0)
                return string.Empty;

            // Else return the valid substring
            var clean = new string(ch, 0, i);
            return clean;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsHex(char c)
            => (c >= '0' && c <= '9') ||
               (c >= 'A' && c <= 'Z') ||
               (c >= 'a' && c <= 'z');

        private static void DisposeCertificates(X509Certificate2Collection certificates, X509Certificate2 except)
        {
            if (certificates == null || certificates.Count == 0)
                return;

            foreach (X509Certificate2 certificate in certificates)
            {
                if (except == null || !certificate.Equals(except))
                {
                    certificate.Dispose();
                }
            }
        }
    }
}
