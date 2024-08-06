namespace FunderMaps.Core.Interfaces.Repositories;

public interface ITilesetRepository
{
    Task<byte[]> GetTileAsync(string name, int zoom, int x, int y);
    Task AddTileAsync(string name, int zoom, int x, int y, byte[] tile);
}
