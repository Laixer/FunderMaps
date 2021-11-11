namespace FunderMaps.Core.Helpers;

/// <summary>
///     Helpers for file and path related operations.
/// </summary>
public static class FileHelper
{
    /// <summary>
    ///     Generate a unique file name.
    /// </summary>
    /// <param name="fileName">Optional original file for extenion.</param>
    /// <returns></returns>
    public static string GetUniqueName(string fileName = null)
    {
        if (fileName is null || !Path.HasExtension(fileName))
        {
            return Guid.NewGuid().ToString();
        }

        return $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
    }
}
