using System.IO;
using FunderMaps.Core.Threading.Command;

namespace FunderMaps.Core.MapBundle
{
    internal class SqlLayerSource : LayerSource
    {
        public string Query { get; set; }
        public string Workspace { get; set; }

        public override void Imbue(CommandInfo commandInfo)
        {
            if (Query is not null)
            {
                // Write the SQL to disk and point the command to the sql file.
                // This allows for much larger SQL statements in the future.
                if (Workspace is not null)
                {
                    Query += '\n';
                    var path = $"{Path.Combine(Workspace, Path.GetRandomFileName())}.sql";
                    File.WriteAllText(path, Query);

                    commandInfo.ArgumentList.Add("-sql");
                    commandInfo.ArgumentList.Add($"@{path}");
                }
                else
                {
                    commandInfo.ArgumentList.Add("-sql");
                    commandInfo.ArgumentList.Add(Query.Replace('\n', ' ').Trim());
                }
            }
        }
    }
}
