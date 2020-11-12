namespace FunderMaps.BatchNode.GeoInterface
{
    /// <summary>
    ///     Vector dataset builder options.
    /// </summary>
    public record VectorDatasetBuilderOptions
    {
        /// <summary>
        ///     Additional options to the command, these options are append to the command.
        /// </summary>
        public string AdditionalOptions { get; init; }

        /// <summary>
        ///     Overwrite current file if exist in the output.
        /// </summary>
        public bool Overwrite { get; init; } = false;

        /// <summary>
        ///     Append to current file if exist.
        /// </summary>
        public bool Append { get; init; } = false;
    }
}
