using System;
using System.Collections.Generic;
using System.Linq;
using FunderMaps.BatchNode.Command;
using FunderMaps.BatchNode.GeoInterface;
using FunderMaps.Core.Entities;

namespace FunderMaps.BatchNode.Jobs.BundleBuilder
{
    /// <summary>
    ///     Bundle layer source specialization.
    /// </summary>
    internal class BundleLayerSource : SqlLayerSource
    {
        private readonly string layerOutputName;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleLayerSource(Bundle bundle, Layer layer)
        {
            var configuration = bundle.LayerConfiguration.Layers.Where(x => x.LayerId == layer.Id).FirstOrDefault();
            if (configuration == null)
            {
                // throw new LayerConfigurationException(nameof(bundle.LayerConfiguration.Layers));
                throw new Exception();
            }

            layerOutputName = layer.TableName;

            IEnumerable<string> columns = new List<string>(configuration.ColumnNames);
            if (!columns.Contains("geom"))
            {
                columns.Append("geom");
            }

            Query = $@"
                SELECT
                    {string.Join(',', columns.Select(c => $"s.{c}"))}
                FROM {layer.SchemaName}.{layer.TableName} AS s
                JOIN application.organization AS o ON o.id = '{bundle.OrganizationId}'
                WHERE ST_Intersects(o.fence, s.geom)";
        }

        public override void Imbue(CommandInfo commandInfo)
        {
            base.Imbue(commandInfo);

            commandInfo.ArgumentList.Add("-nln");
            commandInfo.ArgumentList.Add(layerOutputName);
        }
    }
}