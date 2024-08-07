namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Represents a repository for managing tilesets.
/// </summary>
public interface ITilesetRepository
{
    /// <summary>
    ///     Retrieves a tile from the repository.
    /// </summary>
    /// <param name="name">The name of the tileset.</param>
    /// <param name="zoom">The zoom level of the tile.</param>
    /// <param name="x">The x-coordinate of the tile.</param>
    /// <param name="y">The y-coordinate of the tile.</param>
    /// <returns>The byte array representing the tile.</returns>
    Task<byte[]> GetTileAsync(string name, int zoom, int x, int y);

    /// <summary>
    ///     Adds a tile to the repository.
    /// </summary>
    /// <param name="name">The name of the tileset.</param>
    /// <param name="zoom">The zoom level of the tile.</param>
    /// <param name="x">The x-coordinate of the tile.</param>
    /// <param name="y">The y-coordinate of the tile.</param>
    /// <param name="tile">The byte array representing the tile.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddTileAsync(string name, int zoom, int x, int y, byte[] tile);
}
