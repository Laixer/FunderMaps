namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Provides an abstraction for random nummer generation.
    /// </summary>
    public interface IRandom
    {
        void WriteBytes(byte[] data);

        void WriteBytes(byte[] data, int offset, int count);

        byte[] GetRandomByteArray(int size);
    }
}
