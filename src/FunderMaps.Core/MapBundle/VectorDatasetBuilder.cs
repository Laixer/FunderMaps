using FunderMaps.Core.Threading.Command;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Interface the vector dataset command.
    /// </summary>
    internal class VectorDatasetBuilder
    {
        private const string CommandName = "ogr2ogr";

        private DataSource input;
        private DataSource output;
        private LayerSource inputLayer = new();

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

        /// <summary>
        ///     Build the command from builder parts.
        /// </summary>
        /// <remarks>
        ///     Keep the order in which arguments needs to be passed to the command.
        /// </remarks>
        /// <returns>The <see cref="CommandInfo"/> to be executed.</returns>
        public CommandInfo Build(string formatName)
        {
            CommandInfo command = new(CommandName);

            command.ArgumentList.Add("-f");
            command.ArgumentList.Add(formatName);
            command.ArgumentList.Add(output.Write(command));
            command.ArgumentList.Add(input.Read(command));

            inputLayer.Imbue(command);

            return command;
        }
    }
}
