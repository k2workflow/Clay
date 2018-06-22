#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Security.Cryptography.X509Certificates;

namespace SourceCode.Clay.Security
{
    /// <summary>
    /// Represents <see cref="X509Certificate2"/> extensions.
    /// </summary>
    public static class CertificateLoader
    {
        /// <summary>
        /// The number of hex characters required to represent a Sha1 thumbprint.
        /// </summary>
        public const byte Sha1Length = 20 * 2;

        /// <summary>
        /// Load a <see cref="X509Certificate2"/> given its store location and thumbprint.
        /// </summary>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="thumbprint">The certificate thumbprint to find.</param>
        /// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false.</param>
        /// <returns>If found, the certificate with the specified thumbprint.</returns>
        public static X509Certificate2 LoadCertificate(StoreName storeName, StoreLocation storeLocation, string thumbprint, bool validOnly)
        {
            if (string.IsNullOrWhiteSpace(thumbprint)) throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint.Length != Sha1Length) throw new FormatException($"Specified thumbprint should be {Sha1Length} characters long.");
            if (!IsValid(thumbprint[0]) || !IsValid(thumbprint[thumbprint.Length - 1])) throw new FormatException($"Invalid character(s) detected in thumbprint.");
            
            var certStore = new X509Store(storeName, storeLocation);
            certStore.Open(OpenFlags.ReadOnly);
            try
            {
                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly);

                if (certCollection.Count == 0)
                    throw new InvalidOperationException($@"Cannot find certificate in {storeName}/{storeLocation} with thumbprint '{thumbprint}'.");

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
        /// Try to load a <see cref="X509Certificate2"/> given its store location and thumbprint.
        /// </summary>
        /// <param name="storeName">The certificate <see cref="StoreName"/> to use.</param>
        /// <param name="storeLocation">The certificate <see cref="StoreLocation"/> to use.</param>
        /// <param name="thumbprint">The certificate thumbprint to find.</param>
        /// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false.</param>
        /// <param name="certificate">If found, the certificate with the specified thumbprint.</param>
        /// <returns>true if the specified certificate was found; otherwise, false.</returns>
        public static bool TryLoadCertificate(StoreName storeName, StoreLocation storeLocation, string thumbprint, bool validOnly, out X509Certificate2 certificate)
        {
            if (string.IsNullOrWhiteSpace(thumbprint)) throw new ArgumentNullException(nameof(thumbprint));
            if (thumbprint.Length != Sha1Length) throw new FormatException($"Specified thumbprint should be {Sha1Length} characters long.");
            if (!IsValid(thumbprint[0]) || !IsValid(thumbprint[thumbprint.Length - 1])) throw new FormatException($"Invalid character(s) detected in thumbprint.");

            var certStore = new X509Store(storeName, storeLocation);
            certStore.Open(OpenFlags.ReadOnly);
            try
            {
                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly);

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
        /// Trims all leading/trailing characters that are not alphanumeric.
        /// Truncates the resulting output to 40 characters if it is longer than that.
        /// (If the thumbprint was copied from MMC, it likely contains special characters)
        /// </summary>
        public static string NormalizeThumbprint(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
                return string.Empty;

            // Since the expected length is reasonable we use stackalloc instead of StringBuilder
            Span<char> ch = stackalloc char[Sha1Length];

            var i = 0;
            for (var j = 0; j < thumbprint.Length 
                // We choose to ignore (vs throw) anything beyond the expected number of valid characters
                && i < Sha1Length; j++)
            {
                var c = thumbprint[j];

                if (!IsValid(c))
                    continue;
                
                ch[i++] = c;
            }

            // Return empty if all characters were invalid
            if (i == 0)
                return string.Empty;

            // Else return the valid substring
            ch = ch.Slice(0, i);

            var clean = new string(ch);
            return clean;
        }

        private static bool IsValid(char c)
            => (c >= '0' && c <= '9') ||
               (c >= 'A' && c <= 'Z') ||
               (c >= 'a' && c <= 'z');
    }
}
