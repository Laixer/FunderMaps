using System.IO;
using FunderMaps.Core.Threading.Command;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Bundle layer source specialization.
    /// </summary>
    internal class BundleLayerSource : LayerSource
    {
        private readonly string layerOutputName;
        public readonly string query;
        public readonly string _workspace;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleLayerSource(string layer, string workspace, int offset, int limit)
        {
            _workspace = workspace;
            layerOutputName = layer;
            query = $@"
                SELECT  *
                FROM    maplayer.{layer}
                OFFSET  {offset}
                LIMIT   {limit}";
        }

        // Write the SQL to disk and point the command to the sql file.
        // This allows for much larger SQL statements in the future.
        public override void Imbue(CommandInfo commandInfo)
        {
            string path = $"{Path.Combine(_workspace, layerOutputName)}.sql";
            File.WriteAllText(path, query);

            commandInfo.ArgumentList.Add("-sql");
            commandInfo.ArgumentList.Add($"@{path}");
            commandInfo.ArgumentList.Add("-nln");
            commandInfo.ArgumentList.Add(layerOutputName);
        }
    }
}
