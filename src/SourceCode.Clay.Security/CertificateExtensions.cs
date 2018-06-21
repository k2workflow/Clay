#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace SourceCode.Clay.Security
{
    /// <summary>
    /// Represents <see cref="X509Certificate2"/> extensions.
    /// </summary>
    public static class CertificateExtensions
    {
        // Sha1 hex character length (20 bytes)
        internal const int Sha1HexLen = 20 * 2; // Declared as internal for units

        // Permit N extra special characters
        private const int JunkLen = 6;

        /// <summary>
        /// Load a <see cref="X509Certificate2"/> from My/CurrentUser given its thumbprint.
        /// </summary>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="thumbprint">The certificate thumbprint to find.</param>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false.</param>
        /// <returns>If found, the certificate with the specified thumbprint.</returns>
        public static X509Certificate2 LoadCertificate(this StoreLocation storeLocation, string thumbprint, StoreName storeName = StoreName.My, bool validOnly = true)
        {
            if (string.IsNullOrWhiteSpace(thumbprint)) throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint.Length > Sha1HexLen + JunkLen) throw new ArgumentException($"Specified {nameof(thumbprint)} should be exactly {Sha1HexLen} characters long.", nameof(thumbprint));

            var certStore = new X509Store(storeName, storeLocation);
            certStore.Open(OpenFlags.ReadOnly);
            try
            {
                var clean = Clean(thumbprint);
                if (thumbprint.Length != Sha1HexLen) throw new ArgumentException($"Specified {nameof(thumbprint)} should be exactly {Sha1HexLen} characters long.", nameof(thumbprint));

                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, clean, validOnly);

                if (certCollection.Count == 0)
                    throw new ArgumentException($@"Cannot find certificate with thumbprint '{clean}' in {storeName}/{storeLocation}.", nameof(thumbprint));

                // By convention we return the first certificate
                var certificate = certCollection[0];
                return certificate;
            }
            finally
            {
                certStore.Close();
            }
        }

        /// <summary>
        /// Load a <see cref="X509Certificate2"/> from My/CurrentUser given its thumbprint.
        /// </summary>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="thumbprint">The certificate thumbprint to find.</param>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false.</param>
        /// <param name="certificate">If found, the certificate with the specified thumbprint.</param>
        /// <returns>true if the specified certificate was found; otherwise, false.</returns>
        public static bool TryLoadCertificate(this StoreLocation storeLocation, string thumbprint, out X509Certificate2 certificate, StoreName storeName = StoreName.My, bool validOnly = true)
        {
            if (string.IsNullOrWhiteSpace(thumbprint)) throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint.Length > Sha1HexLen + JunkLen) throw new ArgumentException($"Specified {nameof(thumbprint)} should be exactly {Sha1HexLen} characters long.", nameof(thumbprint));

            var certStore = new X509Store(storeName, storeLocation);
            certStore.Open(OpenFlags.ReadOnly);
            try
            {
                var clean = Clean(thumbprint);
                if (thumbprint.Length != Sha1HexLen)
                {
                    certificate = null;
                    return false;
                }

                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, clean, validOnly);

                if (certCollection.Count == 0)
                {
                    certificate = null;
                    return false;
                }

                // By convention we return the first certificate
                certificate = certCollection[0];
                return true;
            }
            finally
            {
                certStore.Close();
            }
        }

        /// <summary>
        /// If the thumbprint was copied from MMC, it likely contains special characters
        /// </summary>
        internal static string Clean(string thumbprint) // Declared as internal for units
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(thumbprint));

            // We already know the length is reasonable, so use stackalloc instead of a StringBuilder
            Span<char> sb = stackalloc char[thumbprint.Length];

            var i = 0;
            foreach (var c in thumbprint)
            {
                if ((c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    c == '.' ||
                    c == '_')
                {
                    sb[i++] = c;
                }
            }

            // Return empty if all characters were invalid
            if (i == 0)
                return string.Empty;

            // Else return the valid substring
            sb = sb.Slice(0, i);
            var clean = new string(sb);

            return clean;
        }
    }
}
