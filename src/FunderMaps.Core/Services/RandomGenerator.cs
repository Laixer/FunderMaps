using System.Security.Cryptography;

namespace FunderMaps.Core.Components;

/// <summary>
///     Random value generator.
/// </summary>
public class RandomGenerator : IDisposable
{
    private readonly RandomNumberGenerator randomNumbergenerator = RandomNumberGenerator.Create();
    private bool disposedValue = false;

    /// <summary>
    ///     Fills the specified byte array with a cryptographically strong random sequence of values.
    /// </summary>
    /// <param name="data">The array to fill with cryptographically strong random bytes.</param>
    public virtual void WriteBytes(byte[] data)
        => randomNumbergenerator.GetBytes(data);

    /// <summary>
    ///     Fills the specified byte array with a cryptographically strong random sequence of values.
    /// </summary>
    /// <param name="data">The array to fill with cryptographically strong random bytes.</param>
    /// <param name="offset">The index of the array to start the fill operation.</param>
    /// <param name="count">The number of bytes to fill.</param>
    public virtual void WriteBytes(byte[] data, int offset, int count)
        => randomNumbergenerator.GetBytes(data, offset, count);

    /// <summary>
    ///     Get byte array of cryptographically strong random sequence of values.
    /// </summary>
    /// <param name="size">Buffer size.</param>
    /// <returns>Byte array of random data.</returns>
    public virtual byte[] GetRandomByteArray(int size)
    {
        byte[] buffer = new byte[size];
        randomNumbergenerator.GetBytes(buffer);
        return buffer;
    }

    #region Dispose Pattern

    /// <summary>
    ///     Dispose helper.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                randomNumbergenerator.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    ///     Dispose objects.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion Dispose Pattern
}
