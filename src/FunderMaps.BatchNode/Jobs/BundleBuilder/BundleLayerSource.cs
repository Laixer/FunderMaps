using System;
using System.Collections.Generic;
using System.Linq;
using FunderMaps.BatchNode.Command;
using FunderMaps.BatchNode.GeoInterface;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types.MapLayer;

namespace FunderMaps.BatchNode.Jobs.BundleBuilder
{
    /// <summary>
    ///     Bundle layer source specialization.
    /// </summary>
    internal class BundleLayerSource : SqlLayerSource
    {
        private const string GeomColumn = "geom";

        private readonly string layerOutputName;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleLayerSource(Bundle bundle, Layer layer)
        {
            if (bundle.LayerConfiguration.Layers.Where(x => x.LayerId == layer.Id).FirstOrDefault() is not LayerColumnPair configuration)
            {
                // throw new LayerConfigurationException(nameof(bundle.LayerConfiguration.Layers));
                throw new Exception();
            }

            layerOutputName = layer.Slug;

            if (configuration.ColumnNames.Any() && configuration.ColumnNames.First() != "*")
            {
                // Always include geometry column.
                List<string> columns = new(configuration.ColumnNames);
                if (!columns.Contains(GeomColumn))
                {
                    columns.Add(GeomColumn);
                }

                Query = $@"
                    SELECT  {string.Join(',', columns.Select(c => $"s.{c}"))}
                    FROM    {layer.SchemaName}.{layer.TableName} AS s
                    JOIN    application.organization AS o ON o.id = '{bundle.OrganizationId}'
                    WHERE   ST_Intersects(o.fence, s.geom)";
            }
            else
            {
                Query = $@"
                    SELECT  *
                    FROM    {layer.SchemaName}.{layer.TableName} AS s
                    JOIN    application.organization AS o ON o.id = '{bundle.OrganizationId}'
                    WHERE   ST_Intersects(o.fence, s.geom)";
            }
        }

        public override void Imbue(CommandInfo commandInfo)
        {
            base.Imbue(commandInfo);

            commandInfo.ArgumentList.Add("-nln");
            commandInfo.ArgumentList.Add(layerOutputName);
        }
    }
}