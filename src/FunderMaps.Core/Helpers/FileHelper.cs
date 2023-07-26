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
    /// <returns>Newly generated file name.</returns>
    public static string GetUniqueName(string? fileName = null)
    {
        if (fileName is null || !Path.HasExtension(fileName))
        {
            return Guid.NewGuid().ToString();
        }

        return $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
    }

    /// <summary>
    ///     Delete all files with a given extension in a directory.
    /// </summary>
    /// <param name="path">Directory path.</param>
    /// <param name="extension">File extension.</param>
    public static void DeleteFilesWithExtension(string path, string extension)
    {
        foreach (string file in Directory.EnumerateFiles(path, $"*.{extension}"))
        {
            File.Delete(file);
        }
    }
}
