using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Datasource interface.
    /// </summary>
    internal abstract class DataSource
    {
        /// <summary>
        ///     Conversion format.
        /// </summary>        
        public GeometryFormat Format { get; set; }

        public virtual string Read(CommandInfo commandInfo) => ToString();

        public virtual string Write(CommandInfo commandInfo) => ToString();
    }
}
