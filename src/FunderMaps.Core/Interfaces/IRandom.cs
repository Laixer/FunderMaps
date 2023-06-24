namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Provides an abstraction for random nummer generation.
/// </summary>
public interface IRandom
{
    /// <summary>
    ///     Write random data to byte array.
    /// </summary>
    /// <param name="data">Array to write to.</param>
    void WriteBytes(byte[] data);

    /// <summary>
    ///     Write random data to byte array with offset and count.
    /// </summary>
    /// <param name="data">Array to write to.</param>
    /// <param name="offset">Offset in array.</param>
    /// <param name="count">Number of bytes to write.</param>
    void WriteBytes(byte[] data, int offset, int count);

    /// <summary>
    ///     Return array of random bytes with <paramref name="size"/>.
    /// </summary>
    /// <param name="size">Size of byte array to return.</param>
    byte[] GetRandomByteArray(int size);
}
