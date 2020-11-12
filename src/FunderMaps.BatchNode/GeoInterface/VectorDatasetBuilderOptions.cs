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
    }
}
