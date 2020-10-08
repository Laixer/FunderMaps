﻿using FunderMaps.Core.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace FunderMaps.Core.Components
{
    /// <summary>
    ///     Password hasher.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        private const int iterRounds = 10000;
        private const int subkeyLength = 256 / 8; // 256 bits
        private const int saltSize = 128 / 8; // 128 bits
        private const byte formatMarker = 0x01;

        private readonly IRandom _random;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public PasswordHasher(IRandom random)
        {
            _random = random;
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5379:Do Not Use Weak Key Derivation Function Algorithm", Justification = "<Pending>")]
        private byte[] GeneratePasswordHash(string password)
        {
            byte[] salt = new byte[saltSize];
            _random.WriteBytes(salt);
            //_rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterRounds, hashAlgorithm);
            byte[] subkey = pbkdf2.GetBytes(subkeyLength);

            byte[] outputBytes = new byte[1 + saltSize + subkeyLength];
            outputBytes[0] = formatMarker;
            Buffer.BlockCopy(salt, 0, outputBytes, 1, saltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + saltSize, subkeyLength);
            return outputBytes;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5379:Do Not Use Weak Key Derivation Function Algorithm", Justification = "<Pending>")]
        private static bool VerifyHashedPassword(byte[] inputBytes, string password)
        {
            byte[] salt = new byte[saltSize];
            Buffer.BlockCopy(inputBytes, 1, salt, 0, saltSize);

            byte[] expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(inputBytes, 1 + saltSize, expectedSubkey, 0, subkeyLength);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterRounds, hashAlgorithm);
            byte[] subkey = pbkdf2.GetBytes(subkeyLength);

            return ByteArraysEqual(subkey, expectedSubkey);
        }

        /// <summary>
        ///     Hash plaintext paassword and return the password hash.
        /// </summary>
        /// <param name="password">Plaintext password.</param>
        /// <returns>Returns hashed password</returns>
        public string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            return Convert.ToBase64String(GeneratePasswordHash(password));
        }

        /// <summary>
        ///     Check if password is valid.
        /// </summary>
        /// <param name="hashedPassword">Password hash.</param>
        /// <param name="providedPassword">Plaintext password to test.</param>
        /// <returns>Returns <c>true</c> if passwords match, false otherwise.</returns>
        public bool IsPasswordValid(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            if (decodedHashedPassword[0] != formatMarker)
            {
                return false;
            }

            return VerifyHashedPassword(decodedHashedPassword, providedPassword);
        }
    }
}