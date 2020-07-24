using FunderMaps.Core.Interfaces;
using System;
using System.Security.Cryptography;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Random value generator.
    /// </summary>
    public class RandomGenerator : IRandom, IDisposable
    {
        private readonly RandomNumberGenerator _rng;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RandomGenerator()
        {
            _rng ??= new RNGCryptoServiceProvider();
        }

        /// <summary>
        ///     Fills the specified byte array with a cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="data">The array to fill with cryptographically strong random bytes.</param>
        public virtual void WriteBytes(byte[] data)
        {
            _rng.GetBytes(data);
        }

        /// <summary>
        ///     Fills the specified byte array with a cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="data">The array to fill with cryptographically strong random bytes.</param>
        /// <param name="offset">The index of the array to start the fill operation.</param>
        /// <param name="count">The number of bytes to fill.</param>
        public virtual void WriteBytes(byte[] data, int offset, int count)
        {
            _rng.GetBytes(data, offset, count);
        }

        /// <summary>
        ///     Get byte array of cryptographically strong random sequence of values.
        /// </summary>
        /// <param name="size">Buffer size.</param>
        /// <returns>Byte array of random data.</returns>
        public virtual byte[] GetRandomByteArray(int size)
        {
            byte[] buffer = new byte[size];
            _rng.GetBytes(buffer);
            return buffer;
        }

        public virtual void Dispose()
        {
            _rng.Dispose();
        }
    }
}
