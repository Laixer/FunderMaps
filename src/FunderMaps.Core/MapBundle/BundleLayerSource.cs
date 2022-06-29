using FunderMaps.Core.Threading.Command;

namespace FunderMaps.Core.MapBundle;

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
    public BundleLayerSource(string layer, string workspace, int? offset = null, int? limit = null)
    {
        _workspace = workspace;
        layerOutputName = layer;

        if (offset is not null && limit is not null)
        {
            query = $@"
                SELECT  *
                FROM    maplayer.{layer}
                OFFSET  {offset}
                LIMIT   {limit}";
        }
        else
        {
            query = $@"
                SELECT  *
                FROM    maplayer.{layer}";
        }
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
