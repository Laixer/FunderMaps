using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.MapBundle
{
    internal abstract class DataSource
    {
        public GeometryFormat Format { get; set; }

        public virtual string Read(CommandInfo commandInfo) => ToString();

        public virtual string Write(CommandInfo commandInfo) => ToString();
    }
}
