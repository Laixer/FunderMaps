using FunderMaps.BatchNode.Command;

namespace FunderMaps.BatchNode.GeoInterface
{
    internal class SqlLayerSource : LayerSource
    {
        public string Query { get; set; }

        // FUTURE: Write the SQL to disk and point the command to the sql file
        //         This allows for much larger SQL statements in the future.
        public override void Imbue(CommandInfo commandInfo)
        {
            if (Query != null)
            {
                commandInfo.ArgumentList.Add("-sql");
                commandInfo.ArgumentList.Add(Query.Replace('\n', ' ').Trim());
            }
        }

        public static SqlLayerSource FromSql(string query)
            => new SqlLayerSource
            {
                Query = query,
            };
    }
}
