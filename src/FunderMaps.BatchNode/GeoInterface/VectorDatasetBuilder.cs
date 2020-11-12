using System;
using FunderMaps.BatchNode.Command;
using FunderMaps.Core.Types;

namespace FunderMaps.BatchNode.GeoInterface
{
    /// <summary>
    ///     Interface the vector dataset command.
    /// </summary>
    internal class VectorDatasetBuilder
    {
        private const string CommandName = "ogr2ogr";

        private DataSource input;
        private DataSource output;
        private LayerSource inputLayer = new LayerSource();

        private VectorDatasetBuilderOptions _options;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public VectorDatasetBuilder(VectorDatasetBuilderOptions options = null)
        {
            _options = options ?? new VectorDatasetBuilderOptions();
        }

        /// <summary>
        ///     Set the input dataset.
        /// </summary>
        /// <param name="input">Data input source.</param>
        public VectorDatasetBuilder InputDataset(DataSource input)
        {
            this.input = input;
            return this;
        }

        /// <summary>
        ///     Set the output dataset.
        /// </summary>
        /// <param name="output">Data output source.</param>
        public VectorDatasetBuilder OutputDataset(DataSource output)
        {
            this.output = output;
            return this;
        }

        /// <summary>
        ///     Set the input layer filter.
        /// </summary>
        /// <param name="input">Input layer selector.</param>
        public VectorDatasetBuilder InputLayers(LayerSource input)
        {
            inputLayer = input ?? new LayerSource();
            return this;
        }

        public static (string, string) ExportFormatTuple(GeometryExportFormat format)
            => format switch
            {
                GeometryExportFormat.MapboxVectorTiles => ("MVT", ""),
                GeometryExportFormat.GeoPackage => ("GPKG", ".gpkg"),
                GeometryExportFormat.ESRIShapefile => ("ESRI Shapefile", ".shp"),
                GeometryExportFormat.GeoJSON => ("GeoJSON", ".json"),
                _ => throw new InvalidOperationException(nameof(format)),
            };

        /// <summary>
        ///     Build the command from builder parts.
        /// </summary>
        /// <remarks>
        ///     Keep the order in which arguments needs to be passed to the command.
        /// </remarks>
        /// <returns>The <see cref="CommandInfo"/> to be executed.</returns>
        public CommandInfo Build()
        {
            var command = new CommandInfo(CommandName);
            command.ArgumentList.Add("-overwrite");
            command.ArgumentList.Add("-f");
            command.ArgumentList.Add(ExportFormatTuple(output.Format).Item1);
            command.ArgumentList.Add(output.Write(command));
            command.ArgumentList.Add(input.Read(command));

            inputLayer.Imbue(command);

            if (!string.IsNullOrEmpty(_options.AdditionalOptions))
            {
                foreach (var argument in _options.AdditionalOptions.Split(" "))
                {
                    command.ArgumentList.Add(argument.Trim());
                }
            }

            return command;
        }
    }
}
