using System;
using System.Collections.Generic;
using System.Linq;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Types.MapLayer;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Bundle layer source specialization.
    /// </summary>
    internal class BundleLayerSource : SqlLayerSource
    {
        private const string GeomColumn = "geom";
        private const string SelectWildcard = "*";

        private readonly string layerOutputName;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleLayerSource(Bundle bundle, Layer layer, string workspace)
        {
            if (bundle.LayerConfiguration.Layers.Where(x => x.LayerId == layer.Id).FirstOrDefault() is not LayerColumnPair configuration)
            {
                // throw new LayerConfigurationException(nameof(bundle.LayerConfiguration.Layers));
                throw new Exception();
            }

            layerOutputName = layer.Slug;

            // Select layer field as follows:
            // - If no column is specified then select everything from the layer
            // - If wildcard is found, then only use wildcard.
            // - If no geometry column was found, then add one.
            List<string> columns = new(configuration.ColumnNames);
            if (columns.Count == 0)
            {
                columns.Add(SelectWildcard);
            }
            else if (columns.Contains(SelectWildcard))
            {
                columns = new List<string> { SelectWildcard };
            }
            else if (!columns.Contains(GeomColumn))
            {
                columns.Add(GeomColumn);
            }

            Workspace = workspace;
            Query = $@"
                SELECT  {string.Join(',', columns.Select(c => $"s.{c}"))}
                FROM    {layer.SchemaName}.{layer.TableName} AS s
                JOIN    application.organization AS o ON o.id = '{bundle.OrganizationId}'
                WHERE   ST_Intersects(o.fence, s.geom)";
        }

        public override void Imbue(CommandInfo commandInfo)
        {
            base.Imbue(commandInfo);

            commandInfo.ArgumentList.Add("-nln");
            commandInfo.ArgumentList.Add(layerOutputName);
        }
    }
}
