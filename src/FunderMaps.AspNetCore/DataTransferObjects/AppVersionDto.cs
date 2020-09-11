namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Application version DTO.
    /// </summary>
    public sealed class AppVersionDto
    {
        /// <summary>
        ///     Application name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Application version.
        /// </summary>
        public string Version { get; set; }
    }
}
