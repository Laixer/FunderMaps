using System;
using SystemPath = System.IO.Path;

namespace FunderMaps.Core.IO
{
    /// <summary>
    ///     System.IO.Path extensions.
    /// </summary>
    public static class Path
    {
        /// <summary>
        ///     Generate a unique file name.
        /// </summary>
        /// <param name="fileName">Optional original file for extenion.</param>
        /// <returns></returns>
        public static string GetUniqueName(string fileName = null)
        {
            if (fileName == null || !SystemPath.HasExtension(fileName))
            {
                return Guid.NewGuid().ToString();
            }

            return $"{Guid.NewGuid()}{SystemPath.GetExtension(fileName)}";
        }
    }
}
